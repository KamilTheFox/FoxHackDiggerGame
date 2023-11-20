using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Il2CppSystem.Threading;
using UnityEngine;

public class Building : IHackMenu, IUpdate, IGUIElement, IStarted
{
    public string Name => "Building";

    private bool isFastDig, isFastBuild;

    private float SpeedDig = 1f;

    private static bool BlocksInInventory;

    private bool MassBuild, CurCenter, CursorNormalise, isBuildAxis;

    private float MassX = 1, MassY = 1, MassZ = 1;


    private MyIntVect[] MassBlock;

    private float LimitFilling = 31;


    private List<IHackMenu> buildsMenus = new List<IHackMenu>()
    {
        new MenuBlocks(),
        new MenuSelect(),
        new MenuSaveBuild(),
        new SpoofMenu(),
        new MenuGenerate()
    };


    private bool isRoomFilling;

    public List<BlockInfo> Build = new List<BlockInfo>();

    public static Building Instance { get; private set; }

    private List<MenuSelect.TimePostBuild> returnBuffer = new List<MenuSelect.TimePostBuild>();
    public void Start()
    {
        Instance = this;
        foreach(IStarted started in buildsMenus)
        {
            started.Start();
        }
    }
    public void ResetMenuRect()
    {
        foreach (var menu in buildsMenus)
            menu.Rect = new Rect(menu.Rect.position, new Vector2(200, 150));
    }

    public Action<int> Menu => (id) =>
    {
        if (GUILayout.Button("Destroy: " + (isFastDig ? "On" : "Off"), null))
        {
            isFastDig = !isFastDig;
        }
        if (GUILayout.Button("Build: " + (isFastBuild ? "On" : "Off"), null))
        {
            isFastBuild = !isFastBuild;
        }
        MassBuild = GUILayout.Toggle(MassBuild, "MassBlock", null);
        if(MassBuild)
        {
            GUILayout.Label("X: " + (int)(MassX), null);
            MassX = GUILayout.HorizontalSlider(MassX, 1, 20, null);
            GUILayout.Label("Y: " + (int)(MassY), null);
            MassY = GUILayout.HorizontalSlider(MassY, 1, 20, null);
            GUILayout.Label("Z: " + (int)(MassZ), null);
            MassZ = GUILayout.HorizontalSlider(MassZ, 1, 20, null);
            if(GUI.changed)
            {
                MassBlock = BuildFoxAPI.ArrayCube(new MyIntVect((int)MassX, (int)MassY, (int)MassZ));
            }
        }
        if(!MassBuild && GUI.changed)
        {
            Rect = new Rect(Rect.left, Rect.top, Rect.width, 200);
        }
        CurCenter = GUILayout.Toggle(CurCenter, "isCenter", null);
        CursorNormalise = GUILayout.Toggle(CursorNormalise, "CursorNormalise", null);
        isBuildAxis = GUILayout.Toggle(isBuildAxis, "BuildAxis", null);
        GUILayout.Label("SpeedBuild: " + (int)SpeedDig, null);
        SpeedDig = GUILayout.HorizontalSlider(SpeedDig, 1F, 20F, null);
        GUILayout.Label("count block: " + Build.Count,null);
        
        GUILayoutUtility.GetRect(1f, 3f);
        Rect rect6 = GUILayoutUtility.GetRect(10f, 20f);
        GUI.Box(new Rect(rect6.left - 5f, rect6.top, rect6.width + 10f, 114f), "Casting");
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        if (GUILayout.Button("[B]" + (this.FillBuild ? "On" : "Off"), new GUILayoutOption[0]))
        {
            FillDestroy = false;
            FillBuild = !FillBuild;
        }
        if (GUILayout.Button("[D]" + (this.FillDestroy? "On" : "Off"), new GUILayoutOption[0]))
        {
            FillBuild = false;
            FillDestroy = !FillDestroy;
        }
        GUILayout.EndHorizontal();
        isRoomFilling = GUILayout.Toggle(isRoomFilling, "IsRoomFill", null);
        GUILayout.Label("Limit: " + (LimitFilling == 31 ? "Unlimit" : ((int)LimitFilling).ToString()), null);
        LimitFilling = GUILayout.HorizontalSlider(LimitFilling, 1, 31, null);
        if (GUI.changed)
        {
            BuildFoxAPI.SetLimitedForFilling = (LimitFilling > 30 ? null : (int?)LimitFilling);
        }
        GUILayoutUtility.GetRect(1f, 5f);
        GUIF.BoxCast.ChildMenu("Return", () =>
        {
            if(GUILayout.Button($"StartReturn: {ReturnBuildComponent.GetIndex}", null))
            {
                returnBuffer.Add(new MenuSelect.TimePostBuild(ReturnBuildComponent.GetReturn(), 35, MyIntVect.zero) { isReturnProcess = true });
            }
            if (GUILayout.Button($"ClearAll", null))
            {
                ReturnBuildComponent.ClearBuffer();
            }
        });
        BlocksInInventory = GUILayout.Toggle(BlocksInInventory, "BlocksIsInventory", null);
        foreach (var menu in buildsMenus)
        {
            menu.IsActive = GUILayout.Toggle(menu.IsActive, "Menu: " + menu.Name, null);
        }
        GUI.DragWindow();
    };

    
    public void Update()
    {
        if (Input.GetKeyDown(SettingsMenu.InputBuildMenu) && !RenameHash.isActiveChat)
            IsActive = !IsActive;
        for (int i = 0; i < returnBuffer.Count; i++)
        {
            if (returnBuffer[i].Update())
                returnBuffer.RemoveAt(i);
        }
        foreach (var menu in buildsMenus)
        {
            if (menu is IUpdate update)
                update.Update();
        }
        if (RenameHash.isActiveMenu || RenameHash.isActiveChat)
            return;
        if (Input.GetKeyDown(SettingsMenu.CreateCube))
        {
            BuildAPI.AddBlockRPC(GameObject.Find("MAIN_PLAYER").transform.position + Vector3.down, (BlocksInInventory ? BuildAPI.GetBlocksInSellected().Item1 : MenuBlocks.BlockType), BlockKind.Default);
        }
        BuildDir();
    }
    
    private void BuildDir()
    {
        Ray ray = BuildAPI.GetRayInCamera();
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, 1 << LayerMask.NameToLayer("Terrain") | 1 << LayerMask.NameToLayer("Water")))
        {
            Vector3 vect = hit.point + ray.direction.normalized * 0.01f;
            
            if ((FillBuild || FillDestroy)  && RenameHash.LevelF.IsBuilder(RenameHash.MyName))
            {
                BlockInfo infoFillStart = new BlockInfo();
                infoFillStart += vect;
                infoFillStart.BlockType = (int)BuildAPI.GetBlockType(vect);
                infoFillStart.BlockKind = (int)BuildAPI.GetBlockKind(vect);
                if (infoFillStart.BlockType != 0)
                {
                    if (FillBuild && Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        BuildFoxAPI.Filling(infoFillStart, isRoomFilling ? (MyIntVect?)null : hit.normal);
                    }
                    if (FillDestroy && Input.GetKeyDown(KeyCode.Mouse0))
                        BuildFoxAPI.Filling(infoFillStart, null, true);
                }
            }
            if (Time.frameCount % (int)(21 - SpeedDig) == 0 || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (Input.GetKey(KeyCode.Mouse1) && isFastBuild)
                {
                    if (CursorNormalise)
                        vect = hit.point + hit.normal * 0.01f;
                    var BlocksSellected = BuildAPI.GetBlocksInSellected();
                    if (MassBuild && MassBlock != null)
                        foreach (var intVect in MassBlock)
                        {
                            MyIntVect vect1 = intVect;
                            if (isBuildAxis)
                                vect1 = BuildFoxAPI.GetBuildAxis(intVect, hit.normal);
                            BuildAPI.AddBlockRPC(GetCenter(vect, vect1),
                                (BlocksInInventory ? BlocksSellected.Item1 : MenuBlocks.BlockType), (BlocksInInventory ? BlocksSellected.Item2 : MenuBlocks.BlockKind));
                        }
                    else
                        BuildAPI.AddBlockRPC(vect, (BlocksInInventory ? BlocksSellected.Item1 : MenuBlocks.BlockType), (BlocksInInventory ? BlocksSellected.Item2 : MenuBlocks.BlockKind));
                }
                if (Input.GetKey(KeyCode.Mouse0) && isFastDig)
                {
                    if (MassBuild && MassBlock != null)
                        foreach (var intVect in MassBlock)
                        {
                            MyIntVect vect1 = intVect;
                            if (isBuildAxis)
                                vect1 = BuildFoxAPI.GetBuildAxis(intVect, hit.normal);
                            BuildAPI.RemoveBlockRPC(GetCenter(vect, vect1));
                        }
                    else
                        BuildAPI.RemoveBlockRPC(vect);
                }
            }
        }
    }
    
    public MyIntVect GetCenter(MyIntVect point, MyIntVect vectBlock)
    {
        MyIntVect max = new MyIntVect((int)MassX, (int)MassY, (int)MassZ);
        if (CurCenter)
        {
            return point + new MyIntVect(vectBlock.x - max.x / 2, vectBlock.z - max.z / 2, vectBlock.y);
        }
        return point + vectBlock;
    }
    public void OnGUI()
    {
        int i = 2;
        System.Action<int> act;
        foreach (var menu in buildsMenus)
        {
            if (menu.IsActive)
            {
                act = (id) => { GUI.color = SettingsMenu.ColorContent; };
                act += menu.Menu;
                menu.Rect = GUILayout.Window(HookFox.Instance.CountMenu + i + 2 ,menu.Rect , act, menu.Name, null);
                if (menu is IGUIElement update)
                    update.OnGUI();
            }
            i++;
        }
        if (!HookFox.IsServerGame) return;
        Ray ray = BuildAPI.GetRayInCamera();
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, 1 << LayerMask.NameToLayer("Terrain") | 1 << LayerMask.NameToLayer("Water")))
        {
            Vector3 vect = hit.point + ray.direction.normalized * 0.01f;
            if (CursorNormalise)
                vect = hit.point + hit.normal * 0.01f;
            if (MassBuild && MassBlock != null)
            {
                foreach(var intVect in MassBlock)
                    {
                        if ((intVect.x == 0 && intVect.y == 0) ||
                            (intVect.y == 0 && intVect.z == 0) || (intVect.z == 0 && intVect.x == 0) ||
                            (intVect.x > MassX - 1 && intVect.y > MassY - 1) || (intVect.y > MassY - 1 && intVect.z > MassZ - 1) ||
                            (intVect.z > MassZ - 1 && intVect.x > MassX - 1))
                        {
                            MyIntVect vect1 = intVect;
                            if (isBuildAxis)
                            vect1 = BuildFoxAPI.GetBuildAxis(intVect, hit.normal);
                            BuildFoxAPI.DrawStringToDislay(GetCenter(vect, vect1), "■");
                        }
                    }
            }
        }
    }

    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(100,100,200,100);
    public bool FillBuild { get; private set; }
    public bool FillDestroy { get; private set; }
}

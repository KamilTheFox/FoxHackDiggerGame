using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using ChatF = RenameHash.ChatF;

public class MenuSelect : IHackMenu, IUpdate, IGUIElement, IStarted
{
    public string Name => "Select";

    private MyIntVect? pointMass1, pointMass2, PointPos;

    private bool Sellect, postCpyBuild, SelectPoint, b_ViewPos, isPause, IsDestroyUpToDown;

    private string DublTime = "1";

    private List<TimePostBuild> timePostBuilds = new List<TimePostBuild>();

    public class TimePostBuild
        {
        public TimePostBuild(BlockInfo[] build, int Dubl, MyIntVect pointPos, bool isDestroy = false, ReturnBuildComponent returnBuild = null)
        {
            List<BlockInfo> blocks = new List<BlockInfo>();
            foreach (BlockInfo b in build)
            {
                blocks.Add(b.Copy());
            }
            if(returnBuild != null)
                this.returnBuild = returnBuild;
            Build = blocks.ToArray();
            dubl = Dubl;
            block = 0;
            pos = pointPos;
            this.isDestroy = isDestroy;
        }
        ReturnBuildComponent returnBuild;

        public bool isReturnProcess;

        private bool isDestroy;
        private MyIntVect pos;
        private int dubl;
        private BlockInfo[] Build;
        private int block;

        public void StopTimer()
        {
            if (returnBuild != null)
                returnBuild.Dispose();
        }
        public bool Update()
        {
            if (Time.frameCount % 5 == 0)
            {
                for (int i = 0 ; i <dubl; i ++)
                {
                    if (Build[block] != null)
                    {
                        if (returnBuild != null)
                            returnBuild.SetInfoBlock(pos + (MyIntVect)Build[block]);
                        if (isDestroy)
                            BuildAPI.RemoveBlockRPC(pos + (MyIntVect)Build[block]);
                        else
                            BuildAPI.AddBlockRPC(pos + (MyIntVect)Build[block], (BlockType)Build[block].BlockType, (BlockKind)Build[block].BlockKind);
                    }
                    block++;
                }
            }
            return block >= Build.Count();
        }
    }
    private List<BlockInfo> GetBuild => Building.Instance.Build;
    private void CopyBuild()
    {
        GetBuild.Clear();
        if (pointMass1 == null || pointMass2 == null)
        {
            ChatF.Warning("Sellect Point", false);
            return;
        }

        var point = pointMass1.Value;
        var point2 = pointMass2.Value;
        BuildFoxAPI.SwichPoint(ref point, ref point2);
        PointPos = point;
        Building.Instance.Build = BuildFoxAPI.CopyBuild(pointMass1.Value, pointMass2.Value).ToList();
    }
    public Tuple<MyIntVect, MyIntVect> GetPoints()
    {
        MyIntVect intVect1 = pointMass1.Value;
        MyIntVect intVect2 = pointMass2.Value;
        BuildFoxAPI.SwichPoint(ref intVect1, ref intVect2);
        return Tuple.Create(intVect1, intVect2);
    }
    public System.Action<int> Menu => (id) =>
    {
        Sellect = GUILayout.Toggle(Sellect, "Sellect", "Button", null);

        if (GUILayout.Button("Copy", null))
        {
            CopyBuild();
        }
        Rect rect6 = GUILayoutUtility.GetRect(10f, 20f);
        GUI.Box(new Rect(rect6.left - 5f, rect6.top, rect6.width + 10f, 50f), "Rotate");
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        if (GUILayout.Button("X", null))
        {
            BuildFoxAPI.RotateBuild(GetBuild.ToArray(), true, false, false, false);
        }
        if (GUILayout.Button("Y", null))
        {
            BuildFoxAPI.RotateBuild(GetBuild.ToArray(), false, true, false, false);
        }
        if (GUILayout.Button("Z", null))
        {
            BuildFoxAPI.RotateBuild(GetBuild.ToArray(), false, false, true, false);
        }
        if (GUILayout.Button("90", null))
        {
            BuildFoxAPI.RotateBuild(GetBuild.ToArray(), false, false, false, true);
        }
        if (GUILayout.Button("Q", null))
        {
            BuildFoxAPI.RotateBuild(GetBuild.ToArray(), false, false, false, false, true);
        }
        if (GUILayout.Button("W", null))
        {
            BuildFoxAPI.RotateBuild(GetBuild.ToArray(), false, false, false, false, false, true);
        }
        GUILayout.EndHorizontal();
        postCpyBuild = GUILayout.Toggle(postCpyBuild, "PostCursor", "Button", null);
        Rect rectSpoof = GUILayoutUtility.GetRect(10f, 20f);
        GUI.Box(new Rect(rectSpoof.left - 5f, rectSpoof.top, rectSpoof.width + 10f, 50f), "Spoof");
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        if (GUILayout.Button("Start", null))
        {
            BlockInfo[] blocks = BuildFoxAPI.CopyBuild(pointMass1.Value, pointMass2.Value, false, true);
            if (int.TryParse(DublTime, out int Duble))
            {
                timePostBuilds.Add(new TimePostBuild(blocks, Duble, GetPoints().Item1,false, new ReturnBuildComponent()));
            }
        }
        if (GUILayout.Button("NotTime", null))
        {
            BlockInfo[] blocks = BuildFoxAPI.CopyBuild(pointMass1.Value, pointMass2.Value, false, true);
            using (var ret = new ReturnBuildComponent())
                foreach (var item in blocks)
                {
                    ret.SetInfoBlock((MyIntVect)item + GetPoints().Item1);
                    BuildAPI.AddBlockRPC((MyIntVect)item + GetPoints().Item1, (BlockType)item.BlockType, (BlockKind)item.BlockKind);
                }
        }
        GUILayout.EndHorizontal();
        Rect rectDestroy = GUILayoutUtility.GetRect(10f, 20f);
        GUI.Box(new Rect(rectDestroy.left - 5f, rectDestroy.top, rectDestroy.width + 10f, 50f), "DestroySelect");
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        if (GUILayout.Button("Start", null))
        {
            BlockInfo[] blocks = BuildFoxAPI.CopyBuild(pointMass1.Value, pointMass2.Value, true);
            if (int.TryParse(DublTime, out int Duble))
            {
                timePostBuilds.Add(new TimePostBuild(blocks, Duble, GetPoints().Item1, true, new ReturnBuildComponent()));
            }
        }
        if (GUILayout.Button("NotTime", null))
        {
            BlockInfo[] blocks = BuildFoxAPI.CopyBuild(pointMass1.Value, pointMass2.Value, true);
            using (var ret = new ReturnBuildComponent())
                foreach (var item in blocks)
                {
                    ret.SetInfoBlock((MyIntVect)item + GetPoints().Item1);
                    BuildAPI.RemoveBlockRPC((MyIntVect)item + GetPoints().Item1);
                }
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Clear", null))
        {
            GetBuild.Clear();
        }
        if (GUILayout.Button("PointPosition: " + (SelectPoint? "On" : "Off"), null))
        {
            SelectPoint = !SelectPoint;
        }
        if (GUILayout.Button("ViewToPoint: " + (b_ViewPos ? "On" : "Off"), null))
        {
            b_ViewPos = !b_ViewPos;
        }
        IsDestroyUpToDown = GUILayout.Toggle(IsDestroyUpToDown, "UpToDown", null);
        GUILayout.Label("Repeat Block", null);
        DublTime = GUILayout.TextField(DublTime, null);
        Rect rectBuild = GUILayoutUtility.GetRect(10f, 20f);
        GUI.Box(new Rect(rectBuild.left - 5f, rectBuild.top, rectBuild.width + 10f, 50f), "Build");
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        if (GUILayout.Button("Start", null))
        {
            if (int.TryParse(DublTime, out int Duble) && PointPos != null)
            {
                timePostBuilds.Add(new TimePostBuild(GetBuild.ToArray(), Duble, PointPos.Value, false, new ReturnBuildComponent()));
            }
        }
        if (GUILayout.Button("NotTime", null))
        {
            using (var ret = new ReturnBuildComponent())
                foreach (var item in GetBuild)
                {
                    ret.SetInfoBlock((MyIntVect)item + PointPos.Value);
                    BuildAPI.AddBlockRPC((MyIntVect)item + PointPos.Value, (BlockType)item.BlockType, (BlockKind)item.BlockKind);
                }
        }
        GUILayout.EndHorizontal();
        Rect rectDestroySellect = GUILayoutUtility.GetRect(10f, 20f);
        GUI.Box(new Rect(rectDestroySellect.left - 5f, rectDestroySellect.top, rectDestroySellect.width + 10f, 50f), "DestroyPoint");
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        if (GUILayout.Button("Start", null))
        {
            if (int.TryParse(DublTime, out int Duble) && PointPos != null)
            {
                timePostBuilds.Add(new TimePostBuild(GetBuild.ToArray(), Duble, PointPos.Value, true, new ReturnBuildComponent()));
            }
        }
        if (GUILayout.Button("NotTime", null))
        {
            using (var ret = new ReturnBuildComponent())
                foreach (var item in GetBuild)
                {
                    ret.SetInfoBlock((MyIntVect)item + PointPos.Value);
                    BuildAPI.RemoveBlockRPC((MyIntVect)item + PointPos.Value);
                }
        }
        GUILayout.EndHorizontal();
        isPause = GUILayout.Toggle(isPause, "isPause", null);
        if(GUILayout.Button("AllClearTimers", null))
        {
            foreach (var time in timePostBuilds)
                time.StopTimer();
            timePostBuilds.Clear();
        }
        GUI.DragWindow();
    };

    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(100, 100, 200, 150);
    public void Start()
    {
    }
    public void Update()
    {
        if (RenameHash.isActiveMenu || RenameHash.isActiveChat)
            return;
        if(!isPause)
        for (int i = 0; i < timePostBuilds.Count; i++)
        {
                if (timePostBuilds[i].Update())
                {
                    timePostBuilds[i].StopTimer();
                    timePostBuilds.RemoveAt(i);
                }
        }
        if (Input.GetKeyDown(SettingsMenu.Select))
        {
            Sellect = !Sellect;
        }
        if (Input.GetKeyDown(SettingsMenu.CopyBuild))
        {
            CopyBuild();
        }
        if (Input.GetKeyDown(SettingsMenu.PostBuild))
        {
            postCpyBuild = !postCpyBuild;
        }
        if (Input.GetKeyDown(SettingsMenu.ClearBuild))
        {
            GetBuild.Clear();
        }
        Ray ray = BuildAPI.GetRayInCamera();
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, 1 << LayerMask.NameToLayer("Terrain") | 1 << LayerMask.NameToLayer("Water")))
        {
            Vector3 vect = hit.point + ray.direction.normalized * 0.01f;
            if (postCpyBuild && Input.GetKeyDown(KeyCode.Mouse1))
            {
                vect = hit.point + hit.normal * 0.01f;
                using (var ret = new ReturnBuildComponent())
                    foreach (var block in GetBuild)
                    {
                        ret.SetInfoBlock(vect + (MyIntVect)block);
                        BuildAPI.AddBlockRPC(vect + (MyIntVect)block, (BlockType)block.BlockType, (BlockKind)block.BlockKind);
                    }
            }
            if(SelectPoint)
            {
                if(Input.GetKeyDown(KeyCode.Mouse0))
                {
                    PointPos = vect;
                }
            }
            if (Sellect)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1))
                    pointMass1 = vect;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                    pointMass2 = vect;
            }
        }
    }
    private void ViewBuild(MyIntVect intVect)
    {
        int prom = 0;
        foreach (var block in GetBuild)
        {
            prom++;
            MyIntVect vector = block;
            if ((GetBuild.Count >= 3000 && GetBuild.Count < 10000 && prom % 3 == 0) || (GetBuild.Count >= 10000 && GetBuild.Count < 15000 && prom % 10 == 0) || (GetBuild.Count < 150000 && prom % 20 == 0) || (GetBuild.Count >= 150000 && prom % 100 == 0))
            {
                BuildFoxAPI.DrawStringToDislay(intVect + vector, "●");
            }
            else if (GetBuild.Count < 3000)
                BuildFoxAPI.DrawStringToDislay(intVect + vector, "●");
        }
    }
    public void OnGUI()
    {
        if (PointPos != null)
            BuildFoxAPI.DrawStringToDislay(PointPos.Value, "$");
        if (pointMass1 != null)
            BuildFoxAPI.DrawStringToDislay(pointMass1.Value, "★");
        if (pointMass2 != null)
            BuildFoxAPI.DrawStringToDislay(pointMass2.Value, "★");
        if(b_ViewPos && PointPos != null)
        {
            ViewBuild(PointPos.Value);
        }
        if (postCpyBuild)
        {
            Ray ray = BuildAPI.GetRayInCamera();
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, 1 << LayerMask.NameToLayer("Terrain") | 1 << LayerMask.NameToLayer("Water")))
            {
                var vect = hit.point + hit.normal * 0.01f;
                ViewBuild(vect);
            }
        }
    }
}

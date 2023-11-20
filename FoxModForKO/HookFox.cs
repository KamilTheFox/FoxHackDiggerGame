using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using UnityEngine;
using PhotonNetWork = PDEEDIKOEHC;

public class HookFox : MelonMod, IHackMenu
{
    IHackMenu MenuBuilder =  new Building();
    public static HookFox Instance { get; private set; }
    public int CountMenu => Instance.menus.Count;

    public static bool IsServerGame => WorldGameObjectL.BJANBGEILJD != null;

    private static GameObject mainPlayer;

    public static GameObject MainPlayer
    { 
        get
        {
            if (mainPlayer == null)
                mainPlayer = GameObject.Find("MAIN_PLAYER");
            return mainPlayer;
        }
    }
    public static bool IsStaredGame => MainPlayer != null;

    public static event System.Action JoinedGame;

    public static event System.Action LeavedGame;

    public static event System.Action StartGame;

    List<IHackMenu> menus = new List<IHackMenu>()
    {
        new Other(),
        new BodyMod(),
        new PlayerList(),
        new Permissions(),
        new Room(),
        new Chunks(),
        new AutoKickMenu(),
        new Exploits(),
        new Entity(),
        new Skins(),
        new Tablet(),
        new SettingsMenu(),
    };
    public override void OnApplicationStart()
    {
        base.OnApplicationStart();
        Instance = this;
        JoinedGame += () => Debug.Log("Я присоединяюсь");
        JoinedGame += DeleteMatBase;
        StartGame += () => Debug.Log("Я ПРИСОЕДИНИЛСЯ");
        LeavedGame += () => Debug.Log("Я ПОКИНУЛ ИГРУ");
        foreach (var menu in menus.Append(MenuBuilder))
        {
            if(menu is IStarted started)
                try
                {
                        started.Start();
                }
                catch
                {
                    Debug.LogError($"menu Started: {menu.Name} Error");
                }
        }
    }
    private void DeleteMatBase()
    {
        App.Instance.AntiMatSystem.EJFMGDLANEI = new UnhollowerBaseLib.Il2CppStringArray(new string[0]);
        JoinedGame -= DeleteMatBase;
    }
    public string Name => "FoxHack";

    public System.Action<int> Menu => MainMenu;

    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(100, 100, 200, 100);

    public override void OnGUI()
    {
        base.OnGUI();
        GUI.color = SettingsMenu.ColorMenu;
        System.Action<int> act;
        if (MenuBuilder.IsActive)
        {
            act = (id) => { GUI.color = SettingsMenu.ColorContent; };
            act += MenuBuilder.Menu;
            MenuBuilder.Rect = GUILayout.Window(1, MenuBuilder.Rect, act, MenuBuilder.Name, new GUILayoutOption[0]);
            if(MenuBuilder is IGUIElement gui) gui.OnGUI();
        }
        if (!IsActive) return;
        act = (i) => { GUI.color = SettingsMenu.ColorContent; };
        act += Menu;
        Rect = GUILayout.Window(0, Rect, act, "FoxHack", new GUILayoutOption[0]);
        act = null;
            
        var emenus = menus.ToList();
        for (int i = 2; i <= emenus.Count + 1; i++)
        {
            IHackMenu menu = emenus[i - 2];
            if (menu.IsActive)
            {
                act = (id) => { GUI.color = SettingsMenu.ColorContent; };
                act += menu.Menu;
                menu.Rect = GUILayout.Window(i, menu.Rect, act, menu.Name, new GUILayoutOption[0]);
            }
            if (menu is IGUIElement gui) gui.OnGUI();
        }
    }
    private bool checkJoinHelp;
    private bool checkStartGame;
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!checkJoinHelp && IsServerGame)
        {
            checkJoinHelp = true;
            JoinedGame?.Invoke();
        }
        else if (checkJoinHelp && !IsServerGame)
        {
            checkJoinHelp = false;
            checkStartGame = false;
            LeavedGame?.Invoke();
        }
        if (!checkStartGame && IsStaredGame)
        {
            checkStartGame = true;
            StartGame?.Invoke();
        }

        if (Input.GetKeyDown(SettingsMenu.InputMainMenu) && !RenameHash.isActiveChat)
            IsActive = !IsActive;
        if (Input.GetKeyDown(KeyCode.Home))
            WorldGameObjectL.BJANBGEILJD.ExitGame();
        foreach (IHackMenu menu in menus.Append(MenuBuilder))
        {
            if (menu is IUpdate update)
                try
                {
                    update.Update();
                }
                catch
                {
                    Debug.LogError($"menu: {menu.Name} Error");
                }
                
        }
    }
    public void ResetMenuRect()
    {
        foreach (var menu in menus)
            menu.Rect = new Rect(menu.Rect.position, new Vector2(200, 150));
    }
    private void MainMenu(int id)
    {
        if(GUI.Button(new Rect(5,5,60,17), "Reset"))
        {
            ResetMenuRect();
            Building.Instance.ResetMenuRect();
        }
        Rect rect = GUILayoutUtility.GetRect(10, 20);
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        GUILayoutUtility.GetRect(0, 100);
        GUILayout.BeginVertical(new GUILayoutOption[0]);
        rect.height += menus.Count * 21 + 15;
        GUI.Box(rect, "List Menu");
        foreach (var menu in menus)
        {
            menu.IsActive = GUILayout.Toggle(menu.IsActive, "  Menu: " + menu.Name, null);
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayoutUtility.GetRect(10, 5);
        GUI.DragWindow();
    }
}

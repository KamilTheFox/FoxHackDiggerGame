using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SettingsMenu : IHackMenu, IStarted, IUpdate, IGUIElement
{
    public string Name => "Settings";
    public static Color ColorMenu { get; private set; } = Color.white;
    public static Color ColorContent { get; private set; } = Color.white;

    public static KeyCode Select { get; private set; } = KeyCode.E;
    public static KeyCode CopyBuild { get; private set; } = KeyCode.T;

    public static KeyCode PostBuild { get; private set; } = KeyCode.G;
    public static KeyCode ClearBuild { get; private set; } = KeyCode.V;
    public static KeyCode CreateCube { get; private set; } = KeyCode.Y;

    public static KeyCode InputBuildMenu { get; private set; } = KeyCode.Insert;

    public static KeyCode InputMainMenu { get; private set; } = KeyCode.End;

    private bool b_Select, b_CopyBuild, b_PostBuild, b_ClearBuild, b_CreateCube, b_ChangeInputBuildMenu, b_ChangeInputMainMenu; 
    public Action<int> Menu => (id) => 
    {
        GUI.color = ColorMenu;
        GUILayout.Box("ColorMenu", null);
        Color menu = ColorMenu;
        menu.r = GUILayout.HorizontalScrollbar(menu.r, 0.4f, 0, 1.4f, null);
        menu.g = GUILayout.HorizontalScrollbar(menu.g, 0.4f, 0, 1.4f, null);
        menu.b = GUILayout.HorizontalScrollbar(menu.b, 0.4f, 0, 1.4f, null);
        menu.a = GUILayout.HorizontalScrollbar(menu.a, 0.4f, 0, 1.4f, null);
        ColorMenu = menu;
        GUI.color = ColorContent;
        menu = ColorContent;
        GUILayout.Box("ColorContent", null);
        menu.r = GUILayout.HorizontalScrollbar(menu.r, 0.4f, 0, 1.4f, null);
        menu.g = GUILayout.HorizontalScrollbar(menu.g, 0.4f, 0, 1.4f, null);
        menu.b = GUILayout.HorizontalScrollbar(menu.b, 0.4f, 0, 1.4f, null);
        menu.a = 1F;
        if (b_Select)
            GUI.color = Color.red;
        if(GUILayout.Button("Select: " + Select.ToString(), null))
        {
            b_Select = !b_Select;
        }
        GUI.color = ColorContent;
        if (b_CopyBuild)
            GUI.color = Color.red;
        if (GUILayout.Button("CopyBuild: " + CopyBuild.ToString(), null))
        {
            b_CopyBuild = !b_CopyBuild;
        }
        GUI.color = ColorContent;
        if (b_PostBuild)
            GUI.color = Color.red;
        if (GUILayout.Button("PostBuild: " + PostBuild.ToString(), null))
        {
            b_PostBuild = !b_PostBuild;
        }
        GUI.color = ColorContent;
        if (b_ClearBuild)
            GUI.color = Color.red;
        if (GUILayout.Button("ClearBuild: " + ClearBuild.ToString(), null))
        {
            b_ClearBuild = !b_ClearBuild;
        }
        GUI.color = ColorContent;
        if (b_CreateCube)
            GUI.color = Color.red;
        if (GUILayout.Button("CreateCube: " + CreateCube.ToString(), null))
        {
            b_CreateCube = !b_CreateCube;
        }
        if(b_ChangeInputBuildMenu)
            GUI.color = Color.red;
        if (GUILayout.Button("BuildMenu: " + InputBuildMenu.ToString(), null))
        {
            b_ChangeInputBuildMenu = !b_ChangeInputBuildMenu;
        }
        GUI.color = ColorContent;
        if (b_ChangeInputMainMenu)
            GUI.color = Color.red;
        if (GUILayout.Button("MainMenu: " + InputMainMenu.ToString(), null))
        {
            b_ChangeInputMainMenu = !b_ChangeInputMainMenu;
            if(b_ChangeInputMainMenu)
            {
                InputMainMenu = KeyCode.None;
            }
        }
        GUI.color = ColorContent;
        if (GUILayout.Button("SetDefault", null))
        {
            PlayerPrefs.DeleteKey("ColorMenu: R");
            PlayerPrefs.DeleteKey("ColorMenu: G");
            PlayerPrefs.DeleteKey("ColorMenu: B");
            PlayerPrefs.DeleteKey("ColorMenu: A");
            PlayerPrefs.DeleteKey("ColorContent: R");
            PlayerPrefs.DeleteKey("ColorContent: G");
            PlayerPrefs.DeleteKey("ColorContent: B");
            PlayerPrefs.DeleteKey("Select");
            PlayerPrefs.DeleteKey("CopyBuild");
            PlayerPrefs.DeleteKey("PostBuild");
            PlayerPrefs.DeleteKey("ClearBuild");
            PlayerPrefs.DeleteKey("CreateCube");
            Start();
        }
        if (GUI.changed)
        {
            PlayerPrefs.SetFloat("ColorMenu: R", ColorMenu.r);
            PlayerPrefs.SetFloat("ColorMenu: G", ColorMenu.g);
            PlayerPrefs.SetFloat("ColorMenu: B", ColorMenu.b);
            PlayerPrefs.SetFloat("ColorMenu: A", ColorMenu.a);

            PlayerPrefs.SetFloat("ColorContent: R", ColorContent.r);
            PlayerPrefs.SetFloat("ColorContent: G", ColorContent.g);
            PlayerPrefs.SetFloat("ColorContent: B", ColorContent.b);
        }
        ColorContent = menu;
        GUI.DragWindow();
    };
    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(100,100,250,100);



    public void Start()
    {
        float r, g, b, a;
        r = PlayerPrefs.GetFloat("ColorMenu: R", 0.1192363F);
        g = PlayerPrefs.GetFloat("ColorMenu: G", 0.6327164F);
        b = PlayerPrefs.GetFloat("ColorMenu: B", 0.8190799F);
        a = PlayerPrefs.GetFloat("ColorMenu: A", 1F);
        ColorMenu = new Color(r, g, b, a);
        float r1, g1, b1;
        r1 = PlayerPrefs.GetFloat("ColorContent: R", 0.5445136F);
        g1 = PlayerPrefs.GetFloat("ColorContent: G", 0.9653531F);
        b1 = PlayerPrefs.GetFloat("ColorContent: B", 0.6886991F);
        ColorContent = new Color(r1, g1, b1);
        Select = (KeyCode)PlayerPrefs.GetInt("Select", (int)KeyCode.E);
        CopyBuild = (KeyCode)PlayerPrefs.GetInt("CopyBuild", (int)KeyCode.T);
        PostBuild = (KeyCode)PlayerPrefs.GetInt("PostBuild", (int)KeyCode.G);
        ClearBuild = (KeyCode)PlayerPrefs.GetInt("ClearBuild", (int)KeyCode.V);
        CreateCube = (KeyCode)PlayerPrefs.GetInt("CreateCube", (int)KeyCode.Y);
        if(PlayerPrefs.GetInt("test", 0) == 0)
        {
            PlayerPrefs.SetInt("test", 1);
            PlayerPrefs.DeleteKey("InputBuildMenu");
            PlayerPrefs.DeleteKey("InputMainMenu");
        }
        InputBuildMenu = (KeyCode)PlayerPrefs.GetInt("InputBuildMenu", (int)KeyCode.Insert);
        InputMainMenu = (KeyCode)PlayerPrefs.GetInt("InputMainMenu", (int)KeyCode.End);
    }

    public void Update()
    {
        if (b_Select || b_CopyBuild || b_PostBuild || b_ClearBuild || b_CreateCube || b_ChangeInputBuildMenu || b_ChangeInputMainMenu)
        {
            KeyCode keyCode = ((KeyCode[])Enum.GetValues(typeof(KeyCode))).Where(x => Input.GetKeyDown(x)).FirstOrDefault();
            if (Event.current.isKey)
            {
                if (b_Select)
                {
                    PlayerPrefs.SetInt("Select", (int)keyCode);
                    Select = keyCode;
                    b_Select = false;
                }
                if (b_CopyBuild)
                {
                    PlayerPrefs.SetInt("CopyBuild", (int)keyCode);
                    CopyBuild = keyCode;
                    b_CopyBuild = false;
                }
                if (b_PostBuild)
                {
                    PlayerPrefs.SetInt("PostBuild", (int)keyCode);
                    PostBuild = keyCode;
                    b_PostBuild = false;
                }
                if (b_ClearBuild)
                {
                    PlayerPrefs.SetInt("ClearBuild", (int)keyCode);
                    ClearBuild = keyCode;
                    b_ClearBuild = false;
                }
                if(b_CreateCube)
                {
                    PlayerPrefs.SetInt("CreateCube", (int)keyCode);
                    CreateCube = keyCode;
                    b_CreateCube = false;
                }
                if (b_ChangeInputBuildMenu)
                {
                    PlayerPrefs.SetInt("InputBuildMenu", (int)keyCode);
                    InputBuildMenu = keyCode;
                    b_ChangeInputBuildMenu = false;
                }
                if (b_ChangeInputMainMenu)
                {
                    PlayerPrefs.SetInt("InputMainMenu", (int)keyCode);
                    InputMainMenu = keyCode;
                    b_ChangeInputMainMenu = false;
                }
            }
        }
    }

    public void OnGUI()
    {
       
    }
}

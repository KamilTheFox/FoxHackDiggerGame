using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MenuGenerate : IHackMenu, IGUIElement, IUpdate
{
    public string Name => "Generate";

    private IGenerate generate;
    public Action<int> Menu => (id) =>
    {
        ToggleGenerate<Roof>();
        ToggleGenerate<Line>();
        ToggleGenerate<PixelArt>();
        GUILayout.Toggle(false, "BezierLine", null);
        GUILayout.Toggle(false, "Room", null);
        ToggleGenerate<Wall>();
        ToggleGenerate<Trees>();
        if (GUILayout.Button("GetBuildBuffer", null))
        {
            generate.Regenerate();
            Building.Instance.Build = generate.GetBuild.ToList();
        }
        if (GUILayout.Button("PostBuild", null))
        {
            generate.Regenerate();
            foreach(BlockInfo block in generate.GetBuild)
            {
                BuildAPI.AddBlockRPC(block);
            }
        }
        GUI.DragWindow();
    };
    private void ToggleGenerate<T>() where T : IGenerate, new()
    {
        if (GUILayout.Toggle(generate != null && generate.GetType().Name == typeof(T).Name, "Menu: " + typeof(T).Name, null))
        {
            if (generate == null || generate.GetType().Name != typeof(T).Name)
            {
                RectMenu = new Rect(RectMenu.left, RectMenu.top, 0, 0);
                generate = new T();
            }
        }
    }
    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(100,200,200,150);
    public Rect RectMenu { get; set; } = new Rect(100, 200, 200, 150);

    public void OnGUI()
    {
        if (generate == null)
            return;
        RectMenu = GUILayout.Window(99, new Rect(Rect.left + Rect.width, Rect.top, RectMenu.width, RectMenu.height), generate.Menu, "Configurations", new GUILayoutOption[0]);
        if (generate is IGUIElement iGUI)
            iGUI.OnGUI();
        

    }

    public void Update()
    {
        if (generate == null)
            return;
        if (generate is IUpdate update)
            update.Update();
    }
}


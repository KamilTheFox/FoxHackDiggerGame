using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Wall : IGenerate, IGUIElement, IUpdate
{
    public BlockInfo[] GetBuild { get; private set; }

    private List<MyIntVect> points = new List<MyIntVect>();

    private bool isSellect, isPerimeter;

    List<bool> ignoreWall = new List<bool>();

    private float Height = 1F, scroll = 0F;

    Action<int> IGenerate.Menu => (id) =>
    {
        if (GUILayout.Button("Sellect point: " + (isSellect ? "On" : "Off"), null))
            isSellect = !isSellect;
        GUILayout.Label("Height Wall: " + Height.ToString("0"), null);
        Height = GUILayout.HorizontalSlider(Height,1,40, null);
        isPerimeter = GUILayout.Toggle(isPerimeter, "isPerimeter", null);
        if(GUILayout.Button("Clear", null))
        {
            points.Clear();
            ignoreWall.Clear();
        }
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        scroll = GUILayout.VerticalSlider(scroll, 0, points.Count * 20, null);
        Rect area = GUILayoutUtility.GetRect(160, 120);
        GUI.Box(area, "");
        GUI.BeginGroup(area);
        for (int i = 0; i < points.Count; i++)
        {
            if (GUI.Button(new Rect(5, 20 * i - scroll, area.width - 70, 20), "Delete: " + i.ToString()))
            {
                points.RemoveAt(i);
                ignoreWall.RemoveAt(i);
            }
            ignoreWall[i] = GUI.Toggle(new Rect(area.width - 65, 20 * i - scroll, area.width - 10, 20), ignoreWall[i], "ignore");
        }
        GUI.EndGroup();
        GUILayout.EndHorizontal();
        GUI.DragWindow();
    };
    public void OnGUI()
    {
        if (points == null) return;
        int index = 0;
        foreach (var point in points)
        {
            BuildFoxAPI.DrawStringToDislay(point, index.ToString());
            index++;
        }
    }

    public void Regenerate()
    {
        List<BlockInfo> blocks = new List<BlockInfo>();
        for (int height = 0; height < Height; height++)
        {
            for (int i = 0; i < points.Count - 1; i++)
                if(!ignoreWall[i])
                    blocks.AddRange(Line.GetLine(points[i] + Vector3.up * height, points[i + 1] + Vector3.up * height, BuildAPI.GetBlocksInSellected().Item1));
            if(isPerimeter)
                blocks.AddRange(Line.GetLine(points[points.Count - 1] + Vector3.up * height, points[0] + Vector3.up * height, BuildAPI.GetBlocksInSellected().Item1));
        }
        GetBuild = blocks.ToArray();
    }

    public void Update()
    {
        if (RenameHash.isActiveMenu || RenameHash.isActiveChat)
            return;
        if (!isSellect) return;
        Ray ray = BuildAPI.GetRayInCamera();
        if (Input.GetKeyDown(KeyCode.Mouse1))
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, 1 << LayerMask.NameToLayer("Terrain") | 1 << LayerMask.NameToLayer("Water")))
            {
                var vect = hit.point + hit.normal * 0.01f;
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    points.Add(vect);
                    ignoreWall.Add(false);
                }
            }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SpoofMenu : IHackMenu, IUpdate
{
    public string Name => "Spoofs";

    private BlockType blockThis, blockThat;

    private bool isBlockThis, isBlockThat;
    
    public Action<int> Menu => (id) =>
    {
        GUILayout.BeginHorizontal(null);
        if(GUILayout.Button("This", null))
        {
            isBlockThis = true;
        }
        GUILayout.Label(blockThis.ToString(), null);
        if (GUILayout.Button("That", null))
        {
            isBlockThat = true;
        }
        GUILayout.Label(blockThat.ToString(), null);
        GUILayout.EndHorizontal();
        if(GUILayout.Button("SetSpoof", null))
        {
            BuildAPI.Spoofs.Add(new BuildAPI.Spoof(blockThis, blockThat));
        }
        Rect area = GUILayoutUtility.GetRect(130, 0);
        GUI.Box(new Rect(area.left, area.top, area.width,  75 * BuildAPI.Spoofs.Count), "");
        for (int i = 0; i < BuildAPI.Spoofs.Count; i++)
        {
            GUILayout.BeginHorizontal(null);
            GUILayout.Label(BuildAPI.Spoofs[i]._This.ToString(),null);
            GUILayout.Label(BuildAPI.Spoofs[i]._That.ToString(), null);
            if(GUILayout.Button("Delete", null))
            {
                BuildAPI.Spoofs.RemoveAt(i);
            }
            GUILayout.EndHorizontal();
        }
        GUI.DragWindow();
    };

    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(0,0,200,200);

    public void Update()
    {
        if (!isBlockThis && !isBlockThat)
            return;
        Ray ray = BuildAPI.GetRayInCamera(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, 1 << LayerMask.NameToLayer("Terrain") | 1 << LayerMask.NameToLayer("Water")))
        {
            if (isBlockThis)
            {
                blockThis = BuildAPI.GetBlockType(hit.point + ray.direction.normalized * 0.01f);
                if(Input.GetKeyDown(KeyCode.Mouse0))
                isBlockThis = false;
            }
            if (isBlockThat)
            {
                blockThat = BuildAPI.GetBlockType(hit.point + ray.direction.normalized * 0.01f);
                if (Input.GetKeyDown(KeyCode.Mouse0))
                    isBlockThat = false;
            }
        }
        else
        {
            if (isBlockThis)
            {
                blockThis = BlockType.Air;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                    isBlockThis = false;
            }
            if (isBlockThat)
            {
                blockThat = BlockType.Air;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                    isBlockThat = false;
            }
        }
    }
}

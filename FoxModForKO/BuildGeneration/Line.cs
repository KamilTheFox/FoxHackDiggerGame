using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Line : IGenerate, IGUIElement, IUpdate
{
    public BlockInfo[] GetBuild { get; private set; }

    private MyIntVect? point1, point2;

    private bool isSellect;

    Action<int> IGenerate.Menu => (id) =>
    {
        if (GUILayout.Button("Sellect point: " + (isSellect ? "On" : "Off"), null))
            isSellect = !isSellect;
    };

    public void Regenerate()
    {
        GetBuild = GetLine(point1.Value, point2.Value, BuildAPI.GetBlocksInSellected().Item1);
    }

    public static BlockInfo[] GetLine(MyIntVect vect1, MyIntVect vect2, BlockType type)
    {
        int distance = (int)Vector3.Distance(vect1, vect2) + 1;

        BlockInfo[] blocks = new BlockInfo[distance];

        for (int i = 0; i < blocks.Length; i++)
        {
            Vector3 vector = Vector3.Lerp(vect1.NormalizedVector3(), vect2.NormalizedVector3(), i / (float)blocks.Length);
            //GameObject game = new GameObject("Line");
            //LineRenderer line = game.AddComponent<LineRenderer>();
            //line.castShadows = false;
            //UnhollowerBaseLib.Il2CppStructArray<Vector3> arr = new UnhollowerBaseLib.Il2CppStructArray<Vector3>(2);
            //arr[0] = vect1.NormalizedVector3();
            //arr[1] = vect2.NormalizedVector3();
            //line.SetPositions(arr);
            blocks[i] = new BlockInfo(vector) { BlockType = (int)type, BlockKind = 0 };
        }
        return blocks;
    }

    public void OnGUI()
    {
        if(point1 != null)
            BuildFoxAPI.DrawStringToDislay(point1.Value, "Line1");
        if (point2 != null)
            BuildFoxAPI.DrawStringToDislay(point2.Value, "Line2");
    }

    public void Update()
    {
        if (RenameHash.isActiveMenu || RenameHash.isActiveChat)
            return;
        if (!isSellect) return;
        Ray ray = BuildAPI.GetRayInCamera();
        if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, 1 << LayerMask.NameToLayer("Terrain") | 1 << LayerMask.NameToLayer("Water")))
        {
            var vect = hit.point + hit.normal * 0.01f;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                    point1 = vect;
                if (Input.GetKeyDown(KeyCode.Mouse1))
                    point2 = vect;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Roof : IGenerate
{
    //MyIntVect _pointCreater1, _pointCreater2;
    private MyIntVect Size;
    private bool RotateRoof;
    //public Roof(MyIntVect point1, MyIntVect point2, bool isDiagonal, bool isHigh)
    //{
    //    int x = point2.x - point1.x,
    //     y = point2.y - point1.y, z = 0;
    //    if (x > y)
    //    {
    //        BuildFoxAPI.Swich(ref x, ref y);
    //    }
    //    Size = new MyIntVect(x, y, x + y);
    //    BlockKind = isDiagonal ? BlockKind.DiagonalEast : BlockKind.StairEast;
    //    this.isHigh = isHigh;
    //}

    public BlockInfo[] GetBuild { get; private set; }

    Action<int> IGenerate.Menu => (id) =>
    {
        GUILayout.Label(Size.x.ToString("0.0"), null);
        Size.x = (int)GUILayout.HorizontalSlider(Size.x, 4, 40, null);
        GUILayout.Label(Size.y.ToString("0.0"), null);
        Size.y = (int)GUILayout.HorizontalSlider(Size.y, 4, 40, null);
        RotateRoof = GUILayout.Toggle(RotateRoof, "Rotate", null);
        if(GUILayout.Button("GetBuild", null))
        {
            Regenerate();
            Building.Instance.Build = GetBuild.ToList();
        }
    };

    public void Regenerate()
    {
        List<BlockInfo> blocks = new List<BlockInfo>();
        int z = -1;
        for (int x = -1; x < Size.x + 2; x++)
            for (int y = -1; y < Size.y + 2; y++)
            {
                int index = x;
                int indexMax = Size.x + 1;
                if (RotateRoof)
                {
                    index = y;
                    indexMax = Size.y + 1;
                }
                z = index > (indexMax + 1) / 2 ? indexMax - index : index;
                blocks.Add(new BlockInfo() { x = x, y = y, z = z, BlockType = 9, BlockKind = 0 });


            }
        GetBuild = blocks.ToArray();
    }
}

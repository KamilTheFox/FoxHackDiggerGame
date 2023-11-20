using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Chunks : IHackMenu
{
    public string Name => "Chunks";

    private MyIntVect SizeChunk, CountBlockChunk;

    public Action<int> Menu => (id) =>
    {
        GUILayout.Label("SizeChunk", null);
        SizeChunk.x = int.Parse(GUILayout.TextField(SizeChunk.x.ToString(),null));
        SizeChunk.y = int.Parse(GUILayout.TextField(SizeChunk.y.ToString(), null));
        SizeChunk.z = int.Parse(GUILayout.TextField(SizeChunk.z.ToString(), null));
        GUILayout.Label("CountBlockChunk", null);
        CountBlockChunk.x = int.Parse(GUILayout.TextField(CountBlockChunk.x.ToString(), null));
        CountBlockChunk.y = int.Parse(GUILayout.TextField(CountBlockChunk.y.ToString(), null));
        CountBlockChunk.z = int.Parse(GUILayout.TextField(CountBlockChunk.z.ToString(), null));
        if(GUILayout.Button("Create", null))
        {
            WorldGameObjectL.BJANBGEILJD.PBKFBFAGLOL(SizeChunk.x, SizeChunk.y, SizeChunk.z, CountBlockChunk.x, CountBlockChunk.y, CountBlockChunk.z);
        }
        if(GUILayout.Button("PostBlockMax", null))
        {
            MyIntVect maxMap = BuildAPI.GetMaxMap();
            BuildAPI.AddBlockRPC(maxMap, (BlockType)150, BlockKind.Default);
            BuildAPI.AddBlockRPC(MyIntVect.zero, (BlockType)150, BlockKind.Default);

            BuildAPI.AddBlockRPC(new MyIntVect(maxMap.x,0,0), (BlockType)150, BlockKind.Default);
            BuildAPI.AddBlockRPC(new MyIntVect(maxMap.x, 0, maxMap.z), (BlockType)150, BlockKind.Default);
            BuildAPI.AddBlockRPC(new MyIntVect(0, 0 , maxMap.z), (BlockType)150, BlockKind.Default);

            BuildAPI.AddBlockRPC(new MyIntVect(0, maxMap.y, 0), (BlockType)150, BlockKind.Default);
            BuildAPI.AddBlockRPC(new MyIntVect(0, maxMap.y, maxMap.z), (BlockType)150, BlockKind.Default);
            BuildAPI.AddBlockRPC(new MyIntVect(maxMap.x, 0, maxMap.z), (BlockType)150, BlockKind.Default);
        }
        GUILayout.Label("In Development", null);
        GUI.DragWindow();
    };

    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(200, 200, 200, 200);
}

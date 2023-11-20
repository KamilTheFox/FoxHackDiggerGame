using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ReturnBuildComponent : IDisposable
{
    private static int Index;
    public static int GetIndex => Index;
    public ReturnBuildComponent()
    {
        Index = PlayerPrefs.GetInt("Index", 0);
    }
    private List<BlockInfo> blockInfos = new List<BlockInfo>();

    public static BlockInfo[] GetReturn()
    {
        if(Index== 0)
            return new BlockInfo[0];
        var blocks = SavingConfig.Open<List<BlockInfo>>(SavingConfig.DirectoryType.ReturnBuild, $"Return {Index}");
        Index--;
        PlayerPrefs.SetInt("Index", Index);
        return blocks.ToArray();
    }

    public static void ClearBuffer()
    {
        Index = 0;
        PlayerPrefs.SetInt("Index", Index);
        SavingConfig.ClearAllFiles(SavingConfig.DirectoryType.ReturnBuild);
    }
    public void SetInfoBlock(MyIntVect myInt)
    {
          blockInfos.Add(new BlockInfo(myInt) { BlockType = (int)BuildAPI.GetBlockType(myInt), BlockKind = (int)BuildAPI.GetBlockKind(myInt) });
    }
    public void Dispose()
    {
        Index++;
        PlayerPrefs.SetInt("Index", Index);
        SavingConfig.Save(SavingConfig.DirectoryType.ReturnBuild, $"Return {Index}", blockInfos); SavingConfig.Save(SavingConfig.DirectoryType.ReturnBuild, $"Return {Index}", blockInfos);
    }
}

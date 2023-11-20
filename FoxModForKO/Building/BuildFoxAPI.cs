using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class BuildFoxAPI
{
	public static BlockInfo[] CopyBuild(MyIntVect point1, MyIntVect point2, bool reversCopy = false, bool isSpoofBlock = false)
	{
		bool isThereAir = BuildAPI.Spoofs.Where(block => block._This == BlockType.Air).Count() > 0;
		List<BlockInfo> blockList = new List<BlockInfo>();
		SwichPoint(ref point1, ref point2);
		for (int z = reversCopy ? point2.z : point1.z; reversCopy ? z >= point1.z : z <= point2.z;)
		{
			for (int x = point1.x; x <= point2.x; x++)
				for (int y = point1.y; y <= point2.y; y++)
				{
					MyIntVect intVect = new MyIntVect(x, y, z);
					BlockType blockType = BuildAPI.GetBlockType(intVect);
					if (!isThereAir && blockType == BlockType.Air)
						continue;

					if (isSpoofBlock && BuildAPI.Spoofs.Where(spoof => spoof._This == blockType).Count() == 0)
						continue;

					BlockInfo block = new BlockInfo();
					block.BlockType = (int)BuildAPI.GetBlockType(intVect);
					block.BlockKind = (int)BuildAPI.GetBlockKind(intVect);
					block += intVect - point1;
					blockList.Add(block);
				}
			if (reversCopy)
				z--;
			else
				z++;
		}
		return blockList.ToArray();
	}
	private static KamilBuildMod buildMod = new KamilBuildMod();
	public static void DrawStringToDislay(MyIntVect pos, string text)
	{
		DrawStringToDislay((Vector3)pos + Vector3.one * 0.5F, text);
	}
	public static void DrawStringToDislay(Vector3 pos, string text)
	{
		Vector3 value = RenameHash.GetCameraController.WorldToScreenPoint(pos);
		if ((double)value.z > 0.0)
		{
			Vector2 vector2 = GUIUtility.ScreenToGUIPoint(value);
			vector2.y = (float)Screen.height - (vector2.y + 1f);
			GUI.Label(new Rect(vector2.x, vector2.y, 30f, 30f), text);
		}
	}
	public static void SwichPoint(ref MyIntVect point1, ref MyIntVect point2)
	{
		if (point1.x > point2.x)
		{
			Swich(ref point1.x, ref point2.x);
		}
		if (point1.y > point2.y)
		{
			Swich(ref point1.y, ref point2.y);
		}
		if (point1.z > point2.z)
		{
			Swich(ref point1.z, ref point2.z);
		}
	}
	private static BlockInfo CheckBlockForFilling(in BlockInfo StartBuild, in BlockType Cheking, MyIntVect checkVect)
	{
		BlockInfo newBlock;
		if (Cheking == BuildAPI.GetBlockType(checkVect))
		{
			newBlock = new BlockInfo();
			newBlock += checkVect;
			if (Cheking != BlockType.Air)
			{
				newBlock.BlockType = 0;
				newBlock.BlockKind = 0;
			}
			else
			{
				newBlock.BlockType = StartBuild.BlockType;
				newBlock.BlockKind = StartBuild.BlockKind;
			}
			return newBlock;
		}
		return null;
	}
	private static int? FillLimit;
	public static int? SetLimitedForFilling 
	{
		set
        {
			if(value != null)
            {
				FillLimit = value.Value * 100;
				return;
			}				
			FillLimit = value;
		}
	}
	public static void Filling(in BlockInfo startBlock, MyIntVect? normal = null , bool isDestroy = false)
    {
		Stack<BlockInfo> blocks = new Stack<BlockInfo>();
		blocks.Push(startBlock);
		int i = 0;
		using (var blocksReturn = new ReturnBuildComponent())
			while (blocks.Count != 0)
			{
				if (FillLimit != null && i >= FillLimit.Value)
					break;
				BlockInfo stackBlock = blocks.Pop();
				i++;
				MyIntVect intVect = stackBlock;
				MyIntVect MaxMap = BuildAPI.GetMaxMap();
				if (MaxMap.x < intVect.x || MaxMap.y < intVect.y || MaxMap.z < intVect.z)
					return;
				if (0 > intVect.x || 0 > intVect.y || 0 > intVect.z)
					return;
				if ((int)BuildAPI.GetBlockType(stackBlock) != stackBlock.BlockType)
				{
					blocksReturn.SetInfoBlock(stackBlock);
					BuildAPI.AddBlockRPC(stackBlock, (BlockType)stackBlock.BlockType, (BlockKind)stackBlock.BlockKind);
				}
				MyIntVect newIntVect;
				BlockInfo newBlock;
				if (normal == null || isDestroy)
				{
					newIntVect = new MyIntVect(intVect.x, intVect.y, intVect.z + 1);
					if ((newBlock = CheckBlockForFilling(startBlock, isDestroy ? (BlockType)startBlock.BlockType : 0, newIntVect)) != null)
					{
						blocks.Push(newBlock);
					}
					newIntVect = new MyIntVect(intVect.x, intVect.y, intVect.z - 1);
					if ((newBlock = CheckBlockForFilling(startBlock, isDestroy ? (BlockType)startBlock.BlockType : 0, newIntVect)) != null)
					{
						blocks.Push(newBlock);
					}
					newIntVect = new MyIntVect(intVect.x, intVect.y + 1, intVect.z);
					if ((newBlock = CheckBlockForFilling(startBlock, isDestroy ? (BlockType)startBlock.BlockType : 0, newIntVect)) != null)
					{
						blocks.Push(newBlock);
					}
					newIntVect = new MyIntVect(intVect.x, intVect.y - 1, intVect.z);
					if ((newBlock = CheckBlockForFilling(startBlock, isDestroy ? (BlockType)startBlock.BlockType : 0, newIntVect)) != null)
					{
						blocks.Push(newBlock);
					}
					newIntVect = new MyIntVect(intVect.x - 1, intVect.y, intVect.z);
					if ((newBlock = CheckBlockForFilling(startBlock, isDestroy ? (BlockType)startBlock.BlockType : 0, newIntVect)) != null)
					{
						blocks.Push(newBlock);
					}
					newIntVect = new MyIntVect(intVect.x + 1, intVect.y, intVect.z);
					if ((newBlock = CheckBlockForFilling(startBlock, isDestroy ? (BlockType)startBlock.BlockType : 0, newIntVect)) != null)
					{
						blocks.Push(newBlock);
					}

				}
				else
				{

					if (Math.Abs(normal.Value.y) > 0)
					{
						newIntVect = new MyIntVect(intVect.x - 1, intVect.y, intVect.z);
						if ((newBlock = CheckBlockForFilling(startBlock, 0, newIntVect)) != null)
						{
							blocks.Push(newBlock);
						}
						newIntVect = new MyIntVect(intVect.x + 1, intVect.y, intVect.z);
						if ((newBlock = CheckBlockForFilling(startBlock, 0, newIntVect)) != null)
						{
							blocks.Push(newBlock);
						}
						newIntVect = new MyIntVect(intVect.x, intVect.y, intVect.z - 1);
						if ((newBlock = CheckBlockForFilling(startBlock, 0, newIntVect)) != null)
						{
							blocks.Push(newBlock);
						}
						newIntVect = new MyIntVect(intVect.x, intVect.y, intVect.z + 1);
						if ((newBlock = CheckBlockForFilling(startBlock, 0, newIntVect)) != null)
						{
							blocks.Push(newBlock);
						}
					}
					if (Math.Abs(normal.Value.x) > 0)
					{
						newIntVect = new MyIntVect(intVect.x, intVect.y, intVect.z + 1);
						if ((newBlock = CheckBlockForFilling(startBlock, 0, newIntVect)) != null)
						{
							blocks.Push(newBlock);
						}
						newIntVect = new MyIntVect(intVect.x, intVect.y, intVect.z - 1);
						if ((newBlock = CheckBlockForFilling(startBlock, 0, newIntVect)) != null)
						{
							blocks.Push(newBlock);
						}
						newIntVect = new MyIntVect(intVect.x, intVect.y + 1, intVect.z);
						if ((newBlock = CheckBlockForFilling(startBlock, 0, newIntVect)) != null)
						{
							blocks.Push(newBlock);
						}
						newIntVect = new MyIntVect(intVect.x, intVect.y - 1, intVect.z);
						if ((newBlock = CheckBlockForFilling(startBlock, 0, newIntVect)) != null)
						{
							blocks.Push(newBlock);
						}
					}
					if (Math.Abs(normal.Value.z) > 0)
					{
						newIntVect = new MyIntVect(intVect.x - 1, intVect.y, intVect.z);
						if ((newBlock = CheckBlockForFilling(startBlock, 0, newIntVect)) != null)
						{
							blocks.Push(newBlock);
						}
						newIntVect = new MyIntVect(intVect.x + 1, intVect.y, intVect.z);
						if ((newBlock = CheckBlockForFilling(startBlock, 0, newIntVect)) != null)
						{
							blocks.Push(newBlock);
						}
						newIntVect = new MyIntVect(intVect.x, intVect.y - 1, intVect.z);
						if ((newBlock = CheckBlockForFilling(startBlock, 0, newIntVect)) != null)
						{
							blocks.Push(newBlock);
						}
						newIntVect = new MyIntVect(intVect.x, intVect.y + 1, intVect.z);
						if ((newBlock = CheckBlockForFilling(startBlock, 0, newIntVect)) != null)
						{
							blocks.Push(newBlock);
						}
					}
				}
			}
    }
	private static Vector3 RotateVector(Vector3 VectorBlock, Vector3 VecrorCur, bool x, bool y, bool z, bool w90, bool Q = false, bool W = false)
	{
		return buildMod.OALBDCEAGPC(VectorBlock, x,y,z,w90,Q,W);
	}
	public static void RotateBuild(BlockInfo[] blocks, bool x, bool y, bool z, bool w90, bool Q = false, bool W = false)
    {
		foreach (var block in blocks)
        {
			BlockInfo block1 = block;
			block1 += RotateVector((MyIntVect)block,Vector3.zero,x,y,z,w90, Q, W);
			block1.BlockKind = (int)RotateKind((BlockKind)block.BlockKind, x, y, z, w90, Q, W);
		}
    }
	private static BlockKind RotateKind(BlockKind K, bool x, bool y, bool z, bool w90, bool Q = false, bool W = false)
	{
		return buildMod.ILKMCJAIFOM(K, x, y, z, w90, Q, W);
	}

	public static BlockKind DoFlip(this BlockKind kind)
	{
		if (kind >= BlockKind.Flip)
		{
			return (BlockKind)(kind - BlockKind.Flip);
		}
		return kind + 128;
	}
	public static BlockKind DoRevertX(this BlockKind kind)
	{
		return RotateKind(kind, true, false, false, false);
	}
	public static BlockKind DoRevertY(this BlockKind kind)
	{
		return RotateKind(kind, false, true, false, false);
	}

	public static MyIntVect GetBuildAxis(MyIntVect vector, MyIntVect Normal)
    {
        if (Normal.x == 1)
        {
            vector = new MyIntVect(vector.z, vector.x, vector.y);
        }
        else if (Normal.x == -1)
        {
            vector = new MyIntVect(-vector.z, -vector.x, -vector.y);
        }
        if (Normal.y == 1)
        {
            vector = new MyIntVect(vector.y, vector.z, vector.x);
        }
        else if (Normal.y == -1)
        {
            vector = new MyIntVect(-vector.y, -vector.z, -vector.x);
        }
        else if (Normal.z == -1)
        {
            vector = new MyIntVect(-vector.x, -vector.y, -vector.z);
        }
        return vector;
    }
    public static void Swich<T>(ref T value1,ref T value2)
    {
        var valueSwich = value1;
        value1 = value2;
        value2 = valueSwich;
    }
    public static MyIntVect[] ArrayCube(MyIntVect vect)
    {
        List<MyIntVect> cubeList = new List<MyIntVect>();
        for (int x = 0; x < vect.x; x++)
            for (int y = 0; y < vect.y; y++)
                for (int z = 0; z < vect.z; z++)
                    {
                        cubeList.Add(new MyIntVect(x, y, z));
                    }
        return cubeList.ToArray();
    }
	public static Vector3 NormalizedVector3(this MyIntVect myInt)
	{
		return (Vector3)myInt + Vector3.one * 0.5F;
	}
}

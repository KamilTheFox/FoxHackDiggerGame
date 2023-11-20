using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class BlockInfo
{
    public int BlockType;
    public int BlockKind;
    public int x;
    public int y;
    public int z;
    public BlockInfo(int type, int kind, int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;
        BlockType = type;
        BlockKind = kind;
    }
    public BlockInfo()
    {
        x = 0;
        y = 0;
        z = 0;
        BlockType = 0;
        BlockKind = 0;
    }
    public BlockInfo(MyIntVect vect)
    {
        x = vect.x;
        y = vect.y;
        z = vect.z;
        BlockType = 0;
        BlockKind = 0;
    }
    public static BlockInfo operator +(BlockInfo block, MyIntVect vector)
    {
        block.x = vector.x;
        block.y = vector.y;
        block.z = vector.z;
        return block;
    }
    public static implicit operator MyIntVect(BlockInfo vector)
    {
        return new MyIntVect(vector.x, vector.y, vector.z);
    }
    public BlockInfo Copy()
    {
        return new BlockInfo(BlockType,BlockKind,x,y,z);
    }
    public override string ToString()
    {
        return $"Type {(BlockType)BlockType} Type {(BlockKind)BlockKind}";
    }
}

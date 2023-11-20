using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IntVect = AECPBBDPBHB;

public struct MyIntVect
{
    public static MyIntVect zero = new MyIntVect(0,0,0);
    public MyIntVect(int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }
    public int x;
    public int y;
    public int z;
    public static implicit operator MyIntVect(IntVect vect)
    {
        return new MyIntVect(vect.OAIFOGMNCCN, vect.CPILBPDMBFK, vect.GLGCLMDCHJN);
    }
    public static implicit operator IntVect(MyIntVect vect)
    {
        return new IntVect(vect.x, vect.y, vect.z);
    }

    public static implicit operator MyIntVect(Vector3 vect)
    {
        return new MyIntVect((int)(vect.x), (int)(vect.z), (int)(vect.y));
    }
    public static implicit operator Vector3(MyIntVect vect)
    {
        return new Vector3(vect.x, vect.z, vect.y);
    }
    public static bool operator ==(MyIntVect vect1, MyIntVect vect2)
    {
        return vect1.x == vect2.x && vect1.y == vect2.y && vect1.z == vect2.z;
    }
    public static bool operator !=(MyIntVect vect1, MyIntVect vect2)
    {
        return vect1.x != vect2.x && vect1.y != vect2.y && vect1.z != vect2.z;
    }
    public static MyIntVect operator *(MyIntVect vect1, int value)
    {
        return new MyIntVect(vect1.x * value, vect1.y * value, vect1.z * value);
    }
    public static MyIntVect operator /(MyIntVect vect1, int value)
    {
        if (value == 0)
            return zero;
        return new MyIntVect(vect1.x / value, vect1.y / value, vect1.z / value);
    }
    public static MyIntVect operator +(MyIntVect my , Vector3 vect)
    {
        MyIntVect myInt = vect;
        return new MyIntVect(my.x + myInt.x, my.y + myInt.y, my.z + myInt.z);
    }
    public static MyIntVect operator -(MyIntVect my, Vector3 vect)
    {
        MyIntVect myInt = vect;
        return new MyIntVect(my.x - myInt.x, my.y - myInt.y, my.z - myInt.z);
    }
    public override string ToString()
    {
        return $"x: {x}, y: {y}, z: {z}";
    }
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using EntityType = BEDOMOKJJMN;

static class BuildAPI
{
    public struct Spoof
    {
        public BlockType _This;
        public BlockType _That;
        public Spoof(BlockType _this, BlockType _that)
        {
            _This = _this;
            _That = _that;
        }
    }
    public static List<Spoof> Spoofs = new List<Spoof>();

    public static void AddBlockRPC(BlockInfo block)
    {
        AddBlockRPC(block, (BlockType)block.BlockType, (BlockKind)block.BlockKind);
    }
    public static void AddBlockRPC(MyIntVect vect, BlockType type, BlockKind kind)
    {
        MyIntVect MaxMap = GetMaxMap();
        if (MaxMap.x < vect.x || MaxMap.y < vect.y || MaxMap.z < vect.z)
            return;
        if (0 > vect.x || 0 > vect.y || 0 > vect.z)
            return;

        foreach (Spoof spoof in Spoofs)
        {
            if (spoof._This == type)
            {
                type = spoof._That;
                continue;
            }
        }

        if (GetBlockType(vect) == type && GetBlockKind(vect) == kind)
            return;

        

        Il2CppSystem.Int32 typeBlock = new Il2CppSystem.Int32();
        typeBlock.m_value = (int)type;
        Il2CppSystem.Int32 Kind = new Il2CppSystem.Int32();
        Kind.m_value = (int)kind;
        Il2CppSystem.Int32 x = new Il2CppSystem.Int32();
        Il2CppSystem.Int32 y = new Il2CppSystem.Int32();
        Il2CppSystem.Int32 z = new Il2CppSystem.Int32();

        x.m_value = vect.x;
        y.m_value = vect.y;
        z.m_value = vect.z;
        if (Kind.m_value == 15 || Kind.m_value == 23 || Kind.m_value == 24)
        {
            Kind.m_value = 0;
        }
        if (isCrashBlock(type))
        {
            if (type == BlockType.Air)
            {
                RemoveBlockRPC(vect);
                return;
            }
            AddBlockNoRPC(vect, type, kind);
            return;
        }
        WorldGameObjectL.BJANBGEILJD.photonView.RPC("AddBlockAt", PhotonTargets.All, new Il2CppSystem.Object[]
        {
                typeBlock.BoxIl2CppObject(),
                Kind.BoxIl2CppObject(),
                x.BoxIl2CppObject(), y.BoxIl2CppObject(), z.BoxIl2CppObject(),
        });
    }

    public static EnityParametrs EnityParametrs(EntityType type) => WorldGameObjectL.BJANBGEILJD.GGJPNGDMJJL[(int)type];
    public static void AddEntityRPC(EntityType entity, Vector3 vect, Quaternion quaternion)
    {
        var en = new Il2CppSystem.Int32();
        en.m_value = ((int)entity);
        Il2CppSystem.Object @object = null;
        WorldGameObjectL.BJANBGEILJD.photonView.RPC("AddEntityNetwork", PhotonTargets.All, new Il2CppSystem.Object[]
            {
                    en.BoxIl2CppObject(),
                    vect.BoxIl2CppObject(),
                    quaternion.BoxIl2CppObject(),
                    @object,@object,@object

            });
    }
    public static void DestroyEntityRPC(EntityBase entityBase)
    {
        var en = new Il2CppSystem.Int32();
        en.m_value = entityBase.photonView.viewIdField;
        WorldGameObjectL.BJANBGEILJD.photonView.RPC("DeleteEntityNetworkMasterClient", PhotonTargets.All, new Il2CppSystem.Object[]
            {
                    en.BoxIl2CppObject()
            });
    }
    public static void MoveEntityRPC(int ID, Vector3 vect, Quaternion quaternion)
    {
        var en = new Il2CppSystem.Int32();
        en.m_value = ID;
        WorldGameObjectL.BJANBGEILJD.photonView.RPC("MoveEntityNetwork", PhotonTargets.All, new Il2CppSystem.Object[]
            {
                    en.BoxIl2CppObject(),
                    vect.BoxIl2CppObject(),
                    quaternion.BoxIl2CppObject(),

            });
    }
    public static void AddBlockNoRPC(MyIntVect vect, BlockType type, BlockKind kind)
    {
        WorldGameObjectL.BJANBGEILJD.LBJEMKPILPE.OEILDDMGOKF(vect, type, kind);
    }
    public static BlockType GetBlockType(MyIntVect vect)
    {
        return WorldGameObjectL.BJANBGEILJD.PHNNAMINPMK.LFHJMFNHCOF(vect.x, vect.y, vect.z);
    }

    public static MyIntVect GetMaxMap()
    {
        int Z = WorldGameObjectL.BJANBGEILJD.PHNNAMINPMK.PFOGGBKDEFG - 1;
        int X = WorldGameObjectL.BJANBGEILJD.PHNNAMINPMK.EINFGALHEHH - 1;
        int Y = WorldGameObjectL.BJANBGEILJD.PHNNAMINPMK.HNFADHFODCH - 1;
        return new MyIntVect(X,Y,Z);
    }
    private static bool isCrashBlock(BlockType T)
    {
        if (T == BlockType.Air || T == BlockType.Iron || T == BlockType.Gold || BlockType.Brick5 == T ||
            BlockType.Stone9 == T || BlockType.Stone14_3 == T || BlockType.Stone15_3 == T || BlockType.Stone16_1 == T ||
            BlockType.HideWhenStep == T || BlockType.Light == T || BlockType.Gold == T || BlockType.Stone19_1 == T ||
            BlockType.Stone19 == T || BlockType.Stone19_2 == T || BlockType.Iron == T || BlockType.Stone4_1 == T || BlockType.Stone4_2 == T ||
            BlockType.Coal == T)
            return true;
        return false;
    }
    public static BlockKind GetBlockKind(MyIntVect vect)
    {
        return WorldGameObjectL.BJANBGEILJD.PHNNAMINPMK.OCMOHECLLBP(vect.x, vect.y, vect.z);
    }
    public static Ray GetRayInCamera()
    {
        if (!CameraController.IILOHLGKKEG)
        {
            return new Ray(Vector3.zero, Vector3.up);
        }
        return CameraController.IILOHLGKKEG.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
    }
    public static Ray GetRayInCamera(Vector2 vector2)
    {
        if(!CameraController.IILOHLGKKEG)
        {
            return new Ray(Vector3.zero, Vector3.up);
        }
        return CameraController.IILOHLGKKEG.ScreenPointToRay(vector2);
    }
    public static (BlockType, BlockKind) GetBlocksInSellected()
    {
        return (WorldGameObjectL.BJANBGEILJD.KLCNHHPDEKC, WorldGameObjectL.BJANBGEILJD.NPAMNIOIHGN);
    }
    public static void RemoveBlockRPC(MyIntVect vect)
    {
        if (GetBlockType(vect) == BlockType.Air)
            return;
        MyIntVect MaxMap = GetMaxMap();
        if (MaxMap.x < vect.x || MaxMap.y < vect.y || MaxMap.z < vect.z)
            return;
        if (0 > vect.x || 0 > vect.y || 0 > vect.z)
            return;
        Il2CppSystem.Int32 x = new Il2CppSystem.Int32();
        Il2CppSystem.Int32 y = new Il2CppSystem.Int32();
        Il2CppSystem.Int32 z = new Il2CppSystem.Int32();
        x.m_value = vect.x;
        y.m_value = vect.y;
        z.m_value = vect.z;
        Il2CppSystem.Boolean boolean = new Il2CppSystem.Boolean();
        boolean.m_value = false;
        WorldGameObjectL.BJANBGEILJD.photonView.RPC("RemoveBlockAt", PhotonTargets.All, new Il2CppSystem.Object[]
        {
            x.BoxIl2CppObject(), y.BoxIl2CppObject(), z.BoxIl2CppObject(), boolean.BoxIl2CppObject(), boolean.BoxIl2CppObject()
        });
    }
}

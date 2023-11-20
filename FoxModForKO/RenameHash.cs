using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using World = LBJEMKPILPE;
using IntVect = AECPBBDPBHB;
using PhotonPlayer = PJIKCJLCHHE;
using PhotonNetWork = PDEEDIKOEHC;
// allProperties = LPGPPHOLGBB;


public static class RenameHash
{
    public static bool isActiveMenu =>
        MainMenu.BJANBGEILJD.GILGNFMDCMB;
    public static bool isActiveChat => Chat.DDHIHJPBODG();

    public static string MyName => PhotonNetWork.FIMKAPAEODI.CGJMHEDPODP;

    public static HEDCHENEMBL GetRoom => PhotonNetWork.JBPODKLJEDB;

    public static string GetName(this PhotonPlayer player)
    {
        return player.CGJMHEDPODP;
    }
    
    public static bool IsMasterClient(this PhotonPlayer player)
    {
        return player.DHDJAMLAPLF;
    }
    public static class ChatF
    {
        public static void Info(string text, bool isAll)
        {
            Chat.DLGBILNCFJO(text, isAll);
        }
        public static void SetTextLocal(string text, PhotonPlayer photon)
        {
            Warning(" [ffffff]LocalMessage to " + photon.CGJMHEDPODP, false);
            Il2CppSystem.String textil2cpp = " [ffffff]LocalMessage: " + text;
            UnityEngine.Object.FindObjectOfType<Chat>().photonView.RPC("Warning", photon, new Il2CppSystem.Object[] { textil2cpp });
        }
        public static void Text(string text, bool isAll)
        {
            Chat.EIBNELMJCFG(text, isAll);
        }
        public static void Warning(string text, bool isAll)
        {
            Chat.JEDCHDAIKHE(text, isAll);
        }
    }
    public static class LevelF
    {
        private static List<string>[] Permissions = new List<string>[]
                {
                    new List<string>(),
                    new List<string>(),
                    new List<string>(),
                    new List<string>(),
                };

        public static bool isMySet;
        private static Dictionary<string, AMBB> Players = new Dictionary<string, AMBB>();
        public static bool IsAdminAndModer(string name)
        {
            return Level.BJANBGEILJD.ABGKDPOCFFH(name);
        }
        public static bool IsAdmin(string name)
        {
            return Level.BJANBGEILJD.JFCKAOKFCKI(name);
        }
        public static bool IsBanned(string name)
        {
            return Level.BJANBGEILJD.LNOHIGENEMB(name);
        }
        public static bool IsBuilder(string name)
        {
            return Level.BJANBGEILJD.FLJDOFCCEDA(name);
        }
        public static bool IsModer(string name)
        {
            return Level.BJANBGEILJD.ACDNHDLJBPM(name);
        }

        private static void UpdatePlayer()
        {
            Dictionary<string, AMBB> players = new Dictionary<string, AMBB>();
            for (int i = 0; i < Permissions.Length; i++)
                foreach (var playerName in Permissions[i])
                    if (!players.ContainsKey(playerName))
                        players.Add(playerName, GetAMBB(playerName));
            if(isMySet && !players.ContainsKey(MyName))
            {
                players.Add(MyName, GetAMBB(MyName));
            }
            Players = players;
        }
        public static void ClearAll()
        {
            AMBB mBB = Players[RenameHash.MyName];
            Players.Clear();
                if(isMySet)
            Players.Add(RenameHash.MyName, mBB);
        }
        private static AMBB GetAMBB(string PlayerName)
        {
            AMBB mBB;
            if (Players.TryGetValue(PlayerName, out var aMBB))
            {
                mBB = aMBB;
            }
            else
                mBB = new AMBB();
            if (!isMySet || isMySet && RenameHash.MyName != PlayerName)
            {
                mBB.isAdmin = Permissions[0].Contains(PlayerName);
                mBB.isModer = Permissions[1].Contains(PlayerName);
                mBB.isBuilder = Permissions[2].Contains(PlayerName);
                mBB.isBanned = Permissions[3].Contains(PlayerName);
            }
            return mBB;
        }
        public static void GetActualData()
        {
            if(Level.BJANBGEILJD == null)
            {
                Debug.LogError("Level.BJANBGEILJD Null References");
                return;
            }

            Level.BJANBGEILJD.photonView.RPC("GetActualData", PhotonTargets.MasterClient, null);
            Permissions = new List<string>[]
            {
               Level.BJANBGEILJD.ABDLLLOJJAM.RevertListIL2cpp(),
               Level.BJANBGEILJD.DBGPCFBEOFO.RevertListIL2cpp(),
               Level.BJANBGEILJD.MKLPLAFMBIK.RevertListIL2cpp(),
               Level.BJANBGEILJD.OACMFLFEBMM.RevertListIL2cpp(),
            };
            UpdatePlayer();
        }
        public static void AddPlayerOffline(string name, AMBB aMBB)
        {
            if (Players.ContainsKey(name))
                return;
            Players.Add(name, aMBB);
        }
        public static Dictionary<string, AMBB> GetOnlinePlayer()
        {
            Dictionary<string, AMBB> valuePairs = new Dictionary<string, AMBB>();
            foreach(var player in PhotonNetWork.JDLAABPAIEA)
            {
                AMBB aMBB;
                if (Players.TryGetValue(player.GetName(), out AMBB aMBB1))
                    aMBB = aMBB1;
                else
                {
                    aMBB = new AMBB();
                    Players.Add(player.GetName(), aMBB);
                }
                valuePairs.Add(player.GetName(), aMBB);
            }
            return valuePairs;
        }
        public static Dictionary<string, AMBB> GetOfflinePlayer()
        {
            Dictionary<string, AMBB> valuePairs = new Dictionary<string, AMBB>();
            foreach (var player in Players)
            {
                if(PhotonNetWork.JDLAABPAIEA.Where(pl => pl.GetName() == player.Key).Count() == 0)
                    valuePairs.Add(player.Key, player.Value);
            }
            return valuePairs;
        }
        public static void SetActualData()
        {
            var permissions = new List<string>[]
            {
               new List<string>(),
               new List<string>(),
               new List<string>(),
               new List<string>(),
            };
            foreach(var namePlayer in Players.Keys)
            {
                if(Players[namePlayer].isAdmin)
                    permissions[0].Add(namePlayer);
                if (Players[namePlayer].isModer)
                    permissions[1].Add(namePlayer);
                if (Players[namePlayer].isBuilder)
                    permissions[2].Add(namePlayer);
                if (Players[namePlayer].isBanned)
                    permissions[3].Add(namePlayer);
            }
            foreach (var player in PhotonNetWork.JDLAABPAIEA)
            {
                Level.BJANBGEILJD.photonView.RPC("SetActualData", player, new Il2CppSystem.Object[]
                {
                        GetArrayIL2CPP(permissions[0]),
                        GetArrayIL2CPP(permissions[1]),
                        GetArrayIL2CPP(permissions[2]),
                        GetArrayIL2CPP(permissions[3])
                });
            }
            GetActualData();
        }
        public class AMBB : ICloneable
        {
            public bool isAdmin { get; set; }
            public bool isModer { get; set; }
            public bool isBuilder { get; set; }
            public bool isBanned { get; set; }

            public object Clone()
            {
                return new AMBB()
                {
                    isAdmin = isAdmin,
                    isModer = isModer,
                    isBuilder = isBuilder,
                    isBanned = isBanned
                };
            }
        }
    }
    public static List<string> RevertListIL2cpp(this Il2CppSystem.Collections.Generic.List<string> vs)
    {
        List<string> list = new List<string>();
        foreach (var item in vs)
        {
            list.Add(item);
        }
        return list;
    }
    public static Il2CppSystem.Array GetArrayIL2CPP(List<string> array)
    {
        Il2CppSystem.Array array1 = Il2CppSystem.Array.CreateInstance(Il2CppSystem.String.Il2CppType, array.Count);
        for (int i = 0; i < array.Count; i++)
            array1.SetValue(array[i], i);
        return array1;
    }

    public static Camera GetCameraController => CameraController.BJANBGEILJD.ADCPNDNNNLC;

    public static void RPC(this PhotonView photon, string nameMethod , PhotonPlayer player, Il2CppSystem.Object[] objects)
    {
        photon.PLJLBBNLIFL(nameMethod, player, objects);
    }
    public static void RPC(this PhotonView photon, string nameMethod, PhotonTargets targets, Il2CppSystem.Object[] objects)
    {
        photon.PLJLBBNLIFL(nameMethod, targets, objects);
    }

}

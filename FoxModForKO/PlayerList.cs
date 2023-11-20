using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2CppMono;
using UnityEngine;
using PhotonPlayer = PJIKCJLCHHE;
using PhotonNetWork = PDEEDIKOEHC;
using PlayerNode = LLJLFMAKEBC;
using Sound = SoundManager.GMFNLNMNDIN;
using ChatF = RenameHash.ChatF;
using GUIF;
// allProperties = LPGPPHOLGBB;


//playerNode.HIBOFHLNAOO - id

//playerNode.PAHNCMHPKGF - name

public class PlayerList : IHackMenu
{
    private string textBox = "Text";
    public string Name => "PlayerList";
    public bool[] isActive = new bool[24];
    private Vector2 ScrollList = Vector2.zero;
    int idGun;
    public Action<int> Menu => (num) =>
    {
        for(int id = 0; id < PhotonNetWork.JDLAABPAIEA.Count; id++)
        {
            string playerName = PhotonNetWork.JDLAABPAIEA[id].GetName();
            BoxCast.ChildMenu(playerName, () =>
            {
                if (GUILayout.Button(playerName, null))
                {
                    Rect = new Rect(Rect.left, Rect.top, 200, 150);
                    isActive[id] = !isActive[id];
                }
                if (isActive[id])
                {
                    PlayerNode playerNode = WorldGameObjectL.BJANBGEILJD.OMPOJDEJPNN(PhotonNetWork.JDLAABPAIEA[id]);
                    GUILayout.TextField("name: " + playerName, null);
                    string VKid = PhotonNetWork.JDLAABPAIEA[id].LPGPPHOLGBB["player_id"].ToString();
                    if (VKid.Length > 20)
                        VKid.Remove(20);
                    GUILayout.TextField("vk_id: " + VKid, null);
                    GUILayout.TextField("steam_id: " + PhotonNetWork.JDLAABPAIEA[id].LPGPPHOLGBB["steam_id"].ToString(), null);
                    if (GUILayout.Button("Kick", null))
                    {
                        WorldGameObjectL.BJANBGEILJD.networkView.PLJLBBNLIFL("ExitGame", PhotonNetWork.JDLAABPAIEA[id], new Il2CppSystem.Object[] { "Kick" });
                    }
                    
                    if (GUILayout.Button("Kill", null))
                    {
                        WorldGameObjectL.BJANBGEILJD.OIAJJCPNCOO(PhotonNetWork.JDLAABPAIEA[id], Vector3.up, 999f, 0);
                    }
                    
                    
                    if (GUILayout.Button("TpToPlayer", null))
                    {
                        HookFox.MainPlayer.transform.position = playerNode.BIOKBPNIDAM.transform.position;
                    }
                    if (GUILayout.Button("TpPlayerToMe", null))
                    {
                        playerNode.BIOKBPNIDAM.GetComponent<PlayerNetwork>().photonView.PLJLBBNLIFL("SetPos", PhotonTargets.All, new Il2CppSystem.Object[]
                        {
                            HookFox.MainPlayer.transform.position.BoxIl2CppObject()
                        });
                    }
                    ScrollList = GUILayout.BeginScrollView(ScrollList, new GUILayoutOption[] { GUILayout.Height(75F) });
                    if (GUILayout.Button("Explosion", null))
                    {
                        WorldGameObjectL.BJANBGEILJD.networkView.PLJLBBNLIFL("Explosion", PhotonTargets.All, new Il2CppSystem.Object[] { playerNode.BIOKBPNIDAM.transform.position.BoxIl2CppObject() });
                    }
                    if (GUILayout.Button("Shoot", null))
                    {
                        CameraController.BJANBGEILJD.photonView.RPC("Shoot", PhotonTargets.All, new Il2CppSystem.Object[]
                        {
                        (playerNode.BIOKBPNIDAM.transform.position + new Vector3(0f, 30f, 0f)).BoxIl2CppObject(),
                        new Vector3(0f, -90f, 0f).BoxIl2CppObject(),
                        new Il2CppSystem.Single() { m_value = Time.time - 0.01f }.BoxIl2CppObject(),
                        new Il2CppSystem.Boolean() { m_value = true }.BoxIl2CppObject(),
                        new Il2CppSystem.Int32() { m_value = 1 }.BoxIl2CppObject()
                        });
                    }
                    if (GUILayout.Button("Punch", null))
                    {
                        WorldGameObjectL.BJANBGEILJD.OIAJJCPNCOO(PhotonNetWork.JDLAABPAIEA[id], Vector3.up, 0f, 0);
                    }
                    
                    if (GUILayout.Button("Die", null))
                    {
                        playerNode.BIOKBPNIDAM.GetComponent<PlayerNetwork>().photonView.PLJLBBNLIFL("Die", PhotonTargets.All, new Il2CppSystem.Object[0]);
                    }
                    idGun = int.Parse(GUILayout.TextField(idGun.ToString(), null));
                    if (GUILayout.Button("GiveGun", null))
                    {
                        playerNode.BIOKBPNIDAM.GetComponent<PhotonView>().RPC("EnableWeapon", PhotonTargets.All, new Il2CppSystem.Object[]
                        {
                        new Il2CppSystem.Int32() { m_value = idGun }.BoxIl2CppObject()
                        });
                    }
                    if (GUILayout.Button("Whistle Player", null))
                    {
                        Il2CppSystem.Int32 sound = new Il2CppSystem.Int32();
                        sound.m_value = (int)Sound.ShortWhistle;

                        WorldGameObjectL.BJANBGEILJD.networkView.PLJLBBNLIFL("SoundRPC", PhotonTargets.All, new Il2CppSystem.Object[]
                        {
                        sound.BoxIl2CppObject(),
                        playerNode.BIOKBPNIDAM.transform.position.BoxIl2CppObject()
                        });
                    }
                    GUILayout.EndScrollView();
                    textBox = GUILayout.TextField(textBox, null);
                    if (GUILayout.Button("SendText", null))
                    {
                        ChatF.Text(playerName + ": " + textBox, true);
                    }
                    if (GUILayout.Button("SendLocal", null))
                    {
                        ChatF.SetTextLocal(textBox, PhotonNetWork.JDLAABPAIEA[id]);
                    }

                }
            });
            //GUILayout.Label("isMaster " + player.DHDJAMLAPLF, null);
        }
        GUI.DragWindow();
    };

    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(100, 100, 200, 100);
}

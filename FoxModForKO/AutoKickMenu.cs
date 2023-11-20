using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using UnityEngine;
using Photon;
using UnhollowerBaseLib;
using Il2CppICSharpCode;
using Il2CppSystem;
using Il2CppMicrosoft;
using Il2CppMono;
using World = LBJEMKPILPE;
using IntVect = AECPBBDPBHB;
using PlayerNetWork = PDEEDIKOEHC;
using PhotonPlayer = PJIKCJLCHHE;
using PhotonNetWork = PDEEDIKOEHC;
using PlayerNode = LLJLFMAKEBC;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
// allProperties = LPGPPHOLGBB;

public class AutoKickMenu : IHackMenu, IUpdate, IStarted
{

    List<string> Players;
    public string Name => "AutoKick";
    private string IdPlayerSteam = "idSteam";
    private bool IsPlay = false;
    public System.Action<int> Menu => (id) =>
    {
        IdPlayerSteam = GUILayout.TextField(IdPlayerSteam, null);
        if(GUILayout.Button("Add ID", null))
        {
            Players.Add(IdPlayerSteam);
            SavingConfig.Save(SavingConfig.DirectoryType.Config, "AutoKick" , Players);
        }
        if(GUILayout.Button("AutoKick: " + (IsPlay ? "On":"Off"), null))
        {
            IsPlay = !IsPlay;
        }
        for(int i = 0; i< Players.Count(); i++)
            if(GUILayout.Button("Del in: " + Players[i], null))
            {
                Players.RemoveAt(i);
                SavingConfig.Save(SavingConfig.DirectoryType.Config, "AutoKick", Players);
            }
        GUI.DragWindow();
    };
    public void Update()
    {
       if(IsPlay)
        {
            foreach(var palyer in PhotonNetWork.JDLAABPAIEA)
            {
                foreach(string id in Players)
                    if(palyer.LPGPPHOLGBB["steam_id"].ToString() == id)
                        WorldGameObjectL.BJANBGEILJD.networkView.PLJLBBNLIFL("ExitGame", palyer, new Il2CppSystem.Object[] { "Kick" });
            }
        }
    }

    public void Start()
    {
        Players = SavingConfig.Open<List<string>>(SavingConfig.DirectoryType.Config, "AutoKick"); ;
        if (Players == null)
            Players = new List<string>();
    }

    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(100, 100, 250, 100);
}

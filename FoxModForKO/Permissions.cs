using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PhotonPlayer = PJIKCJLCHHE;
using PhotonNetWork = PDEEDIKOEHC;
using PlayerNode = LLJLFMAKEBC;
using ChatF = RenameHash.ChatF;
using LevelF = RenameHash.LevelF;
using AMBB = RenameHash.LevelF.AMBB;
using GUIF;

public class Permissions : IHackMenu, IStarted, IUpdate
{
    public string Name => "Permissions";
    private string playerNameNew;

    private bool isSetPermissionsUpdate;

    private AMBB playerAMBBNew = new AMBB();

    private Vector2 playerOnlineScroll = Vector2.zero, playerOfflineScroll = Vector2.zero;
    private void GiveMyPermissions(int idPermissions)
    {
        LevelF.GetActualData();
        AMBB aMBB = LevelF.GetOnlinePlayer()[RenameHash.MyName];
        switch (idPermissions)
        {
            case 0:
                aMBB.isAdmin = !aMBB.isAdmin;
                break;
            case 1:
                aMBB.isModer = !aMBB.isModer;
                break;
            case 2:
                aMBB.isBuilder = !aMBB.isBuilder;
                break;
            case 3:
                aMBB.isBanned = !aMBB.isBanned;
                break;
        }
        LevelF.SetActualData();
    }

    public void Start()
    {
        HookFox.StartGame += LevelF.GetActualData;
        HookFox.StartGame += LevelF.SetActualData;
        HookFox.LeavedGame += LevelF.ClearAll;
    }

    public void Update()
    {
        if(isSetPermissionsUpdate && HookFox.IsStaredGame)
        {
            LevelF.GetActualData();
            LevelF.SetActualData();
        }
    }

    public System.Action<int> Menu => (id) =>
        {
            BoxCast.ChildMenu("My Permissions", () =>
            {
                GUILayout.BeginHorizontal(null);
                if (GUILayout.Button("A", null))
                {
                    GiveMyPermissions(0);
                }
                if (GUILayout.Button("M", null))
                {
                    GiveMyPermissions(1);
                }
                if (GUILayout.Button("B", null))
                {
                    GiveMyPermissions(2);
                }
                if (GUILayout.Button("Ban", null))
                {
                    GiveMyPermissions(3);
                }
                GUILayout.EndHorizontal();
            });
            LevelF.isMySet = GUILayout.Toggle(LevelF.isMySet, "AutoSetMe", "Button", null);
            if (GUILayout.Button("GetActualData", null))
            {
                LevelF.GetActualData();
            }

            if (GUILayout.Button("SetPermissions", null))
            {
                LevelF.SetActualData();
            }
            isSetPermissionsUpdate = GUILayout.Toggle(isSetPermissionsUpdate, "SetUpdate", "Button", null);
            BoxCast.ChildMenu("AddPlayer", () =>
            {
                playerNameNew = GUILayout.TextArea(playerNameNew, null);
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                playerAMBBNew.isAdmin = GUILayout.Toggle(playerAMBBNew.isAdmin, "A", "Button", null);
                playerAMBBNew.isModer = GUILayout.Toggle(playerAMBBNew.isModer, "M", "Button", null);
                playerAMBBNew.isBuilder = GUILayout.Toggle(playerAMBBNew.isBuilder, "B", "Button", null);
                playerAMBBNew.isBanned = GUILayout.Toggle(playerAMBBNew.isBanned, "Ban", "Button", null);
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Add", null))
                {
                    LevelF.AddPlayerOffline(playerNameNew, (AMBB)playerAMBBNew.Clone());
                }
            });
            BoxCast.ChildMenu("OnlinePlayer", () =>
            {
                playerOnlineScroll = GUILayout.BeginScrollView(playerOnlineScroll, new GUILayoutOption[] { GUILayout.Height(200), GUILayout.Width(150) });
                foreach(var player in LevelF.GetOnlinePlayer())
                {
                    GUILayout.BeginHorizontal(null);
                    GUILayoutUtility.GetRect(6,1);
                    GUILayout.BeginVertical(null);
                    string name = player.Key;
                    BoxCast.ChildMenu(name, () =>
                    {
                        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                        player.Value.isAdmin = GUILayout.Toggle(player.Value.isAdmin, "A", "Button", null);
                        player.Value.isModer = GUILayout.Toggle(player.Value.isModer, "M", "Button", null);
                        player.Value.isBuilder = GUILayout.Toggle(player.Value.isBuilder, "B", "Button", null);
                        player.Value.isBanned = GUILayout.Toggle(player.Value.isBanned, "Ban", "Button", null);
                        GUILayout.EndHorizontal();
                    });
                    GUILayout.EndVertical();
                    GUILayoutUtility.GetRect(6, 1);
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            });
            BoxCast.ChildMenu("Offline", () =>
            {
                playerOfflineScroll = GUILayout.BeginScrollView(playerOfflineScroll, new GUILayoutOption[] { GUILayout.Height(200), GUILayout.Width(150) });
                foreach (var player in LevelF.GetOfflinePlayer())
                {
                    GUILayout.BeginHorizontal(null);
                    GUILayoutUtility.GetRect(6, 1);
                    GUILayout.BeginVertical(null);
                    string name = player.Key;
                    BoxCast.ChildMenu(name, () =>
                    {
                        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                        player.Value.isAdmin = GUILayout.Toggle(player.Value.isAdmin, "A", "Button", null);
                        player.Value.isModer = GUILayout.Toggle(player.Value.isModer, "M", "Button", null);
                        player.Value.isBuilder = GUILayout.Toggle(player.Value.isBuilder, "B", "Button", null);
                        player.Value.isBanned = GUILayout.Toggle(player.Value.isBanned, "Ban", "Button", null);
                        GUILayout.EndHorizontal();
                    });
                    GUILayout.EndVertical();
                    GUILayoutUtility.GetRect(6, 1);
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            });
            if (GUI.changed)
            {
                Rect = new Rect(Rect.left, Rect.top, 150, 200);
            }
            GUI.DragWindow();
        };

    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(100,100,150,200);
}

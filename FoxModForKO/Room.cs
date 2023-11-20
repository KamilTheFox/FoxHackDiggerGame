using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using PhotonPlayer = PJIKCJLCHHE;
using PhotonNetWork = PDEEDIKOEHC;
using PlayerNode = LLJLFMAKEBC;
using RoomInfo = GPPGFFKFHCN;

public class Room : IHackMenu
{
    public string Name => "Room";

    public Action<int> Menu => (id) =>
    {
        foreach (var room in PhotonNetWork.FEDOFCEMNMA())
        {
            GUILayout.Box(room.IMAGONFLHMB["map_name"].ToString(), null);
            if (room.IMAGONFLHMB["password"].ToString() == "")
            {
                if (GUILayout.Button("join", null))
                {
                    PhotonNetWork.NHNAIMNEMMF(room.IMAGONFLHMB["map_name"].ToString());
                    MainMenu.BJANBGEILJD.PPOAFIFEPEB(room);
                }
            }
            else
                if (GUILayout.Button("GetHashMD5", null))
            {
                GUIUtility.systemCopyBuffer = room.IMAGONFLHMB["password"].ToString();
                Application.OpenURL("https://crackstation.net");
            }
            GUILayout.TextField("player_id: " + room.IMAGONFLHMB["player_id"].ToString(), null);
        }
        GUI.DragWindow();
    };

    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(100, 100, 120, 100);
}

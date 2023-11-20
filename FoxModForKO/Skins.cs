using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Skins : IHackMenu
{
    public string Name => "Skins";
    private float scrollSkin;

    private readonly List<string> NameSkin = new List<string>()
    {
        "Default",
        "Pirate",
        "Knight",
        "Gold_Knight",
        "Pirate 2",
        "Viking",
        "Zombe",
        "Death_Knight",
        "Stalker",
        "CCCR",
        "SWAT",
        "USA",
        "Hitler",
        "Recruit",
        "Skeleton",
        "Iron_Men",
        "BatMan",
        "Girl",
        "Girl_2",
        "Archer",
        "Killer",
        "Cool",
        "Dark_Stalker",
        "Terror",
        "forge",
        "None",
        "HackSkin_1",
        "HackSkin_2",
        "Default",
        "Policeman",
        "Santa"
    };
    public Action<int> Menu => (id) =>
    {
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        scrollSkin = GUILayout.VerticalSlider(scrollSkin, 0, 32 * 20, null);
        Rect area = GUILayoutUtility.GetRect(130, 250);
        GUI.Box(area, "");
        GUI.BeginGroup(area);
        for (int i = 0; i < 31; i++)
        {
            if (GUI.Button(new Rect(5, 20 * i - scrollSkin, area.width - 10, 20), NameSkin[i] == null ? "Skin: " + i.ToString() : NameSkin[i]))
            {
                    MHOHKKIABFL.FAHHKIDIEMF(i);
                    MHOHKKIABFL.FJDKFEHJCOC = new POLHNJLGFKP<int>(i);
            }
        }
        GUI.EndGroup();
        GUILayout.EndHorizontal();
        GUI.DragWindow();
    };

    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(100,100,130,100);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

internal class Tablet : IHackMenu
{
    public string Name => "Tablet";

    public Action<int> Menu => MenuTablet;
    
    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(100,100,200,100);

    private string[] TextLine = { "", "", "", ""};

    private bool[,] TextSettings =
    {
        {false,false,false, false, false, false},
        {false,false,false, false, false, false},
        {false,false,false, false, false, false},
        {false,false,false, false, false, false},
    };
    private Color color1, color2;
    private float r = 1F, g = 0.5F, b, r2, g2 = 1F, b2;
    private int Size;
    private string Mod(string texts, bool bool1, bool bool2)
    {
        string text = texts;
        if (bool1)
        {
            text = "<i>" + text + "</i>";
        }
        if (bool2)
        {
            text = "<b>" + text + "</b>";
        }
        return text;
    }
    private string Gradient(string text)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            stringBuilder.Append(
            string.Concat(new string[]
             {
                "<color=#",
                ColorUtility.ToHtmlStringRGB(Color.Lerp(color1, color2, (float)i / text.Length)),
                ">",
                text[i].ToString(),
                "</color>"
             }));
        }
        return stringBuilder.ToString();
    }
    private string SetColor(string text)
    {
        return string.Concat(new string[]
            {
            "<color=#",
            ColorUtility.ToHtmlStringRGB(color1),
            ">",
            text,
            "</color>"
            });
    }
    private string SetSize(string text)
    {
        return string.Concat(new string[]
            {
            "<size=",
            Size.ToString(),
            ">",
            text,
            "</size>"
            });
    }
    private void MenuTablet(int id)
    {
        for(int i = 0; i < TextLine.Length; i++)
        {
            TextLine[i] = GUILayout.TextField(TextLine[i], null);
            GUILayout.BeginHorizontal(null);
            TextSettings[i, 0] = GUILayout.Toggle(TextSettings[i, 0], "<i>ABC</i>", null);
            TextSettings[i, 1] = GUILayout.Toggle(TextSettings[i, 1], "<b>ABC</b>", null);
            GUILayout.EndHorizontal();
            TextSettings[i, 2] = GUILayout.Toggle(TextSettings[i, 2], Gradient("Gradient"), null);
            TextSettings[i, 3] = GUILayout.Toggle(TextSettings[i, 3], SetColor("Color"), null);
            GUILayout.BeginHorizontal(null);
            TextSettings[i, 4] = GUILayout.Toggle(TextSettings[i, 4], "Size", null);
            TextSettings[i, 5] = GUILayout.Toggle(TextSettings[i, 5], "LO.", null);
            GUILayout.EndHorizontal();
        }
        GUILayout.Label("Size: " + Size.ToString(), null);
        Size = (int)GUILayout.HorizontalSlider(Size, 0, 200000, null);
        GUI.color = color1;
        r = GUILayout.HorizontalSlider(r, 0F, 1F, null);
        g = GUILayout.HorizontalSlider(g, 0F, 1F, null);
        b = GUILayout.HorizontalSlider(b, 0F, 1F, null);
        color1 = new Color(r, g, b);
        GUI.color = color2;
        r2 = GUILayout.HorizontalSlider(r2, 0F, 1F, null);
        g2 = GUILayout.HorizontalSlider(g2, 0F, 1F, null);
        b2 = GUILayout.HorizontalSlider(b2, 0F, 1F, null);
        color2 = new Color(r2, g2, b2);
        GUI.color = SettingsMenu.ColorContent;
        //if(GUILayout.Button("GetText", null))
        //{
        //    Ray ray = BuildAPI.GetRayInCamera();
        //    if (Physics.Raycast(ray, out RaycastHit hit, 999f, 1 << EntityBase.NEKNPHKOLIF | 1 << EntityBase.PGLJDGCMLIB | 1 << EntityBase.COEJBHHHCMF))
        //    {
        //        EntityBase entity = hit.collider.gameObject.GetComponentInParent<EntityBase>();
        //        Tablichka tablichka = entity.GetComponent<Tablichka>();
        //        TextLine[0] = tablichka.AOPIPKJFDCP.GetComponent<TextMesh>();
        //        TextLine[1] = tablichka.BEDJKNBEKGA.GetComponent<TextMesh>();
        //        TextLine[2] = tablichka.EILMKJFMNPN.GetComponent<TextMesh>();
        //        TextLine[3] = tablichka.AFGAJPGIDEL.GetComponent<TextMesh>();
        //    }
        //}
        if (GUILayout.Button("SetText", null))
        {
            string[] lines =
            {
                TextLine[0],
                TextLine[1],
                TextLine[2],
                TextLine[3]
            };
            for(int i = 0; i < lines.Length; i++)
            {
                if (TextSettings[i, 3] && !TextSettings[i, 2])
                {
                    lines[i] = SetColor(lines[i]);
                }
                if (TextSettings[i, 2])
                {
                    lines[i] = Gradient(lines[i]);
                }
                if(TextSettings[i, 5])
                {
                    lines[i] = "LO." + lines[i];
                }
                if (TextSettings[i, 4])
                {
                    lines[i] = SetSize(lines[i]);
                }

            }
            Ray ray = BuildAPI.GetRayInCamera();
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, 1 << EntityBase.NEKNPHKOLIF | 1 << EntityBase.PGLJDGCMLIB | 1 << EntityBase.COEJBHHHCMF))
            {
                EntityBase entity = hit.collider.gameObject.GetComponentInParent<EntityBase>();
                Tablichka tablichka = entity.GetComponent<Tablichka>();
                tablichka.photonView.RPC("SetText", PhotonTargets.All, new Il2CppSystem.Object[]
                    {
                        Mod(    lines[0]     ,TextSettings[0, 0],TextSettings[0, 1]),
                        Mod(   lines[1]      ,TextSettings[1, 0],TextSettings[1, 1]),
                        Mod(    lines[2]     ,TextSettings[2, 0],TextSettings[2, 1]),
                        Mod(    lines[3]     ,TextSettings[3, 0],TextSettings[3, 1]),
                    });

            }
        }
        GUI.DragWindow();
    }
    public void Update()
    {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;
using PhotonPlayer = PJIKCJLCHHE;
using PhotonNetWork = PDEEDIKOEHC;
using ChatF = RenameHash.ChatF;
using GUIF;
using UnhollowerRuntimeLib;

public class Other : IHackMenu, IUpdate, IStarted
{
    public string Name => "Other";

    private string TextArea = "";

    private float timeOfDay;
    private bool changeTimeOfDay, isNewSkyBox, isGradiend, isChangeSlotID;

    private Color colorChat1 = new Color(1F,0.5F, 0F), colorChat2 = new Color(0F, 1F, 0F);

    private float r = 1F, g = 0.5F, b = 0F;
    private float r2, g2 = 1F, b2;

    private enum TypeChat
    {
        Info,
        Text,
        Green,
        Warning,
        Gradient
    }
    private TypeChat typeChat;
    private bool spam;

    private bool isGradientChat;

    private string connect1 = "Игрок ", connect2 = " зашел на карту";
    public Action<int> Menu => (id) =>
    {
        BoxCast.ChildMenu("ChangeTime", () =>
        {
            timeOfDay = GUILayout.HorizontalSlider(timeOfDay, -1, isNewSkyBox ? 24 : 1, null);
            if (isNewSkyBox)
            {
                changeTimeOfDay = false;
                if (GUILayout.Button("SetHour: " + timeOfDay.ToString("0.00"), null))
                {
                    TimeOfDay.NBGJMAHAKAD.IJLBJAHJDLD.networkView.PLJLBBNLIFL("SetHour", PhotonTargets.All, new Il2CppSystem.Object[]
                {
                new Il2CppSystem.Single() { m_value = timeOfDay }.BoxIl2CppObject()
                });
                }
                TOD_Sky toD_Sky = GameObject.FindObjectOfType<TOD_Sky>();
                if (toD_Sky)
                {
                    var Clouds = GameObject.FindObjectOfType<TOD_Sky>().transform.GetChild(7).gameObject;
                    if (GUILayout.Button("Clouds: " + (Clouds.active ? "On" : "Off"), null))
                    {
                        Clouds.SetActive(!Clouds.active);
                    }
                }
            }
            else
                changeTimeOfDay = GUILayout.Toggle(changeTimeOfDay, "UseChangeTime: " + timeOfDay.ToString("0.00"), null);
            isNewSkyBox = GUILayout.Toggle(isNewSkyBox, "isNewSkyBox", null);
        });
        GUILayoutUtility.GetRect(10f, 5f);
        isChangeSlotID = GUILayout.Toggle(isChangeSlotID, "isChangeSlotID", null);
        if (MainMenu.BJANBGEILJD && isChangeSlotID)
            MainMenu.BJANBGEILJD.HFEEOOCJCFP = int.Parse(GUILayout.TextField(MainMenu.BJANBGEILJD.HFEEOOCJCFP.ToString(), null));
        if (RenameHash.GetRoom != null)
        {
            RenameHash.GetRoom.CGJLJLJCDAF = (int)GUILayout.HorizontalSlider((float)RenameHash.GetRoom.CGJLJLJCDAF, 0, 24, null);
        }
        BoxCast.ChildMenu("Connect Info", () =>
        {
            connect1 = GUILayout.TextField(connect1, null);
            connect2 = GUILayout.TextField(connect2, null);
            if(GUILayout.Button("Set Info Connect", null))
            {
                string textSearch1 = "PlayerConnect1", textSearch2 = "PlayerConnect2";
                List<string> vs = new List<string>();
                
                for (int i = 3; i< Localize.BJCFJOCIDEO.Count; i++)
                {
                    string text = Localize.BJCFJOCIDEO[i];
                    if ((Localize.BJCFJOCIDEO[i-1] == textSearch1 || Localize.BJCFJOCIDEO[i - 2] == textSearch1))
                        vs.Add(connect1);
                    else if((Localize.BJCFJOCIDEO[i - 1] == textSearch2 || Localize.BJCFJOCIDEO[i - 2] == textSearch2))
                        vs.Add(connect2);
                    else
                    vs.Add(text);
                }
                Localize.BJCFJOCIDEO = vs.ToArray();
            }

        });
        BoxCast.ChildMenu("Chat", () =>
        {
            TextArea = GUILayout.TextArea(TextArea, null);
            GUILayout.BeginHorizontal(null);
            Color oldColor = GUI.color;
            GUI.color = Color.white;
            if (GUILayout.Button(" ", null))
            {
                typeChat = TypeChat.Text;
                SendMessage(typeChat, TextArea);
            }
            GUI.color = Color.red;
            if (GUILayout.Button(" ", null))
            {
                typeChat = TypeChat.Warning;
                SendMessage(typeChat, TextArea);
            }
            GUI.color = Color.yellow;
            if (GUILayout.Button(" ", null))
            {
                typeChat = TypeChat.Info;
                SendMessage(typeChat, TextArea);
            }
            GUI.color = Color.green;
            if (GUILayout.Button(" ", null))
            {
                typeChat = TypeChat.Green;
                SendMessage(typeChat, TextArea);
            }
            GUI.color = Color.Lerp(colorChat1, colorChat2, Mathf.PingPong(Time.time, 0.7f));
            if (GUILayout.Button(" ", null))
            {
                typeChat = TypeChat.Gradient;
                SendMessage(typeChat, TextArea);
            }
            GUILayout.EndHorizontal();
            GUI.color = oldColor;
            spam = GUILayout.Toggle(spam, "Spam", null);
            isGradiend = GUILayout.Toggle(isGradiend, "Gradiend", null);
            if (isGradiend)
            {
                GUI.color = colorChat1;
                r = GUILayout.HorizontalSlider(r, 0F, 1F, null);
                g = GUILayout.HorizontalSlider(g, 0F, 1F, null);
                b = GUILayout.HorizontalSlider(b, 0F, 1F, null);
                colorChat1 = new Color(r, g, b);
                GUI.color = colorChat2;
                r2 = GUILayout.HorizontalSlider(r2, 0F, 1F, null);
                g2 = GUILayout.HorizontalSlider(g2, 0F, 1F, null);
                b2 = GUILayout.HorizontalSlider(b2, 0F, 1F, null);
                colorChat2 = new Color(r2, g2, b2);
                GUI.color = oldColor;
            }
            if(GUILayout.Button($"GradientInput: {(isGradientChat ? "on" : "off")}", null))
            {
                isGradientChat = !isGradientChat;
                PlayerPrefs.SetInt("isGradientChat", (isGradientChat ? 1 : 0));
                if(isGradientChat)
                    Chat.NBGJMAHAKAD.GKNOEOIDFLJ.JJLNHEJDDDI = new Action<string>(OnSubmit);
            }
        });
        if (GUI.changed)
        {
            Rect = new Rect(Rect.left, Rect.top, 100, 100);
        }
        GUI.DragWindow();
    };
    public void Start()
    {
        isGradientChat = PlayerPrefs.GetInt("isGradientChat", 0) == 1;
        HookFox.StartGame += () =>
        {
            Chat.NBGJMAHAKAD.GKNOEOIDFLJ.BIOEIELBHCD = 9999;
        };
        HookFox.StartGame += () =>
        {
            if(isGradientChat)
                Chat.NBGJMAHAKAD.GKNOEOIDFLJ.JJLNHEJDDDI = new Action<string>(OnSubmit);
        };
    }
    private void OnSubmit(string inputText)
    {
        if (!string.IsNullOrEmpty(inputText))
        {
            ChatF.Text(string.Concat(new string[]
            {
                RenameHash.MyName,
                ": ",
                GradientText(inputText)
            }), true);
            Chat.NBGJMAHAKAD.GKNOEOIDFLJ.CCLEBHPCDEI = string.Empty;
            Chat.NBGJMAHAKAD.GKNOEOIDFLJ.DJMBKPAAIFA = string.Empty;
            Chat.NBGJMAHAKAD.GKNOEOIDFLJ.LICHDIIJLMJ = false;
        }
        Chat.NBGJMAHAKAD.GBMFLOACHOJ = true;
    }
    private string GradientText(string text)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            builder.Append($"[{ColorUtility.ToHtmlStringRGB(Color.Lerp(colorChat1, colorChat2, (float)i / text.Length))}]{text[i]}");
        }
        return builder.ToString();
    }
    private void SendMessage(TypeChat type, string text)
    {
        switch (type)
        {
            case TypeChat.Info:
                ChatF.Info(" " + text, true);
                break;
            case TypeChat.Text:
                ChatF.Text(text, true);
                break;
            case TypeChat.Gradient:
                ChatF.Text(GradientText(text), true);
                break;
            case TypeChat.Warning:
                ChatF.Warning(" " + text, true);
                break;
            case TypeChat.Green:
                ChatF.Info(" [00ff00]" + text, true);
                break;
        }
    }
    public void Update()
    {
        if(spam && Time.frameCount % 5 == 0)
        {
            SendMessage(typeChat, TextArea);
        }
        if(changeTimeOfDay)
        {
            TimeOfDay.NBGJMAHAKAD.networkView.PLJLBBNLIFL("InitializeTimeInternal", PhotonTargets.All, new Il2CppSystem.Object[]
            {
                new Il2CppSystem.Single() { m_value = timeOfDay }.BoxIl2CppObject(),
                new Il2CppSystem.Int32() { m_value = 1 }.BoxIl2CppObject(),
                new Il2CppSystem.Single() { m_value = 0F }.BoxIl2CppObject(),
            });
        }
    }

    

    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(100,100,200,100);
}

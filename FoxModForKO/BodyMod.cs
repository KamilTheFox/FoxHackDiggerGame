using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Il2CppSystem;

internal class BodyMod : IHackMenu, IStarted
{
    private bool Fly;
    private bool Speed;
    private float MultySpeed;
    private bool Ghost;
    private bool Free_Famera;
    private bool Test1, Test2, Test3;
    //private CameraMoving camera;

    public string Name => "BodyMod";

    private Dictionary<string, string> m_Mods = new Dictionary<string, string>()
    {
        ["Speed"] = "ADCBGFNCLBC",
        ["Fly"] = "BLGGDOELFLJ",
        ["SpeedValue"] = "MEFMJJCBJFC",
        ["test"] = "ONLABADAAPG",
    };

    public System.Action<int> Menu => (id) =>
    {
        
        GameObject game = HookFox.MainPlayer;
        
        //if (GUILayout.Button("Test1 " + (Test1 ? "On" : "Off"), null))
        //{
        //    Test1 = !Test1;
        //    Il2CppSystem.Boolean boolean = new Il2CppSystem.Boolean();
        //    boolean.m_value = Test1;
        //    game.GetComponent<PlayerMotor>().SendMessage(m_Mods["test"], boolean.BoxIl2CppObject());
        //}
        
        if (GUILayout.Button("Speed " + (Speed ? "On" : "Off"), null))
        {
            Speed = !Speed;
            Il2CppSystem.Boolean boolean = new Il2CppSystem.Boolean();
            boolean.m_value = Speed;
            game.GetComponent<PlayerMotor>().SendMessage(m_Mods["Speed"], boolean.BoxIl2CppObject());
        }
        MultySpeed = GUILayout.HorizontalScrollbar(MultySpeed, 0.1f, 1, 10.1f, null);
        if(GUILayout.Button($"SetSpeed: {MultySpeed}", null ))
        {
            GameObject.FindObjectOfType<PlayerInput>().MEFMJJCBJFC(MultySpeed);
        }
        if (GUILayout.Button("Fly " + (Fly ? "On" : "Off"), null))
        {
            Fly = !Fly;
            Il2CppSystem.Boolean boolean = new Il2CppSystem.Boolean();
            boolean.m_value = Fly;
            game.GetComponent<PlayerMotor>().SendMessage(m_Mods["Fly"], boolean.BoxIl2CppObject());
        }
        if(GUILayout.Button("Ghost: " + (Ghost ? "On" : "Off"), null))
        {
            Ghost = !Ghost;
        }
        if(game)
            Physics.IgnoreLayerCollision(game.layer, LayerMask.NameToLayer("Terrain"), Ghost);
        GUI.DragWindow();
    };

    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(100, 100, 120, 100);

    public void Start()
    {
        //camera.Camera = Camera.main;
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using System.Windows.Forms;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class MenuSaveBuild : IHackMenu
{
    private static string GetPath => SavingConfig.GetPathDirectory(SavingConfig.DirectoryType.Builds);

    public string Name => "SaveBuild";

    private bool isDelete;

    private string nameFile = "name";
    private float scrollFiles = 0F;
    public Action<int> Menu => (id) =>
    {
        GUILayout.Label("name file", null);
        nameFile = GUILayout.TextField(nameFile, null);
        if (GUILayout.Button("Save", null))
        {
            SavingConfig.Save(SavingConfig.DirectoryType.Builds, nameFile, Building.Instance.Build.ToArray());
        }
        if (GUILayout.Button("Read", null))
        {
            if (nameFile.Contains(".json"))
            {
                string textJson = SavingConfig.OpenJson(SavingConfig.DirectoryType.Builds, nameFile);
                Building.Instance.Build = SavingConfig.JsonReadMerts(textJson).ToList();
            }
            else
                Building.Instance.Build = SavingConfig.Open<BlockInfo[]>(SavingConfig.DirectoryType.Builds, nameFile).ToList();
        }
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        string[] vs = GetNames;
        scrollFiles = GUILayout.VerticalSlider(scrollFiles, 0, vs.Length * 20, null);
        Rect area = GUILayoutUtility.GetRect(130, 250);
        GUI.Box(area, "");
        GUI.BeginGroup(area);
        for (int i = 0; i < vs.Length; i++)
        {
            if (GUI.Button(new Rect(5, 20 * i - scrollFiles, area.width - 10, 20), vs[i].Replace(".fox", "")))
            {
                if(isDelete)
                {
                    File.Delete(GetPath + "\\" + vs[i]);
                }
                else
                nameFile = vs[i].Replace(".fox", "");
            }
        }
        GUI.EndGroup();
        GUILayout.EndHorizontal();
        isDelete = GUILayout.Toggle(isDelete, "isDeleteFile", null);
        if (GUILayout.Button("OpenDirectory", null))
        {
            OpenDirectory();
        }
        GUI.DragWindow();
    };
    private string[] GetNames
    { 
        get
        {
            if (Directory.Exists(GetPath))
            {
                return Directory.GetFiles(GetPath).Select(t => t.Replace(GetPath + "\\", "")).ToArray();
            }
            return new string[] { "NonFile" };
        }
    }
    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(100, 100, 120,100);

    private static void OpenDirectory()
    {
        Process.Start(GetPath);
    }
   
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


public static class SavingConfig
{
    private static string GetPath => UnityEngine.Application.persistentDataPath;

    public static string GetPathDirectory(DirectoryType directory) => GetPath + $"\\{directory}";

    public static void ClearAllFiles(DirectoryType type)
    {
        if(Directory.Exists(GetPathDirectory(type)))
            Directory.Delete(GetPathDirectory(type), true);
    }
    public static void Save<T>(DirectoryType type, string fileName , T valuePairs)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        if (!Directory.Exists(GetPathDirectory(type)))
            Directory.CreateDirectory(GetPathDirectory(type));
        string file = GetPath + $"\\{type}\\{fileName}{type.GetTypeFile()}";
        using (FileStream fileStream = File.Create(file))
        {
            binaryFormatter.Serialize(fileStream, valuePairs);
        }
    }
    public static T Open<T>(DirectoryType type, string fileName)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        if (!Directory.Exists(GetPathDirectory(type)))
            Directory.CreateDirectory(GetPathDirectory(type));
        string file = GetPath + $"\\{type}\\{fileName}{type.GetTypeFile()}";
        if (File.Exists(file))
            using (FileStream fileStream = File.OpenRead(file))
            {
                return (T)binaryFormatter.Deserialize(fileStream);
            }
        return default(T);
    }
    public static string OpenJson(DirectoryType type, string fileName)
    {
        if (!Directory.Exists(GetPath))
            Directory.CreateDirectory(GetPath);
        string file = GetPath + $"\\{type}\\{fileName}";
        if (File.Exists(file))
        {
            return File.ReadAllText(file);
        }
        UnityEngine.Debug.LogError("Null json File");
        return "";

    }

    public static BlockInfo[] JsonReadMerts(string json)
    {
        //BuildModMerts buildMod = UnityEngine.JsonUtility.FromJson<BuildModMerts>(json); //Сука не работает блять
        json = json.Replace(" ", "").Replace("   ", "").Replace("\n", "");
        MatchCollection matches = Regex.Matches(json, @"{(.*?)},");
        List<BlockInfo> blocks = new List<BlockInfo>();
        UnityEngine.Debug.Log(matches.Count.ToString());
        foreach (Match block in matches)
        {
            string type = Regex.Match(block.Value, @"""blockType"":([0-9]+)").Groups[1].Value;
            string kind = Regex.Match(block.Value, @"""blockKind"":([0-9]+)").Groups[1].Value;
            string x = Regex.Match(block.Value, @"""x"":([0-9]+)").Groups[1].Value;
            string y = Regex.Match(block.Value, @"""y"":([0-9]+)").Groups[1].Value;
            string z = Regex.Match(block.Value, @"""z"":([0-9]+)").Groups[1].Value;
            if(int.TryParse(type, out int blockT) &&
                int.TryParse(kind, out int blockK) &&
                int.TryParse(x, out int X) &&
                    int.TryParse(y, out int Y) &&
                int.TryParse(z, out int Z))
            blocks.Add(new BlockInfo()
            {
                BlockType = blockT,
                z = Y,
                BlockKind = blockK,
                x = X,
                y = Z
            });
        }
        return blocks.ToArray();
    }
    private static string GetTypeFile(this DirectoryType type)
    {
        string text = ".fox";
        switch (type)
        {
            case DirectoryType.Config:  return ".cnfg";
            case DirectoryType.Builds: return ".fox";
            case DirectoryType.ReturnBuild: return ".rtn";
        }
        return text;
    }
    public enum DirectoryType
    {
        Config,
        Builds,
        Json,
        ReturnBuild
    }
}

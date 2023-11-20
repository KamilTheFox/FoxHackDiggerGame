using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using UnityEngine;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using Graphics = System.Drawing.Graphics;
using Dithering;

public class PixelArt : IHackMenu, IGenerate
{
    public string Name => nameof(PixelArt);

    public Action<int> Menu => MainMenu;

    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(0, 0, 1000, 500);

    public BlockInfo[] GetBuild { get; private set; }

    private string PathImage = "D://Test.png";

    private Bitmap bitmap;

    private string jsonBlock;

    public static int[] settingsImage =
    {
        (int)InterpolationMode.HighQualityBicubic,
        (int)CompositingQuality.Default,
        (int)SmoothingMode.Default,
        (int)PixelOffsetMode.Default,
        (int)TypeDithering.None,
    };
    public Vector2[] settingsImageScroll = new Vector2[7];

    private int wight = 100, height = 100;
    private string s_wight = "100", s_height = "100";

    private Texture2D ImageOrigin = new Texture2D(4, 4), ImageConvert = new Texture2D(4,4);

    public void Regenerate()
    {
        GetBuild = SavingConfig.JsonReadMerts(jsonBlock);
    }
    private void MainMenu(int id)
    {
        GUILayout.BeginHorizontal(null);
        GUILayout.BeginVertical(null);
        PathImage = GUILayout.TextArea(PathImage, new GUILayoutOption[] { GUILayout.Width(250F) } );
        if(GUILayout.Button("Explorer", null))
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    PathImage = openFileDialog.FileName;
                }
            }
        }
        if (GUILayout.Button("ReadImage", null))
        {
            bitmap = new Bitmap(PathImage);
            ImageOrigin = GetImage(bitmap);
            ImageConvert = new Texture2D(4, 4);
            jsonBlock = string.Empty;
        }
        GUILayout.Label("Wight", null);
        s_wight = GUILayout.TextField(s_wight, null);
        if (GUILayout.Button("Proportion", null))
        {
            float w = bitmap.Width;
            float h = bitmap.Height;
            if(int.TryParse(s_wight, out int w2))
                s_height = (h / w * (float)w2).ToString("0");
        }
        GUILayout.Label("Height", null);
        s_height = GUILayout.TextField(s_height, null);
        if(GUILayout.Button("Proportion",null))
        {
            float w = bitmap.Width;
            float h = bitmap.Height;
            if (int.TryParse(s_height, out int h2))
                s_wight = (w / h * (float)h2).ToString("0");
        }
        if (GUILayout.Button("ConvertImage", null))
        {
            if(int.TryParse(s_wight, out int w) && int.TryParse(s_height, out int h))
            {
                wight = w;
                height = h;
                var tuple = DiggerPixelArt.ColorConvert.ConvertRGB(bitmap, wight, height);
                jsonBlock = tuple.Item2;
                Bitmap bitmapConvert = tuple.Item1;
                ImageConvert = GetImage(DiggerPixelArt.ColorConvert.Resize(bitmapConvert, bitmapConvert.Width * 4, bitmapConvert.Height * 4));
            }
        }
        if (GUILayout.Button("Clear", null))
        {
            bitmap = new Bitmap(1, 1);
            ImageOrigin = new Texture2D(4, 4);
            ImageConvert = new Texture2D(4, 4);
            jsonBlock = string.Empty;
        }
        GUIF.BoxCast.ChildMenu("Settings", () =>
        {
            Type[] types =
            {
                typeof(InterpolationMode),
                typeof(CompositingQuality),
                typeof(SmoothingMode),
                typeof(PixelOffsetMode),
                typeof(TypeDithering)
            };
            for (int i = 0; i < types.Length; i++)
            {
                string[] vs = Enum.GetNames(types[i]);
                GUILayout.Box(types[i].Name,null);
                settingsImageScroll[i] = GUILayout.BeginScrollView(settingsImageScroll[i], new GUILayoutOption[] { GUILayout.Height(50) });
                settingsImage[i] = GUILayout.SelectionGrid(settingsImage[i], vs, vs.Length, null);
                GUILayout.EndScrollView();
            }
        });
        GUILayout.Box("CreaterCode: Unknown_User", null);
        GUILayout.EndVertical();
        Rect rectImage1 = GUILayoutUtility.GetRect(400, 400);
        GUI.Box(rectImage1, ImageOrigin);
        Rect rectImage2 = GUILayoutUtility.GetRect(400, 400);
        GUI.Box(rectImage2, ImageConvert);
        GUILayout.EndHorizontal();
        GUI.DragWindow();
    }

    private Texture2D GetImage(Bitmap bitmap)
    {
        
        Texture2D t = new Texture2D(bitmap.Width, bitmap.Height);
        
        for(int x = 0; x < bitmap.Width; x++)
            for (int y = 0; y < bitmap.Height; y++)
            {
                System.Drawing.Color color = bitmap.GetPixel(x, y);
                UnityEngine.Color colornew = new UnityEngine.Color(color.R / 255F, color.G / 255F, color.B / 255F);
                t.SetPixel(x, bitmap.Height - y, colornew);
            }
        t.Apply();
       return t;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class MenuBlocks : IHackMenu, IStarted, IUpdate
{
    public string Name => "MenuBlocks";
    private float Scroll;
    private Dictionary<BlockType, Texture2D> Textures = new Dictionary<BlockType, Texture2D>();
    
    public Action<int> Menu => (id) =>
    {
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        Scroll = GUILayout.VerticalSlider(Scroll, 0, Types.Length * 50, null);
        Rect area = GUILayoutUtility.GetRect(130, 400);
        GUI.Box(area, "");
        GUI.BeginGroup(area);
        GUI.color = Color.white;
        int block = 0;
        for (int x = 0; x < Types.Length / 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Rect button = new Rect(y * area.width / 3, 50 * x - Scroll, area.width / 3, 50);
                if (Textures.TryGetValue(Types[block], out Texture2D texture))
                {
                    if (GUI.Button(button, texture))
                    {
                        BlockType = Types[block];
                    }
                }
                else if (GUI.Button(button, Types[block].ToString()))
                {
                    BlockType = Types[block];
                }
                block++;
            }
        }
        GUI.color = SettingsMenu.ColorContent;
        GUI.EndGroup();
        area = GUILayoutUtility.GetRect(130, 400);
        GUI.Box(area, "");
        GUI.BeginGroup(area);
        for (int i = 0; i < Kinds.Length; i++)
        {
            if (GUI.Button(new Rect(5, 20 * i - Scroll, area.width - 10, 20), Kinds[i].ToString()))
            {
                BlockKind = Kinds[i];
            }
        }

        GUI.EndGroup();

        GUILayout.EndHorizontal();

        GUI.DragWindow();
    };
    public bool IsActive { get; set; }
    public Rect Rect { get; set; } = new Rect(100, 100, 300, 360);
    public static BlockKind BlockKind { get; private set; } = BlockKind.Default;
    public static BlockType BlockType { get; private set; } = BlockType.Brick;

    private BlockType[] Types;
    private BlockKind[] Kinds;
    private Texture2D[] TextureBlock = new Texture2D[256];
    public void Start()
    {
        Types = new BlockType[256];
        Kinds = new BlockKind[256];
        for (int i = 0; i < 256; i++)
        {
            if(i != 2 && i != 59 && i != 61 && i != 58 && i != 54 && i != 55)
                Types[i] = (BlockType)i;
            Kinds[i] = (BlockKind)i;
        }

    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Textures.Clear();
            PHNNAMINPMK.BJANBGEILJD.GGBIMDHPOMG.ToList().ForEach(block =>
            {
                Texture2D texture = block.textureType == BlockDefenition.GAELLCGLGIJ.One ? block.oneTexture : block.sideTexture;
                Textures.Add((BlockType)block.id, texture);
            });
        }
    }
}

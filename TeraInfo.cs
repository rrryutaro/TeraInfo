using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;

namespace TeraInfo
{
    class TeraInfo : Mod
    {
        internal static TeraInfo instance;

        /// <summary>
        /// FKTModSettingsの有無
        /// </summary>
        internal bool LoadedFKTModSettings = false;
        public static Texture2D waku;


        public TeraInfo()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
        }

        public override void Load()
        {
            instance = this;
            waku = ModContent.GetTexture("TeraInfo/waku");

            On.Terraria.Main.NewText_string_Color_bool += Main_NewText_string_Color_bool;
            On.Terraria.Main.NewText_string_byte_byte_byte_bool += Main_NewText_string_byte_byte_byte_bool;
            On.Terraria.Main.NewText_object_Color_bool += Main_NewText_object_Color_bool;
            On.Terraria.Main.NewText_List1 += Main_NewText_List1;
            On.Terraria.Main.NewTextMultiline += Main_NewTextMultiline;
            On.Terraria.Main.NewTextMultilineOld += Main_NewTextMultilineOld;

        }

        private void Main_NewText_string_Color_bool(On.Terraria.Main.orig_NewText_string_Color_bool orig, string newText, Color color, bool force)
        {
            TeraInfoLogger.Write2($"A{"\t"}{newText}{"\t"}{color.ToString()}");
            orig(newText, color, force);
        }

        private void Main_NewText_string_byte_byte_byte_bool(On.Terraria.Main.orig_NewText_string_byte_byte_byte_bool orig, string newText, byte R, byte G, byte B, bool force)
        {
            TeraInfoLogger.Write2($"B{"\t"}{newText}{"\t"}{new Color(R,G,B).ToString()}");
            orig(newText, R, G, B, force);
        }

        private void Main_NewText_object_Color_bool(On.Terraria.Main.orig_NewText_object_Color_bool orig, object o, Color color, bool force)
        {
            TeraInfoLogger.Write2($"C{"\t"}{o.ToString()}{"\t"}{color.ToString()}");
            System.Diagnostics.Debug.Print(o.ToString());
            orig(o, color, force);
        }

        private void Main_NewTextMultilineOld(On.Terraria.Main.orig_NewTextMultilineOld orig, string text, bool force, Color c)
        {
            TeraInfoLogger.Write2($"D{"\t"}{text}{"\t"}{c.ToString()}");
            orig(text, force, c);
        }

        private void Main_NewTextMultiline(On.Terraria.Main.orig_NewTextMultiline orig, string text, bool force, Color c, int WidthLimit)
        {
            TeraInfoLogger.Write2($"E{"\t"}{text}{"\t"}{c.ToString()}{"\t"}{WidthLimit}");
            orig(text, force, c, WidthLimit);
        }

        private void Main_NewText_List1(On.Terraria.Main.orig_NewText_List1 orig, List<Terraria.UI.Chat.TextSnippet> snippets)
        {
            for (int i = 0; i < snippets.Count; i++)
            {
                Terraria.UI.Chat.TextSnippet x = snippets[i];
                TeraInfoLogger.Write2($"F{i}{"\t"}{x.Text}{"\t"}{x.Color.ToString()}{"\t"}{x.TextOriginal}{"\t"}{x.Scale}");
            }
            orig(snippets);
        }

        public override void PostAddRecipes()
        {
            using (var file = System.IO.File.CreateText(System.IO.Path.Combine(Main.SavePath, "hoge.txt")))
            {
                foreach (Item item in Main.item)
                {
                    file.WriteLine($"{item.modItem?.mod?.Name ?? "Terraria"}:{item.Name}:{item.ToolTip?.ToString() ?? ""}");
                }
            }
        }

        /// <summary>
        /// 各種更新を行う
        /// </summary>
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int layerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Interface Logic 1"));

            //BGM名称表示
            if (Config.isViewBGMName)
            {
                layers.Insert(layerIndex, new LegacyGameInterfaceLayer(
                    "TeraInfo: BGM Name",
                    delegate
                    {
                        string bgmName = InfoMusic.BGMName;
                        float width = Main.fontMouseText.MeasureString(bgmName).X;
                        Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, bgmName, 800 - width / 2, 5, Color.White, Color.Transparent, Vector2.Zero, 1f);
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }

            //日時表
            if (Config.isViewDateTime)
            {
                layers.Insert(layerIndex, new LegacyGameInterfaceLayer(
                    "TeraInfo: DateTime",
                    delegate
                    {
                        TeraInfoTime.CheckTime();
                        //string msg = TeraInfoTime.GetDateTimeAndElapsedDays();
                        string msg = TeraInfoTime.GetTimeAndElapsedDays();
                        float width = Main.fontMouseText.MeasureString(msg).X;
                        Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, msg, Main.screenWidth - width - 10, Main.screenHeight - 20, Color.White, Color.Black, Vector2.Zero, 1f);
                        if (!Main.playerInventory)
                        {
                            msg = TeraInfoTime.GetDate();
                            Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, msg, 2, 2, Color.White, Color.Black, Vector2.Zero, 1f);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
            if (Config.isViewWaku)
            {
                layers.Insert(layerIndex, new LegacyGameInterfaceLayer(
                    "TeraInfo: Waku",
                    delegate
                    {
                        int width = (Main.screenWidth - waku.Width) / 2;
                        int height = Main.screenHeight - waku.Height - 5;
                        Main.spriteBatch.Draw(waku, new Vector2(width, height), Color.White);
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        /// <summary>
        /// BGM更新
        /// </summary>
        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            int curMusic = music;
            if (curMusic == -1)
                curMusic = Main.curMusic;
            InfoMusic.UpdateMusic(curMusic);
        }
    }
}

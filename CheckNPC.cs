using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TeraInfo
{
    class CheckNPC
    {
        public List<int> encountNPC;
        private const string encountNPCSaveName = "CheckNPC_EncountNPC";

        public CheckNPC()
        {
            encountNPC = new List<int>();
        }

        public void Save(TagCompound tag)
        {
            tag.Add(encountNPCSaveName, encountNPC.ToArray());
        }

        public void Load(TagCompound tag)
        {
            encountNPC = tag.GetIntArray(encountNPCSaveName).ToList();
        }

        public bool Contains(NPC npc)
        {
            bool result = encountNPC.Contains(npc.netID);
            return result;
        }

        public void Add(NPC npc)
        {
            encountNPC.Add(npc.netID);

            string msg = $"初遭遇 (#{encountNPC.Count})：{npc.FullName}";

            //アナウンス
            if (Config.isCheckNPCAnnounce)
            {
                Main.NewText(msg);
            }

            //ファイル出力
            AviUtl.EncountNPC(encountNPC.Count, npc);
        }
    }

    class CheckNPCPlayer : ModPlayer
    {
        public override void OnHitAnything(float x, float y, Entity victim)
        {
            var checkNPC = Main.LocalPlayer.GetModPlayer<TeraInfoPlayer>().checkNPC;
            if (victim is NPC)
            {
                var npc = victim as NPC;
                if (!checkNPC.Contains(npc))
                {
                    checkNPC.Add(npc);

                }
            }
        }
        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            var checkNPC = Main.LocalPlayer.GetModPlayer<TeraInfoPlayer>().checkNPC;
            if (!checkNPC.Contains(npc))
            {
                checkNPC.Add(npc);
            }
        }
    }
}

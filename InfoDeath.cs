using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TeraInfo
{
    public class InfoDeath
    {
        public int DeathCount;
        public List<string> DeathDateTime = new List<string>();
        public List<string> DeathName = new List<string>();
        public List<int> DeathLostCoins = new List<int>();

        private const string DEATH_COUNT = "InfoDeath_DeathCount";
        private const string DEATH_DATETIME = "InfoDeath_DateTime";
        private const string DEATH_NAME = "InfoDeath_Name";
        private const string DEATH_LostCOINS = "InfoDeath_LostCoins";

        public void Death(ModPlayer modPlayer, PlayerDeathReason damageSource)
        {
            string name = string.Empty;
            if (0 <= damageSource.SourceNPCIndex)
            {
                name = Main.npc[damageSource.SourceNPCIndex].FullName;
            }
            else if (0 <= damageSource.SourceOtherIndex)
            {
                name = $"OtherIndex({damageSource.SourceOtherIndex})";
            }

            DeathCount++;
            DeathDateTime.Add(TeraInfoTime.GetDateTimeAndElapsedDays());
            DeathName.Add(name);
            DeathLostCoins.Add(modPlayer.player.lostCoins);

            int index = DeathCount - 1;
            AviUtl.PlayerDeath(DeathCount, DeathDateTime[index], DeathName[index], DeathLostCoins[index]);
        }

        public void Save(TagCompound tag)
        {
            tag.Add(DEATH_COUNT, DeathCount);
            tag.Add(DEATH_DATETIME, DeathDateTime);
            tag.Add(DEATH_NAME, DeathName);
            tag.Add(DEATH_LostCOINS, DeathLostCoins);
        }

        public void Load(TagCompound tag)
        {
            try
            {
                string pathImport = $@"{Main.SavePath}\TeraInfo_PlayerDeath.txt";
                if (File.Exists(pathImport))
                {
                    Import(pathImport);
                }
                else
                {
                    if (tag.ContainsKey(DEATH_COUNT))
                    {
                        DeathCount = tag.GetAsInt(DEATH_COUNT);
                    }
                    if (tag.ContainsKey(DEATH_DATETIME))
                    {
                        DeathDateTime = tag.GetList<string>(DEATH_DATETIME).ToList();
                    }
                    if (tag.ContainsKey(DEATH_NAME))
                    {
                        DeathName = tag.GetList<string>(DEATH_NAME).ToList();
                    }
                    if (tag.ContainsKey(DEATH_LostCOINS))
                    {
                        DeathLostCoins = tag.GetList<int>(DEATH_LostCOINS).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                TeraInfoLogger.Write(ex);
            }
        }

        public void Import(string path)
        {
            if (File.Exists(path))
            {
                DeathCount = 0;
                DeathDateTime = new List<string>();
                DeathName = new List<string>();
                DeathLostCoins = new List<int>();

                StreamReader st = new StreamReader(path);

                while (!st.EndOfStream)
                {
                    var line = st.ReadLine().Split(',');
                    DeathDateTime.Add(line[0]);
                    DeathName.Add(line[1]);
                    DeathLostCoins.Add(int.Parse(line[2]));
                    DeathCount++;
                }

                TeraInfoLogger.Write($"{path} を読込みました。");
            }
        }
    }
}

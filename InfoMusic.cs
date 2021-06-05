using System;
using Terraria;

namespace TeraInfo
{
    public static class InfoMusic
    {
        public static string BGMName = "";

        public static string logPath = $@"{Main.SavePath}\Logs\TeraInfo_BGM.txt";

        public static int[] beforeMusicID = new int[] { -1, -1 };
        public static DateTime[] beforeMusicChangeTime = new DateTime[] { DateTime.Now, DateTime.Now };

        private static string[] bgmNames = { "", "OverworldDay", "Eerie", "Night", "Underground", "Boss1", "Title", "Jungle", "Corruption", "TheHallow", "UndergroundCorruption", "UndergroundHallow", "Boss2", "Boss3", "Snow", "Space", "Crimson", "Boss4", "AltOverworldDay", "Rain", "Ice", "Desert", "Ocean", "Dungeon", "Plantera", "Boss5", "Temple", "Eclipse", "RainSoundEffect", "Mushrooms", "PumpkinMoon", "AltUnderground", "FrostMoon", "UndergroundCrimson", "TheTowers", "PirateInvasion", "Hell", "MartianMadness", "LunarBoss", "GoblinInvasion", "Sandstorm", "OldOnesArmy" };

        public static void UpdateMusic(int music)
        {
            if (beforeMusicID[0] != music)
            {
                // OverworldDay と AltOverworldDay が高速で交互に変わる現象の判断
                if (beforeMusicID[1] == music && !TeraInfoTime.CheckElapsedTime(beforeMusicChangeTime[1], DateTime.Now, 1))
                {
                }
                else
                {
                    SetBGMName(music);
                    string msg = $"{"\t"}{TeraInfoTime.GetDateTimeAndElapsedDays()}{"\t"}{BGMName}";
                    TeraInfoLogger.Write(logPath, msg);
                    //Logger.Write2(BGMName);
                }

                beforeMusicID[1] = beforeMusicID[0];
                beforeMusicID[0] = music;
                beforeMusicChangeTime[1] = beforeMusicChangeTime[0];
                beforeMusicChangeTime[0] = DateTime.Now;
            }
        }

        public static void SetBGMName(int musicID)
        {
            BGMName = $"♪ {bgmNames[musicID]}";
        }

    }
}

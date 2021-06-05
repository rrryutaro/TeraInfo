using Terraria.ModLoader;

namespace TeraInfo
{
    public static class Config
    {
        public static bool isViewDateTime { get { return ModContent.GetInstance<TeraInfoConfig>().isViewDateTime; } }
        public static bool isCheckItemAnnounce { get { return ModContent.GetInstance<TeraInfoConfig>().isCheckItemAnnounce; } }
        public static bool isCheckNPCAnnounce { get { return ModContent.GetInstance<TeraInfoConfig>().isCheckNPCAnnounce; } }
        public static bool isAnglerQuestAnnounce { get { return ModContent.GetInstance<TeraInfoConfig>().isAnglerQuestAnnounce; } }
        public static bool isViewWaku { get { return ModContent.GetInstance<TeraInfoConfig>().isViewWaku; } }
        public static bool isViewBGMName { get { return ModContent.GetInstance<TeraInfoConfig>().isViewBGMName; } }
    }
}

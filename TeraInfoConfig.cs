using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TeraInfo
{
    [Label("Config")]
    public class TeraInfoConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        /// <summary>
        /// 日時表示
        /// </summary>
        [Label("日時表示")]
        [DefaultValue(true)]
        public bool isViewDateTime = true;
        /// <summary>
        /// アイテム取得アナウンス
        /// </summary>
        [Label("新規取得アナウンス")]
        [DefaultValue(true)]
        public bool isCheckItemAnnounce = true;
        /// <summary>
        /// 初遭遇アナウンス
        /// </summary>
        [Label("新規遭遇アナウンス")]
        [DefaultValue(true)]
        public bool isCheckNPCAnnounce = true;
        /// <summary>
        /// 釣りクエストアナウンス
        /// </summary>
        [Label("釣りクエストアナウンス")]
        [DefaultValue(true)]
        public bool isAnglerQuestAnnounce = true;
        /// <summary>
        /// 実況用台詞枠表示
        /// </summary>
        [Label("台詞枠表示")]
        [DefaultValue(false)]
        public bool isViewWaku = false;
        /// <summary>
        /// BGM名表示
        /// </summary>
        [Label("BGM名表示")]
        [DefaultValue(true)]
        public bool isViewBGMName = true;
    }
}

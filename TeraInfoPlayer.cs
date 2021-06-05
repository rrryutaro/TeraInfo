using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;

namespace TeraInfo
{
    class TeraInfoPlayer : ModPlayer
    {
        public CheckItem checkItem;
        public CheckNPC checkNPC;
        public InfoEquip infoEquip;
        public InfoDeath infoDeath;

        public override void OnEnterWorld(Player player)
        {
            TeraInfoTime.Initialize();
        }

        public override void Initialize()
        {
            if (!Main.dedServ)
            {
                checkItem = new CheckItem();
                checkNPC = new CheckNPC();
                infoEquip = new InfoEquip();
                infoDeath = new InfoDeath();
            }
        }

        public override TagCompound Save()
        {
            TagCompound result = new TagCompound();
            checkItem.Save(result);
            checkNPC.Save(result);
            infoDeath.Save(result);
            return result;
        }

        public override void Load(TagCompound tag)
        {
            checkItem.Load(tag);
            checkNPC.Load(tag);
            infoDeath.Load(tag);
        }

        /// <summary>
        /// プレイヤー死亡時
        /// </summary>
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            infoDeath.Death(this, damageSource);
        }

        public override void PreUpdate()
        {
            if (!Main.dedServ)
            {
                ///インベントリチェック
                for (int i = 0; i < 59; i++)
                {
                    if (!player.inventory[i].IsAir)
                    {
                        checkItem.ItemReceived(player.inventory[i]);
                    }
                }
                infoEquip.PlayerPreUpdate(this);
            }
        }

        /// <summary>
        /// 釣りクエスの報酬取得時
        /// </summary>
        public override void AnglerQuestReward(float rareMultiplier, List<Item> rewardItems)
        {
            AviUtl.AnglerQuestReward(rewardItems);
        }
    }
}

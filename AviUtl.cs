using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Terraria;

namespace TeraInfo
{
    /// <summary>
    /// AviUtl exoファイル出力のためのインプットデータ出力用のクラス
    /// </summary>
    public static class AviUtl
    {
        public static string logPathCheckItem = $@"{Main.SavePath}\Logs\TeraInfo_CheckItem.txt";
        public static string logPathEncountNPC = $@"{Main.SavePath}\Logs\TeraInfo_EncountNPC.txt";
        public static string logPathAnglerQuestReward = $@"{Main.SavePath}\Logs\TeraInfo_AnglerQuest.txt";
        public static string logPathPlayerEquip = $@"{Main.SavePath}\Logs\TeraInfo_PlayerEquip.txt";
        public static string logPathPlayerDeath = $@"{Main.SavePath}\Logs\TeraInfo_PlayerDeath.txt";

        /// <summary>
        /// アイテム取得時の情報を出力する
        /// </summary>
        /// <param name="item">取得アイテム</param>
        /// <param name="recipe">レシピ</param>
        /// <param name="acquired">既得</param>
        public static void CheckItem(int getNo, Item item, int craftNo = 0, Recipe recipe = null, bool acquired = false)
        {
            AviUtlCheckItem info = new AviUtlCheckItem(getNo, item, craftNo, recipe, acquired);
            string msg = $"アイテム取得{"\t"}{JsonConvert.SerializeObject(info)}";

            TeraInfoLogger.Write2(logPathCheckItem, msg);
        }

        public static void EncountNPC(int no, NPC npc)
        {
            AciUtlEncountNPC info = new AciUtlEncountNPC(no, npc);
            string msg = $"初遭遇{"\t"}{JsonConvert.SerializeObject(info)}";

            TeraInfoLogger.Write2(logPathEncountNPC, msg);
        }

        /// <summary>
        /// 釣りクエスト報告時の情報を出力する
        /// </summary>
        /// <param name="rewardItems"></param>
        public static void AnglerQuestReward(List<Item> rewardItems)
        {
            AviUtlAnglerQuestInfo info = new AviUtlAnglerQuestInfo(rewardItems);
            string msg = $"釣りクエスト報告{"\t"}{JsonConvert.SerializeObject(info)}";

            if (Config.isAnglerQuestAnnounce)
            {
                Main.NewText($"釣りクエスト #{info.QuestNo} 完了");
            }
            TeraInfoLogger.Write2(logPathAnglerQuestReward, msg);
        }

        /// <summary>
        /// 装備変更時の情報を出力する
        /// </summary>
        public static void PlayerEquipArmor(int index, Item item)
        {
            PlayerEquipInfoArmor info = new PlayerEquipInfoArmor(index, item);
            string msg = $"装備変更{"\t"}{JsonConvert.SerializeObject(info)}";

            TeraInfoLogger.Write2(logPathPlayerEquip, msg);
        }

        /// <summary>
        /// 染料変更時の情報を出力する
        /// </summary>
        public static void PlayerEquipDye(int index, Item item)
        {
            PlayerEquipInfoDye info = new PlayerEquipInfoDye(index, item);
            string msg = $"染料変更{"\t"}{JsonConvert.SerializeObject(info)}";

            TeraInfoLogger.Write2(logPathPlayerEquip, msg);
        }

        /// <summary>
        /// 防御力変更時の情報を出力する
        /// </summary>
        /// <param name="defenct"></param>
        public static void PlayerEquipDefence(int defenct)
        {
            PlayerEquipInfoDefence info = new PlayerEquipInfoDefence(defenct);
            string msg = $"防御力変更{"\t"}{JsonConvert.SerializeObject(info)}";

            TeraInfoLogger.Write2(logPathPlayerEquip, msg);
        }

        public static void PlayerDeath(int count, string time, string name, int lostCoins)
        {
            PlayerDeathInfo info = new PlayerDeathInfo(count, time, name, lostCoins);
            string msg = $"プレイヤー死亡{"\t"}{JsonConvert.SerializeObject(info)}";

            TeraInfoLogger.Write2(logPathPlayerDeath, msg);
        }
    }

    public class AviUtlItem
    {
        public int NetID;
        public int Stack;
        public AviUtlItem(Item item)
        {
            NetID = item.netID;
            Stack = item.stack;
        }
    }

    /// <summary>
    /// アイテム取得情報
    /// </summary>
    public class AviUtlCheckItem
    {
        public bool Acquired;
        public int GetNo;
        public int CraftNo;
        public int ItemID;
        public string Name;
        public string NameKey;

        public CheckItemRecipe Recipe;
        public class CheckItemRecipe
        {
            public AviUtlItem CraftItem;
            public int[] RequiredTile;
            public List<AviUtlItem> RequiredItems;
            public CheckItemRecipe(Recipe recipe)
            {
                CraftItem = new AviUtlItem(recipe.createItem);
                RequiredTile = recipe.requiredTile.Where(x => 0 <= x).ToArray();
                RequiredItems = new List<AviUtlItem>(recipe.requiredItem.Where(x => 0 < x.netID).Select(x => new AviUtlItem(x)));
            }
        }

        public AviUtlCheckItem(int getNo, Item item, int craftNo, Recipe recipe, bool acquired)
        {
            Acquired = acquired;
            GetNo = getNo;
            ItemID = item.netID;
            Name = item.Name;
            NameKey = Lang.GetItemName(item.netID).Key;

            if (recipe != null)
            {
                CraftNo = craftNo;
                Recipe = new CheckItemRecipe(recipe);
            }
        }
    }

    /// <summary>
    /// 初遭遇情報
    /// </summary>
    public class AciUtlEncountNPC
    {
        public int No;
        public int NetID;
        public string Name;
        public string NameKey;

        public int Life;
        public int Damage;
        public int Defense;
        public float KnockBackResist;

        public AciUtlEncountNPC(int no, NPC npc)
        {
            No = no;
            NetID = npc.netID;
            Name = npc.FullName;
            NameKey = Lang.GetNPCName(npc.netID).Key;

            Life = npc.lifeMax;
            Damage = npc.defDamage;
            Defense = npc.defDefense;
            KnockBackResist = npc.knockBackResist;
        }
    }

    /// <summary>
    /// 釣りクエスト報酬の情報
    /// </summary>
    public class AviUtlAnglerQuestInfo
    {
        public int QuestNo;
        public int QuestItem;
        public List<AviUtlItem> RewardItems = new List<AviUtlItem>();

        public AviUtlAnglerQuestInfo(List<Item> rewardItems)
        {
            QuestNo = Main.LocalPlayer.anglerQuestsFinished;
            QuestItem = Main.anglerQuestItemNetIDs[Main.anglerQuest];
            RewardItems.AddRange(rewardItems.Select(x => new AviUtlItem(x)));
        }
    }

    /// <summary>
    /// 装備情報
    /// </summary>
    public class PlayerEquipInfoArmor
    {
        public int Index;
        public int NetID;
        public string Name;
        public string NameKey;

        public PlayerEquipInfoArmor(int index, Item item)
        {
            Index = index;
            NetID = item.netID;
            Name = item.Name;
            NameKey = Lang.GetItemName(item.netID).Key;
        }
    }

    /// <summary>
    /// 染料情報
    /// </summary>
    public class PlayerEquipInfoDye
    {
        public int Index;
        public int NetID;
        public string Name;
        public string NameKey;

        public PlayerEquipInfoDye(int index, Item item)
        {
            Index = index;
            NetID = item.netID;
            Name = item.Name;
            NameKey = Lang.GetItemName(item.netID).Key;
        }
    }

    /// <summary>
    /// 防御力情報
    /// </summary>
    public class PlayerEquipInfoDefence
    {
        public int Defence;

        public PlayerEquipInfoDefence(int defence)
        {
            Defence = defence;
        }
    }

    /// <summary>
    /// 死亡情報
    /// </summary>
    public class PlayerDeathInfo
    {
        public int Count;
        public string Name;
        public string Time;
        public int LostCoins;

        public PlayerDeathInfo(int count, string time, string name, int lostCoins)
        {
            Count = count;
            Time = time;
            Name = name;
            LostCoins = lostCoins;
        }
    }
}

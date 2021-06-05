using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.UI;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TeraInfo
{
    class CheckItem
    {
        public Dictionary<int, int> getItemNo;

        public List<Item> getItems;
        public List<Item> craftItems;
        private const string getItemsSaveName = "CheckItem_GetItems";
        private const string craftItemsSaveName = "CheckItem_CraftItems";

        //
        public bool[] foundItem;
        public bool[] foundCraftItem;
        public bool[] findableItems;

        public int totalItemsToFind;
        public int totalItemsFound;
        public int totalCraftItemsFound;

        public CheckItem()
        {
            getItemNo = new Dictionary<int, int>();
            getItems = new List<Item>();
            craftItems = new List<Item>();
            foundItem = new bool[ItemLoader.ItemCount];
            foundCraftItem = new bool[ItemLoader.ItemCount];
            findableItems = new bool[ItemLoader.ItemCount];

            totalItemsFound = 0;
            totalCraftItemsFound = 0;

            for (int i = 0; i < ItemLoader.ItemCount; i++)
            {
                if (0 < i && !ItemID.Sets.Deprecated[i] && i != ItemID.Count)
                {
                    totalItemsToFind++;
                    findableItems[i] = true;
                }
            }
        }

        public void Save(TagCompound tag)
        {
            tag.Add(getItemsSaveName, getItems.Select(ItemIO.Save).ToList());
            tag.Add(craftItemsSaveName, craftItems.Select(ItemIO.Save).ToList());
        }

        public void Load(TagCompound tag)
        {
            try
            {
                string pathGetItem = $@"{Main.SavePath}\TeraInfo_GetItem.txt";
                string pathCraft = $@"{Main.SavePath}\\TeraInfo_Craft.txt";
                if (File.Exists(pathGetItem) || File.Exists(pathCraft))
                {
                    Import(pathGetItem, pathCraft);
                }
                else
                {
                    if (tag.ContainsKey(getItemsSaveName))
                    {
                        getItems = tag.GetList<TagCompound>(getItemsSaveName).Select(ItemIO.Load).ToList();

                        int no = 1;
                        foreach (var item in getItems)
                        {
                            try
                            {
                                if (!getItemNo.ContainsKey(item.type))
                                    getItemNo.Add(item.type, no++);

                                if (item.Name != "Unloaded Item")
                                {
                                    foundItem[item.type] = true;
                                    totalItemsFound++;
                                }
                            }
                            catch (Exception ex)
                            {
                                TeraInfoLogger.Write(ex);
                            }
                        }
                    }
                    if (tag.ContainsKey(craftItemsSaveName))
                    {
                        craftItems = tag.GetList<TagCompound>(craftItemsSaveName).Select(ItemIO.Load).ToList();
                        foreach (var item in craftItems)
                        {
                            if (item.Name != "Unloaded Item")
                            {
                                foundCraftItem[item.type] = true;
                                totalCraftItemsFound++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TeraInfoLogger.Write(ex);
            }
        }

        public void Import(string pathGetItem, string pathCraft)
        {
            //取得アイテムの取り込み
            if (File.Exists(pathGetItem))
            {
                getItemNo = new Dictionary<int, int>();
                getItems = new List<Item>();
                foundItem = new bool[ItemLoader.ItemCount];
                totalItemsFound = 0;

                StreamReader st = new StreamReader(pathGetItem);
                while (!st.EndOfStream)
                {
                    Item newItem = new Item();
                    newItem.SetDefaults(int.Parse(st.ReadLine()));
                    getItems.Add(newItem);
                    foundItem[newItem.type] = true;
                    totalItemsFound++;
                    getItemNo.Add(newItem.type, totalItemsFound);
                }
                TeraInfoLogger.Write($"{pathGetItem} を読込みました。");
            }

            //クラフトアイテムの取り込み
            if (File.Exists(pathCraft))
            {
                craftItems = new List<Item>();
                foundCraftItem = new bool[ItemLoader.ItemCount];
                totalCraftItemsFound = 0;

                StreamReader st = new StreamReader(pathCraft);
                while (!st.EndOfStream)
                {
                    Item newItem = new Item();
                    newItem.SetDefaults(int.Parse(st.ReadLine()));
                    craftItems.Add(newItem);
                    foundCraftItem[newItem.type] = true;
                    totalCraftItemsFound++;
                }
                TeraInfoLogger.Write($"{pathCraft} を読込みました。");
            }
        }

        public void ItemReceived(Item item, Recipe recipe = null)
        {
            if (findableItems[item.type] && (!foundItem[item.type] || (recipe != null && !foundCraftItem[item.type])))
            {
                string msg;

                Item newItem = new Item();
                newItem.SetDefaults(item.type);

                //既得か
                bool acquired = false;

                if (!foundItem[item.type])
                {
                    getItems.Add(newItem);
                    totalItemsFound++;
                    foundItem[item.type] = true;
                    msg = $"新規取得 (#{totalItemsFound})：{item.Name} ({(100f * totalItemsFound / totalItemsToFind).ToString("0.00")}%)";

                    getItemNo.Add(newItem.type, totalItemsFound);
                }
                else
                {
                    msg = $"取得済み (#{getItemNo[newItem.type]})：{item.Name}";
                    acquired = true;
                }

                if (recipe != null)
                {
                    craftItems.Add(newItem);
                    totalCraftItemsFound++;
                    foundCraftItem[item.type] = true;
                    msg += $" 新規クラフト (#{totalCraftItemsFound})";
                }

                //アナウンス
                if (Config.isCheckItemAnnounce)
                {
                    Main.NewText(msg);
                }

                //AviUtl exo出力用のインプットファイル出力
                if (recipe == null)
                {
                    AviUtl.CheckItem(totalItemsFound, item);
                }
                else
                {
                    AviUtl.CheckItem(totalItemsFound, item, totalCraftItemsFound, recipe, acquired);
                }
            }
        }
    }

    class TeraInfoGlobalItem : GlobalItem
    {
        public override void OnCraft(Item item, Recipe recipe)
        {
            Main.LocalPlayer.GetModPlayer<TeraInfoPlayer>().checkItem.ItemReceived(item, recipe);
        }

        public override bool OnPickup(Item item, Player player)
        {
            player.GetModPlayer<TeraInfoPlayer>().checkItem.ItemReceived(item);
            return true;
        }

        /// <summary>
        /// アイテムの価格をツールチップの最後に表示するコード。
        /// 未使用だが、参考のため残しておく
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private TooltipLine GetPriceTooltipLine(Item item)
        {
            TooltipLine result = new TooltipLine(TeraInfo.instance, "Price", string.Empty);

            string[] array = new string[20];
            int num4 = 0;
            int storeValue = item.GetStoreValue();
            int a = (int)Main.mouseTextColor;
            float num20 = (float)Main.mouseTextColor / 255f;

            if (item.shopSpecialCurrency != -1)
            {
                result = new TooltipLine(TeraInfo.instance, "SpecialPrice", string.Empty);
                CustomCurrencyManager.GetPriceText(item.shopSpecialCurrency, array, ref num4, storeValue);
                result.text = array[num4];
                result.overrideColor = new Color((int)((byte)(255f * num20)), (int)((byte)(255f * num20)), (int)((byte)(255f * num20)), a);
            }
            else if (storeValue > 0)
            {
                string text = "";
                int num21 = 0;
                int num22 = 0;
                int num23 = 0;
                int num24 = 0;
                int num25 = storeValue * item.stack;
                if (!item.buy)
                {
                    num25 = storeValue / 5;
                    if (num25 < 1)
                    {
                        num25 = 1;
                    }
                    num25 *= item.stack;
                }
                if (num25 < 1)
                {
                    num25 = 1;
                }
                if (num25 >= 1000000)
                {
                    num21 = num25 / 1000000;
                    num25 -= num21 * 1000000;
                }
                if (num25 >= 10000)
                {
                    num22 = num25 / 10000;
                    num25 -= num22 * 10000;
                }
                if (num25 >= 100)
                {
                    num23 = num25 / 100;
                    num25 -= num23 * 100;
                }
                if (num25 >= 1)
                {
                    num24 = num25;
                }
                if (num21 > 0)
                {
                    object obj = text;
                    text = string.Concat(new object[] { obj, num21, " ", Lang.inter[15].Value, " " });
                }
                if (num22 > 0)
                {
                    object obj = text;
                    text = string.Concat(new object[] { obj, num22, " ", Lang.inter[16].Value, " " });
                }
                if (num23 > 0)
                {
                    object obj = text;
                    text = string.Concat(new object[] { obj, num23, " ", Lang.inter[17].Value, " " });
                }
                if (num24 > 0)
                {
                    object obj = text;
                    text = string.Concat(new object[] { obj, num24, " ", Lang.inter[18].Value, " " });
                }
                if (!item.buy)
                {
                    result.text = Lang.tip[49].Value + " " + text;
                }
                else
                {
                    result.text = Lang.tip[50].Value + " " + text;
                }
                if (num21 > 0)
                {
                    result.overrideColor = new Color((int)((byte)(220f * num20)), (int)((byte)(220f * num20)), (int)((byte)(198f * num20)), a);
                }
                else if (num22 > 0)
                {
                    result.overrideColor = new Color((int)((byte)(224f * num20)), (int)((byte)(201f * num20)), (int)((byte)(92f * num20)), a);
                }
                else if (num23 > 0)
                {
                    result.overrideColor = new Color((int)((byte)(181f * num20)), (int)((byte)(192f * num20)), (int)((byte)(193f * num20)), a);
                }
                else if (num24 > 0)
                {
                    result.overrideColor = new Color((int)((byte)(246f * num20)), (int)((byte)(138f * num20)), (int)((byte)(96f * num20)), a);
                }
            }
            else if (Main.HoverItem.type != 3817)
            {
                result.text = Lang.tip[51].Value;
                result.overrideColor = new Color((int)((byte)(120f * num20)), (int)((byte)(120f * num20)), (int)((byte)(120f * num20)), a);
            }
            return result;
        }
    }
}

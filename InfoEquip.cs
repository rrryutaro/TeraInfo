using Terraria.ModLoader;

namespace TeraInfo
{
    public class InfoEquip
    {
        public int[] Armor = new int[20];
        public int[] Dye = new int[10];
        public int Defense;

        private bool isInitialize = false;

        public void Initialize(ModPlayer modPlayer)
        {
            for (int i = 0; i < 20; i++)
            {
                Armor[i] = modPlayer.player.armor[i].netID;
            }
            for (int i = 0; i < 10; i++)
            {
                Dye[i] = modPlayer.player.dye[i].netID;
            }
            Defense = modPlayer.player.statDefense;
        }

        public void PlayerPreUpdate(ModPlayer modPlayer)
        {
            if (isInitialize)
            {
                Initialize(modPlayer);
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    if (modPlayer.player.armor[i].netID != Armor[i])
                    {
                        Armor[i] = modPlayer.player.armor[i].netID;
                        AviUtl.PlayerEquipArmor(i, modPlayer.player.armor[i]);
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    if (modPlayer.player.dye[i].netID != Dye[i])
                    {
                        Dye[i] = modPlayer.player.dye[i].netID;
                        AviUtl.PlayerEquipDye(i, modPlayer.player.dye[i]);
                    }
                }
                if (modPlayer.player.statDefense != Defense)
                {
                    Defense = modPlayer.player.statDefense;
                    AviUtl.PlayerEquipDefence(modPlayer.player.statDefense);
                }
            }
        }
    }
}

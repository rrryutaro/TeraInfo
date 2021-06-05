using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TeraInfo
{
    public class TeraInfoWorld : ModWorld
    {
        public static int elapsedDays;

        public override void Initialize()
        {
            elapsedDays = 1;
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                ["ElapsedDays"] = elapsedDays,
            };
        }

        public override void Load(TagCompound tag)
        {
            if (tag.ContainsKey("ElapsedDays"))
            {
                elapsedDays = tag.GetAsInt("ElapsedDays");
            }
        }
    }

}

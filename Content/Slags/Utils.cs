using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;

namespace OreProcessing.Content.Slags
{
    public static class Utils
    {
        public static readonly List<int> Ores_phm = new() { ItemID.CopperOre, ItemID.TinOre, ItemID.IronOre, ItemID.LeadOre, ItemID.SilverOre, ItemID.TungstenOre, ItemID.GoldOre, ItemID.PlatinumOre };
        public static readonly List<int> Ores_evil = new() { ItemID.DemoniteOre, ItemID.CrimtaneOre, ItemID.Hellstone, ItemID.Obsidian };
        public static readonly List<int> Ores_hm = new() { ItemID.CobaltOre, ItemID.PalladiumOre, ItemID.MythrilOre, ItemID.OrichalcumOre, ItemID.AdamantiteOre, ItemID.TitaniumOre };
        public static readonly List<int> Gems = new() { ItemID.Amethyst, ItemID.Topaz, ItemID.Sapphire, ItemID.Emerald, ItemID.Ruby, ItemID.Diamond };
        public static readonly List<int> Souls = new() { ItemID.SoulofLight, ItemID.SoulofNight, ItemID.SoulofFlight };
        public static int RandomEntry(this List<int> list) => list[Terraria.Main.rand.Next(list.Count)];
        public static List<int> WithEntries(this List<int> l1, List<int> l2)
        {
            var list = l1.ToList();
            list.AddRange(l2);
            return list;
        }
    }
    
}
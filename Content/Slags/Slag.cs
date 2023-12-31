﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OreProcessing.Content.Slags
{
    public class SlagItem : ModItem
    {
        /*
         * 1.4.4.8 code for dynamically loading from
         * https://github.com/tModLoader/tModLoader/blob/1.4.4/ExampleMod/Content/Items/Placeable/ExampleTrap.cs
         * **/
        public struct SlagData
        {
            public Color tint;
            public string name;
            public List<int> oreDrops;
            public List<int> miscDrops;
            public float cointMult;
        }
        public class SlagItemLoader : ILoadable
        {
            internal static SlagData[] slagDatas = new SlagData[]
            {
                new SlagData{tint = Color.White, name = "Slag", 
                    oreDrops = Utils.Ores_phm, 
                    miscDrops = Utils.Gems, 
                    cointMult = 1f},
                new SlagData{tint = Color.RosyBrown, name = "RichSlag", 
                    oreDrops = Utils.Ores_phm.WithEntries(Utils.Ores_evil), 
                    miscDrops = Utils.Gems, 
                    cointMult = 1.2f},
                new SlagData{tint = Color.Goldenrod, name = "InfusedSlag", 
                    oreDrops = Utils.Ores_hm, 
                    miscDrops = Utils.Gems.WithEntries(Utils.Souls), 
                    cointMult = 1.5f},
                new SlagData{tint = Color.LightCyan, name = "LunarSlag", 
                    oreDrops = new List<int> { ItemID.LunarOre, ItemID.ChlorophyteOre, ItemID.ChlorophyteOre }, 
                    miscDrops = new List<int> { ItemID.FragmentVortex, ItemID.FragmentNebula, ItemID.FragmentSolar, ItemID.FragmentStardust },
                    cointMult = 2f
                }
            };
            public void Load(Mod mod)
            {
                foreach (SlagData data in slagDatas)
                {
                    SlagItem slagItem = new(data);
                    mod.AddContent(slagItem);
                }
            }

            public void Unload()
            {
            }
        }
        public SlagItem(SlagData data)
        {
            this.data = data;
        }
        public static SlagItem GetSlagTier(int tier)
        {
            if (tier < 0 || tier >= SlagItemLoader.slagDatas.Length) { throw new System.Exception("Invalid style"); }
            return new SlagItem(SlagItemLoader.slagDatas[tier]);
        }
        public static int GetSlagTierID(int tier) => ModContent.GetInstance<OreProcessing>().Find<ModItem>(GetSlagTier(tier).Name).Type;
        internal SlagData data;
        protected override bool CloneNewInstances => true;
        public override string Texture => "OreProcessing/Content/Slags/Slag";
        public override string Name => data.name;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(TileID.Silt);
            Item.createTile = -1;
            Item.placeStyle = 0;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(OreProcessing.SlagTex.Value, position, null, data.tint, 0f, origin, scale, SpriteEffects.None, 0);
            return false;
        }
        public override void ExtractinatorUse(int extractinatorBlockType, ref int resultType, ref int resultStack)
        {
            int addend = (extractinatorBlockType == TileID.ChlorophyteExtractinator) ? 1 : 0;
            resultStack = 1;
            if (Main.rand.NextBool(3))
            {
                resultType = data.oreDrops.RandomEntry();
                for (int i = 0; i < 8 + addend * 2; i++)
                    if (Main.rand.NextBool(3)) resultStack++;
                return;
            }
            if (Main.rand.NextBool(4))
            {
                resultType = data.miscDrops.RandomEntry();
                for (int i = 0; i < 5; i++)
                    if (Main.rand.NextBool(30 - 3 * addend)) resultStack++;
                return;
            }
            resultType = ItemID.CopperCoin;
            for (int n = 2 + addend, p = 2; n < 100; n *= 2, p++)
                if (Main.rand.NextBool(p)) resultStack += n;
            resultStack = (int)(resultStack * data.cointMult);
            if (resultStack > 99)
            {
                resultStack /= 100;
                resultType = ItemID.SilverCoin;
            }
        }
    }
}
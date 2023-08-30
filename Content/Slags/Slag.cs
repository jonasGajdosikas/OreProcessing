using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OreProcessing.Content.Slags
{
    [Autoload(false)]
    public class SlagItem : ModItem
    {
        public SlagItem(SlagData data)
        {
            this.data = data;
        }
        public SlagItem()
        {

        }
        public void SetData(SlagData data) => this.data = data;
        SlagData data;
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
    public struct SlagData
    {
        public Color tint;
        public string name;
        public List<int> oreDrops;
        public List<int> miscDrops;
        public float cointMult;
    }
    public class SlagSystem : ModSystem
    {
        internal static SlagData[] slagDatas = new SlagData[]
        {
            new SlagData{tint = Color.White, name = "Slag", oreDrops = Utils.Ores_phm, miscDrops = Utils.Gems, cointMult = 1f},
            new SlagData{tint = Color.RosyBrown, name = "RichSlag", oreDrops = Utils.Ores_phm.WithEntries(Utils.Ores_evil), miscDrops = Utils.Gems, cointMult = 1.2f},
            new SlagData{tint = Color.Goldenrod, name = "InfusedSlag", oreDrops = Utils.Ores_hm, miscDrops = Utils.Gems.WithEntries(Utils.Souls), cointMult = 1.5f}
        };
        public static List<int> slagIDs = new();
        public static void LoadSlags(OreProcessing mod)
        {
            foreach (SlagData data in slagDatas)
            {
                SlagItem slagItem = new();
                slagItem.SetData(data);
                mod.AddContent(slagItem);
                slagIDs.Add(slagItem.Item.type);
            }
        }
    }
}
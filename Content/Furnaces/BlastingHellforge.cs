using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace OreProcessing.Content.Furnaces
{
    public class BlastingHellforge : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 18 };
            TileObjectData.addTile(Type);

            AdjTiles = new int[] { TileID.Hellforge, TileID.Furnaces };

            AddMapEntry(new Color(238, 85, 70), ModContent.GetInstance<BlastingHellforgeItem>().DisplayName);
            AnimationFrameHeight = 72;
        }
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            if (++frameCounter >= 5)
            {
                frameCounter = 0;
                frame = ++frame % 12;
            }
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.93f;
            g = 0.06f;
            b = 0.05f;
        }
    }
    public class BlastingHellforgeItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Hellforge);
            Item.createTile = ModContent.TileType<BlastingHellforge>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<BlastFurnaceItem>()
                .AddIngredient(ItemID.HellstoneBar, 20)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }
}

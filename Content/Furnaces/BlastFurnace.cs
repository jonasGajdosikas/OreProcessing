using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace OreProcessing.Content.Furnaces
{
    public class BlastFurnace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Origin = new Point16(2, 1);
            TileObjectData.addTile(Type);

            AdjTiles = new int[] { TileID.Furnaces };

            AddMapEntry(new Color(54, 49, 47));
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
            r = 0.83f;
            g = 0.09f;
            b = 0.08f;
        }
    }
    public class BlastFurnaceItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Furnace);
            Item.createTile = ModContent.TileType<BlastFurnace>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Furnace)
                .AddIngredient(ItemID.StoneBlock, 20)
                .AddIngredient(ItemID.Torch, 3)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
    public class BlastSmelting : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];
                if (recipe.requiredTile.Contains(TileID.Furnaces) &&
                    (recipe.createItem.createTile == TileID.MetalBars))
                {
                    recipe.AddOnCraftCallback(GiveSlag);
                }
            }
        }
        public static void GiveSlag(Recipe recipe, Item item, List<Item> consumedItems, Item destinationStack)
        {
            Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), Slags.SlagItem.GetSlagTier(0).Type);
        }
    }
}

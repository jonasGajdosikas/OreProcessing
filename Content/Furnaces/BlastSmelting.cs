using OreProcessing.Content.Slags;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OreProcessing.Content.Furnaces
{
    public class BlastSmelting : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];
                if (RecipeIsBars(recipe))
                {
                    recipe.AddConsumeItemCallback(Blast);
                    recipe.AddOnCraftCallback(BlastSlag);
                }
            }
        }

        static bool RecipeIsBars(Recipe recipe)
        {
            bool createsBars = recipe.createItem.createTile == TileID.MetalBars;
            bool fromOre = recipe.requiredItem.TrueForAll(item => item.createTile != TileID.MetalBars);

            return createsBars && fromOre;
        }
        static int RecipeBarTier(Recipe recipe)
        {
            //if (recipe.requiredTile.Contains(TileID.LunarCraftingStation)) return 4;
            if (recipe.requiredTile.Contains(TileID.AdamantiteForge)) return 3;
            if (recipe.requiredTile.Contains(TileID.Hellforge)) return 2;
            if (recipe.requiredTile.Contains(TileID.Furnaces)) return 1;
            return 4;
        }
        static int Consumed = 0;
        private static void Blast(Recipe recipe, int type, ref int amount)
        {
            int tDiff = Main.LocalPlayer.GetModPlayer<BlastPlayer>().BlastTier - RecipeBarTier(recipe);
            if (tDiff >= 0)
            {
                for (int i = amount; i > 0; i--)
                {
                    if (Main.rand.NextBool(5 - tDiff)) amount--;
                    else Consumed++;
                }
            }
        }
        public static void BlastSlag(Recipe recipe, Item item, List<Item> consumedItems, Item destinationStack)
        {
            int tier = Main.LocalPlayer.GetModPlayer<BlastPlayer>().BlastTier;
            
            int tRecipe = RecipeBarTier(recipe);
            if (tRecipe > tier) return;
            if (Consumed < 10) return;
            if (Main.rand.NextBool(Consumed / (10 + tRecipe - tier))) return;
            Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), SlagItem.GetSlagTierID(tier - 1));
            Consumed -= 10;
        }
    }
    public class BlastPlayer : ModPlayer
    {
        public int BlastTier
        {
            get {
                if (Player.adjTile[ModContent.TileType<ShimmeringExtractinator>()]) return 4;
                if (Player.adjTile[ModContent.TileType<BlastingHMForge>()]) return 3;
                if (Player.adjTile[ModContent.TileType<BlastingHellforge>()]) return 2;
                if (Player.adjTile[ModContent.TileType<BlastFurnace>()]) return 1;
                return 0;
            }
        }
    }
}

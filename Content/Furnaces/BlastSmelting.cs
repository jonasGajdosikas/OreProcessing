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
            bool fromOre = recipe.requiredItem[0].Name.EndsWith("Ore");
            return createsBars && fromOre;
        }
        static int RecipeBarTier(Recipe recipe)
        {
            if (recipe.requiredTile.Contains(TileID.Furnaces)) return 1;
            if (recipe.requiredTile.Contains(TileID.Hellforge)) return 2;
            if (recipe.requiredTile.Contains(TileID.AdamantiteForge)) return 3;
            if (recipe.requiredTile.Contains(TileID.LunarCraftingStation)) return 4;
            return 0;
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
            int tDiff = tier - RecipeBarTier(recipe);
            if (tDiff < 0) return;
            if (Consumed < 10) return;
            if (Main.rand.NextBool(Consumed / 10 + tDiff)) return;
            Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), Slags.SlagItem.GetSlagTierID(tier - 1));
            Consumed -= 10;
        }
    }
    public class BlastPlayer : ModPlayer
    {
        public int BlastTier
        {
            get
            {
                if (Player.adjTile[ModContent.TileType<BlastingHellforge>()]) return 2;
                if (Player.adjTile[ModContent.TileType<BlastFurnace>()]) return 1;
                return 0;
            }
        }
    }
}

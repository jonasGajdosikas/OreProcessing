using OreProcessing.Content.Slags;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OreProcessing.Content.Furnaces
{
    public class BlastSmelting : ModSystem
    {
        public override void PostAddRecipes()
        {
            /**for (int i = TileID.Count; i < TileLoader.TileCount; i++) {
                ModTile tile = TileLoader.GetTile(i);
                if (true) BarTileIDs.Add(tile.Type);
                bool TileIsBars(Tile tile) {
                    TileID.Sets.
                }
            }/**/
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
        // TODO: fix bar detection
        //static readonly List<int> BarTileIDs = new() { TileID.MetalBars };
        static bool RecipeIsBars(Recipe recipe)
        {
            if (recipe.createItem.createTile == -1) return false;
            bool flag = Main.tileFrameImportant[recipe.createItem.createTile];
            bool fromOre = recipe.requiredItem.Any(item => item.createTile != -1 && TileID.Sets.Ore[item.createTile]);
            bool notFromWalls = recipe.requiredItem.TrueForAll(item => item.createWall == -1);

            return flag && fromOre && notFromWalls;
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
            Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), SlagItem.GetSlagTierID(tRecipe - 1));
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

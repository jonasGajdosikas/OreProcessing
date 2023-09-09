using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace OreProcessing.Content.Furnaces
{
    public class BlastingHMForge : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 18 };
            TileObjectData.addTile(Type);

            AdjTiles = new int[] { TileID.AdamantiteForge ,TileID.Hellforge, TileID.Furnaces };

            AddMapEntry(new Color(192, 189, 221), ModContent.GetInstance<BlastingTitaniumForgeItem>().DisplayName);
            AddMapEntry(new Color(231, 53, 56), ModContent.GetInstance<BlastingAdamantiteForgeItem>().DisplayName);
            AnimationFrameHeight = 74;
        }
        public override ushort GetMapOption(int i, int j) => (ushort)(Main.tile[i, j].TileFrameX / 48);
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            int style = GetMapOption(i,j);
            if (style == 0)
                yield return new Item(ModContent.ItemType<BlastingTitaniumForgeItem>());
            else if (style == 1)
                yield return new Item(ModContent.ItemType<BlastingAdamantiteForgeItem>());
            else
                throw new Exception("Invalid Style");
        }
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            if (++frameCounter >= 5)
            {
                frameCounter = 0;
                frame = ++frame % 6;
            }
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.93f;
            g = 0.06f;
            b = 0.05f;
        }
    }
    public class BlastingAdamantiteForgeItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.AdamantiteForge);
            Item.placeStyle = 1;
            Item.createTile = ModContent.TileType<BlastingHMForge>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<BlastingHellforgeItem>()
                .AddIngredient(ItemID.CrystalBall)
                .AddIngredient(ItemID.AdamantiteOre, 40)
                .AddIngredient(ItemID.CrystalShard, 20)
                .AddIngredient(ItemID.SoulofLight, 10)
                .AddIngredient(ItemID.SoulofNight, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class BlastingTitaniumForgeItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.TitaniumForge);
            Item.placeStyle = 0;
            Item.createTile = ModContent.TileType<BlastingHMForge>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<BlastingHellforgeItem>()
                .AddIngredient(ItemID.CrystalBall)
                .AddIngredient(ItemID.TitaniumOre, 40)
                .AddIngredient(ItemID.CrystalShard, 20)
                .AddIngredient(ItemID.SoulofLight, 10)
                .AddIngredient(ItemID.SoulofNight, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}

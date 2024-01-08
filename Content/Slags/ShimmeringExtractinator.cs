using Microsoft.CodeAnalysis.Options;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using OreProcessing.Content.Furnaces;
using System;
using System.ComponentModel;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ObjectData;

namespace OreProcessing.Content.Slags {
	public class ShimmeringExtractinator : ModTile
	{
        public override void SetStaticDefaults() {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Origin = new Terraria.DataStructures.Point16(1, 2);
            TileObjectData.newTile.CoordinateHeights = new int[] {16,16,16};
            TileObjectData.addTile(Type);

            //AdjTiles = new int[] { };

            AnimationFrameHeight = 52;
        }
        public override void AnimateTile(ref int frame, ref int frameCounter) {
            if (++frameCounter >= 5) {
                frameCounter = 0;
                frame = ++frame % 6;
            }
        }
    }
    public class ShimmeringExtractinatorItem : ModItem {
        public override void SetDefaults() {
            Item.CloneDefaults(ItemID.ChlorophyteExtractinator);
            Item.createTile = ModContent.TileType<ShimmeringExtractinator>();
        }
        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.BottomlessShimmerBucket)
                .AddIngredient(ItemID.ChlorophyteExtractinator)
                .AddIngredient(ItemID.FragmentVortex, 5)
                .AddIngredient(ItemID.FragmentNebula, 5)
                .AddIngredient(ItemID.FragmentSolar, 5)
                .AddIngredient(ItemID.FragmentStardust, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
    public class ShimmeringExtraction : GlobalItem {
        public override void ExtractinatorUse(int extractType, int extractinatorBlockType, ref int resultType, ref int resultStack) {
            int amberMosquitoOdds = 5000;
            int gemOdds = 25;
            int amberOdds = 50;
            if (extractType == ItemID.Sets.ExtractinatorMode[ItemID.DesertFossil]) extractType = 1;
            switch (extractType) {
                case 0: // silt/slush
                    break;
                case 1: //desert fossil
                    amberMosquitoOdds /= 3;
                    gemOdds *= 2;
                    amberOdds = 20;
                    if (NextBool(10)) {
                        resultType = ItemID.FossilOre;
                        resultStack = 1;
                        if (NextBool(5))
                            resultStack += NextHighBound(2);
                        if (NextBool(10))
                            resultStack += NextHighBound(3);
                        if (NextBool(15))
                            resultStack += NextHighBound(4);
                        return;
                    }
                    break;
                case 2: //moss
                    resultType = NextHighBound(4) switch {
                        0 => 2674,
                        1 => 2006,
                        2 => 2675,
                        _ => 2002
                    };
                    resultStack = 1;
                    return;
                case 3: //fishing junk
                    resultType = (extractinatorBlockType == TileID.ChlorophyteExtractinator && NextBool(10)) ?
                        NextHighBound(5) switch {
                            0 => 4354,
                            1 => 4389,
                            2 => 4377,
                            3 => 5127,
                            _ => 4378,
                        } : 
                        NextHighBound(5) switch {
                             0 => 4349,
                             1 => 4350,
                             2 => 4351,
                             3 => 4352,
                             _ => 4353,
                        };
                    resultStack = 1;
                    return;
                default:
                    base.ExtractinatorUse(extractType, extractinatorBlockType, ref resultType, ref resultStack);
                    return;
            }

            
            static bool NextBool(int E) => Main.rand.NextBool(E);
            static int Next(int MinVal, int MaxVal) => Main.rand.Next(MinVal, MaxVal);
            static int NextHighBound(int MaxVal) => Main.rand.Next(MaxVal);

            int type;
            int stack = 1;
            if (NextBool(amberMosquitoOdds)) {
                resultType = ItemID.AmberMosquito;
                resultStack = 1;
                return;
            }
            
            static int IncrementStack(int stack, int times, int odds, int lowNum, int highNum) {
                for (int i = 0; i < times; i++)
                    if (NextBool(odds))
                        stack += Next(lowNum, highNum);
                return stack;
            }
            static int IncrementStackIncrementally(int stack) {
                for (int i = 2; i <= 6; i++)
                    if (NextBool(i * 10))
                        stack += Next(0, i);
                return stack;
            }

            if (NextBool(2)) {
                if (NextBool(12000)) {
                    type = ItemID.PlatinumCoin;
                    resultStack = IncrementStack(stack, 3, 14, 0, 2);
                } else if (NextBool(800)) {
                    type = ItemID.GoldCoin;
                    resultStack = IncrementStack(stack, 5, 6, 1, 21);
                } else if (NextBool(60)) {
                    type = ItemID.SilverCoin;
                    resultStack = IncrementStack(stack, 4, 4, 5, 26);
                } else {
                    type = ItemID.CopperCoin;
                    resultStack = IncrementStack(stack, 4, 3, 10, 26);
                }
                resultType = type;
                return;
            } 
            if (NextBool(gemOdds)) {
                resultType = NextHighBound(6) switch {
                    0 => 181,
                    1 => 180,
                    2 => 177,
                    3 => 179,
                    4 => 178,
                    _ => 182,
                };
                resultStack = IncrementStackIncrementally(stack);
                resultStack = stack;
                return;
            } 
            if (NextBool(amberOdds)) {
                resultType = ItemID.Amber;
                resultStack = IncrementStackIncrementally(stack);
                return;
            } 
            if (NextBool(3)) {
                if (NextBool(5000)) {
                    resultType = ItemID.PlatinumCoin;
                    resultStack = IncrementStack(stack, 3, 10, 0, 3);
                    return;

                }
                if (NextBool(400)) {
                    resultType = ItemID.GoldCoin;
                    resultStack = IncrementStack(stack, 5, 5, 1, 21);
                    return;
                }
                if (NextBool(30)) {
                    resultType = ItemID.SilverCoin;
                    resultStack = IncrementStack(stack, 4, 3, 5, 26);
                    return;
                }
                resultType = ItemID.CopperCoin;
                resultStack = IncrementStack(stack, 4, 2, 10, 26);
                return;

            } 
            if (extractinatorBlockType == 642) {
                resultType = NextHighBound(14) switch {
                    0 => 12,
                    1 => 11,
                    2 => 14,
                    3 => 13,
                    4 => 699,
                    5 => 700,
                    6 => 701,
                    7 => 702,
                    8 => 364,
                    9 => 1104,
                    10 => 365,
                    11 => 1105,
                    12 => 366,
                    _ => 1106,
                }; 
                resultStack = IncrementStackIncrementally(stack);
                return;
            } 
            resultType = NextHighBound(8) switch {
                0 => 12,
                1 => 11,
                2 => 14,
                3 => 13,
                4 => 699,
                5 => 700,
                6 => 701,
                _ => 702,
            };
            resultStack = IncrementStackIncrementally(stack);
            return;
        }
        public override void Load() {
            On_Player.PlaceThing_ItemInExtractinator += ShimmerExtractinator;
        }
        public override void Unload() {
            On_Player.PlaceThing_ItemInExtractinator -= ShimmerExtractinator;
        }
        private static void ShimmerExtractinator(On_Player.orig_PlaceThing_ItemInExtractinator orig, Player self, ref Player.ItemCheckContext context) {
            Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
            if (tile.TileType != ModContent.TileType<ShimmeringExtractinator>()) {
                orig.Invoke(self, ref context);
            } else {
                float itemTime = .16f;
                int ExtractTileID = ModContent.TileType<ShimmeringExtractinator>();
                if (!self.adjTile[ExtractTileID] || !self.ItemTimeIsZero || self.itemAnimation <= 0 || !self.controlUseItem)
                    return;
                Item item = self.inventory[self.selectedItem];

                void DropItemFromExtractinator(int itemType, int itemStack) {
                    Vector2 vector = Main.ReverseGravitySupport(Main.MouseScreen) + Main.screenPosition;
                    if (Main.SmartCursorIsUsed || PlayerInput.UsingGamepad)
                        vector = self.Center;

                    int number = Item.NewItem(self.GetSource_TileInteraction(Player.tileTargetX, Player.tileTargetY), (int)vector.X, (int)vector.Y, 1, 1, itemType, itemStack, noBroadcast: false, -1);
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f);
                }

                int extractType = ItemID.Sets.ExtractinatorMode[item.type];
                if (ItemTrader.ChlorophyteExtractinator.TryGetTradeOption(item, out var option)) {
                    SoundEngine.PlaySound(SoundID.Grab);
                    self.ApplyItemTime(item, itemTime);
                    context.SkipItemConsumption = true;
                    item.stack -= option.TakingItemStack;
                    if (item.stack <= 0)
                        item.TurnToAir();
                    DropItemFromExtractinator(option.GivingITemType, option.GivingItemStack);
                } else if (extractType >= 0) {
                    SoundEngine.PlaySound(SoundID.Grab);
                    self.ApplyItemTime(item, itemTime);

                    int num = ModContent.GetInstance<ExtractConfig>().ExtractItemAmount;
                    if (item.stack > num) {
                        for (int i = 1; i < num; i++) {
                            Extractinate(extractType);
                            item.stack--;
                            if (item.stack <= 0) {
                                item.TurnToAir();
                                break;
                            }
                        }
                    }
                    Extractinate(extractType);
                    void Extractinate(int extractType) {
                        int resultType = ItemID.SiltBlock, resultStack = 1;
                        ItemLoader.ExtractinatorUse(ref resultType, ref resultStack, extractType, TileID.ChlorophyteExtractinator);
                        //Main.NewText($"Item: {resultType}, stack: {resultStack}");
                        DropItemFromExtractinator(resultType, resultStack);
                    }
                }
            }
        }
    }
    public class ExtractConfig : ModConfig {
        public override ConfigScope Mode => ConfigScope.ServerSide;

		[DefaultValue(28)]
		public int ExtractItemAmount;
    }
}
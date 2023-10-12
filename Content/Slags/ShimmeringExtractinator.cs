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
            int num = 5000;
            int num2 = 25;
            int num3 = 50;
            int num4 = -1;
            int num5 = -1;
            int num6 = -1;
            int num7 = 1;
            switch (extractType) {
                case 1:
                    num /= 3;
                    num2 *= 2;
                    num3 = 20;
                    num4 = 10;
                    break;
                case 2:
                    num = -1;
                    num2 = -1;
                    num5 = 1;
                    num7 = -1;
                    break;
                case 3:
                    num = -1;
                    num2 = -1;
                    num5 = -1;
                    num7 = -1;
                    num6 = 1;
                    break;
                default:
                    base.ExtractinatorUse(extractType, extractinatorBlockType, ref resultType, ref resultStack);
                    return;
            }

            static bool NextBool(int E) => Main.rand.NextBool(E);
            static int Next(int MinVal, int MaxVal) => Main.rand.Next(MinVal, MaxVal);
            static int NextHighBound(int MaxVal) => Main.rand.Next(MaxVal);

            int type;
            int stack = 1;
            if (num4 != -1 && NextBool(num4)) {
                type = 3380;
                if (NextBool(5))
                    stack += NextHighBound(2);

                if (NextBool(10))
                    stack += NextHighBound(3);

                if (NextBool(15))
                    stack += NextHighBound(4);
            } else if (num7 != -1 && NextBool(2)) {
                if (NextBool(12000)) {
                    type = 74;
                    if (NextBool(14))
                        stack += Next(0, 2);

                    if (NextBool(14))
                        stack += Next(0, 2);

                    if (NextBool(14))
                        stack += Next(0, 2);
                } else if (NextBool(800)) {
                    type = 73;
                    if (NextBool(6))
                        stack += Next(1, 21);

                    if (NextBool(6))
                        stack += Next(1, 21);

                    if (NextBool(6))
                        stack += Next(1, 21);

                    if (NextBool(6))
                        stack += Next(1, 21);

                    if (NextBool(6))
                        stack += Next(1, 20);
                } else if (NextBool(60)) {
                    type = 72;
                    if (NextBool(4))
                        stack += Next(5, 26);

                    if (NextBool(4))
                        stack += Next(5, 26);

                    if (NextBool(4))
                        stack += Next(5, 26);

                    if (NextBool(4))
                        stack += Next(5, 25);
                } else {
                    type = 71;
                    if (NextBool(3))
                        stack += Next(10, 26);

                    if (NextBool(3))
                        stack += Next(10, 26);

                    if (NextBool(3))
                        stack += Next(10, 26);

                    if (NextBool(3))
                        stack += Next(10, 25);
                }
            } else if (num != -1 && NextBool(num)) {
                type = 1242;
            } else if (num5 != -1) {
                type = ((!NextBool(4)) ? 2674 : ((!NextBool(3)) ? 2006 : ((NextBool(3)) ? 2675 : 2002)));
            } else if (num6 != -1 && extractinatorBlockType == 642) {
                if (NextBool(10)) {
                    type = NextHighBound(5) switch {
                        0 => 4354,
                        1 => 4389,
                        2 => 4377,
                        3 => 5127,
                        _ => 4378,
                    };
                } else {
                    type = NextHighBound(5) switch {
                        0 => 4349,
                        1 => 4350,
                        2 => 4351,
                        3 => 4352,
                        _ => 4353,
                    };
                }
            } else if (num6 != -1) {
                type = NextHighBound(5) switch {
                    0 => 4349,
                    1 => 4350,
                    2 => 4351,
                    3 => 4352,
                    _ => 4353,
                };
            } else if (num2 != -1 && NextBool(num2)) {
                type = NextHighBound(6) switch {
                    0 => 181,
                    1 => 180,
                    2 => 177,
                    3 => 179,
                    4 => 178,
                    _ => 182,
                };
                if (NextBool(20))
                    stack += Next(0, 2);

                if (NextBool(30))
                    stack += Next(0, 3);

                if (NextBool(40))
                    stack += Next(0, 4);

                if (NextBool(50))
                    stack += Next(0, 5);

                if (NextBool(60))
                    stack += Next(0, 6);
            } else if (num3 != -1 && NextBool(num3)) {
                type = 999;
                if (NextBool(20))
                    stack += Next(0, 2);

                if (NextBool(30))
                    stack += Next(0, 3);

                if (NextBool(40))
                    stack += Next(0, 4);

                if (NextBool(50))
                    stack += Next(0, 5);

                if (NextBool(60))
                    stack += Next(0, 6);
            } else if (NextBool(3)) {
                if (NextBool(5000)) {
                    type = 74;
                    if (NextBool(10))
                        stack += Next(0, 3);

                    if (NextBool(10))
                        stack += Next(0, 3);

                    if (NextBool(10))
                        stack += Next(0, 3);

                    if (NextBool(10))
                        stack += Next(0, 3);

                    if (NextBool(10))
                        stack += Next(0, 3);
                } else if (NextBool(400)) {
                    type = 73;
                    if (NextBool(5))
                        stack += Next(1, 21);

                    if (NextBool(5))
                        stack += Next(1, 21);

                    if (NextBool(5))
                        stack += Next(1, 21);

                    if (NextBool(5))
                        stack += Next(1, 21);

                    if (NextBool(5))
                        stack += Next(1, 20);
                } else if (NextBool(30)) {
                    type = 72;
                    if (NextBool(3))
                        stack += Next(5, 26);

                    if (NextBool(3))
                        stack += Next(5, 26);

                    if (NextBool(3))
                        stack += Next(5, 26);

                    if (NextBool(3))
                        stack += Next(5, 25);
                } else {
                    type = 71;
                    if (NextBool(2))
                        stack += Next(10, 26);

                    if (NextBool(2))
                        stack += Next(10, 26);

                    if (NextBool(2))
                        stack += Next(10, 26);

                    if (NextBool(2))
                        stack += Next(10, 25);
                }
            } else if (extractinatorBlockType == 642) {
                type = NextHighBound(14) switch {
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
                if (NextBool(20))
                    stack += Next(0, 2);

                if (NextBool(30))
                    stack += Next(0, 3);

                if (NextBool(40))
                    stack += Next(0, 4);

                if (NextBool(50))
                    stack += Next(0, 5);

                if (NextBool(60))
                    stack += Next(0, 6);
            } else {
                type = NextHighBound(8) switch {
                    0 => 12,
                    1 => 11,
                    2 => 14,
                    3 => 13,
                    4 => 699,
                    5 => 700,
                    6 => 701,
                    _ => 702,
                };
                if (NextBool(20))
                    stack += Next(0, 2);

                if (NextBool(30))
                    stack += Next(0, 3);

                if (NextBool(40))
                    stack += Next(0, 4);

                if (NextBool(50))
                    stack += Next(0, 5);

                if (NextBool(60))
                    stack += Next(0, 6);
            }

            if (type > 0) {
                resultType = type;
                resultStack = stack;
            }
            
        }
        public override void Load() {
            On_Player.PlaceThing_ItemInExtractinator += ShimmerExtractinator;
        }
        public override void Unload() {
            On_Player.PlaceThing_ItemInExtractinator -= ShimmerExtractinator;
        }
        private static void ShimmerExtractinator(On_Player.orig_PlaceThing_ItemInExtractinator orig, Player self, ref Player.ItemCheckContext context) {
            Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
            if (tile.TileType == ModContent.TileType<ShimmeringExtractinator>()) {
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
                    if (item.stack >= num) {
                        for (int i = 0; i < num; i++) {
                            Extractinate(extractType);
                            item.stack--;
                            if (item.stack <= 0) {
                                item.TurnToAir();
                                break;
                            }
                        }
                    } else {
                        Extractinate(extractType);
                    }
                    void Extractinate(int extractType) {
                        int resultType = ItemID.SiltBlock, resultStack = 1;
                        ItemLoader.ExtractinatorUse(ref resultType, ref resultStack, extractType, TileID.Extractinator);
                        Main.NewText($"Item: {resultType}, stack: {resultStack}");
                        DropItemFromExtractinator(resultType, resultStack);
                    }
                }
            } else {
                orig.Invoke(self, ref context);
            }

        }
    }
    public class ExtractingPlayer : ModPlayer {

    }
    public class ExtractConfig : ModConfig {
        public override ConfigScope Mode => ConfigScope.ServerSide;

		[DefaultValue(28)]
		public int ExtractItemAmount;
    }
}
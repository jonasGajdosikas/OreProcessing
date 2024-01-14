using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace OreProcessing.Content.Fortune
{
    public class PlacedTracker : ModSystem
    {
        public override void SaveWorldData(TagCompound tag)
        {
            NaturalTile.SaveWorldData(tag);
            base.SaveWorldData(tag);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            NaturalTile.LoadWorldData(tag);
            base.LoadWorldData(tag);
        }
        public override void ClearWorld()
        {
            base.ClearWorld();
        }
    }
    public class NaturalTile : GlobalTile
    {
        static HashSet<uint> PlacedTiles = new();
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            if (ValidOreTile(i, j, type))
            {
                uint p = Combine((uint)i, (uint)j);
                if (!PlacedTiles.Contains(p))
                {
                    PlacedTiles.Add(p);
                }
            }
        }
        public static bool IsNatural(int i, int j)
        {
            uint p = Combine((uint)i, (uint)j);
            return PlacedTiles.Contains(p);
        }
        public static uint Combine(uint i, uint j) => (i << 16) | j;
        public static void SaveWorldData(TagCompound tag)
        {
            tag[nameof(PlacedTiles)] = PlacedTiles.ToList();
        }
        public static void LoadWorldData(TagCompound tag)
        {
            PlacedTiles = new(tag.ContainsKey(nameof(PlacedTiles)) ? tag.Get<List<uint>>(nameof(PlacedTiles)) : new());

        }

        public override void Drop(int i, int j, int type)
        {
            if (!ValidOreTile(i, j, type))
                return;
            Player player = Main.LocalPlayer;
            if (!player.GetModPlayer<FortunePlayer>().hasMiningFortune || !IsNatural(i, j))
            {
                base.Drop(i, j, type);
                return;
            } 
            int numDrops = (int)player.luck + (player.luck - MathF.Floor(player.luck) > Main.rand.NextFloat() ? 1 : 0);
            int itemType = TileLoader.GetItemDropFromTypeAndStyle(type);
            Item item = new(itemType, numDrops, -1);
            item.Prefix(-1);
            int num = Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, item);
            Main.item[num].TryCombiningIntoNearbyItems(num);
        }

        private static bool ValidOreTile(int i, int j, int type)
        {
            if (TileLoader.IsTileSpelunkable(i, j, type) ?? Main.tileSpelunker[type])
                return !Main.tileCut[type];
            return false;
        }
    }
    public class FortunePlayer : ModPlayer
    {
        public bool hasMiningFortune;
        public override void ResetEffects()
        {
            hasMiningFortune = false;
        }
    }
    public class FortuneGlobalItem : GlobalItem
    {
        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if ((head.type == ItemID.UltrabrightHelmet || head.type == ItemID.MiningHelmet) && body.type == ItemID.MiningShirt && legs.type == ItemID.MiningPants) return "miningSet";
            return "";
        }
        public override void UpdateArmorSet(Player player, string set)
        {
            switch (set)
            {
                case ("miningSet"):
                    player.GetModPlayer<FortunePlayer>().hasMiningFortune = true;
                    player.luck += .1f;
                    return;
                default:
                    base.UpdateArmorSet(player, set);
                    break;
            }
        }
        static readonly string[] FargoMinerEffectItems = { "MinerEnchant", "WorldShaperSoul", "DimensionSoul", "EternitySoul" };
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (ModLoader.TryGetMod("FargowiltasSouls", out Mod FargowiltasSouls))
            {
                int FargoItemType(string name)
                {
                    FargowiltasSouls.TryFind(name, out ModItem fargoItem);
                    return fargoItem.Type;
                }
                if (FargoMinerEffectItems.Any(name => FargoItemType(name) == item.type))
                    player.GetModPlayer<FortunePlayer>().hasMiningFortune = true;
            }
        }
    }
}

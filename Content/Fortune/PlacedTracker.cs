using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace OreProcessing.Content.Fortune
{
    public class PlacedTracker : ModSystem
    {

    }
    public class NaturalTile : GlobalTile
    {
        // TODO: fix isNatural to actually reflect single tiles; not the whole world
        public bool isNatural = true;
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            isNatural = false;
        }

        public override void Drop(int i, int j, int type)
        {
            bool ValidOreTile()
            {
                if (TileLoader.IsTileSpelunkable(i, j, type) ?? Main.tileSpelunker[type])
                    return !Main.tileCut[type];
                return false;
            }
            if (!ValidOreTile())
                return;
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<FortunePlayer>().hasMiningFortune)
            {
                int numDrops = (int)player.luck + (player.luck - MathF.Floor(player.luck) > Main.rand.NextFloat() ? 1 : 0);
                
                int itemType = TileLoader.GetItemDropFromTypeAndStyle(type);
                Item item = new(itemType, numDrops, -1);
                item.Prefix(-1);
                int num = Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, item);
                Main.item[num].TryCombiningIntoNearbyItems(num);
            }
            base.Drop(i, j, type);
        }
        public override void Load()
        {
            base.Load();
        }
    }
    public class FortunePlayer : ModPlayer
    {
        public bool hasMiningFortune;
        
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
    }
}

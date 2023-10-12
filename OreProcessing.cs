using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;

namespace OreProcessing
{
	public class OreProcessing : Mod
    {
        private static Asset<Texture2D> slagTex;
        public static Asset<Texture2D> SlagTex => slagTex;
        public override void Load()
        {
            slagTex = Assets.Request<Texture2D>("Content/Slags/Slag");
        }
        
    }
}
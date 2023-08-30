using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;

namespace OreProcessing
{
	public class OreProcessing : Mod
    {
        public static Asset<Texture2D> SlagTex;
        public override void Load()
        {
            SlagTex = Assets.Request<Texture2D>("Content/Slags/Slag");
        }
    }
}
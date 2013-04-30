using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LandOfAmbrosia.UI
{
    class ImageScreen : Screen
    {
        Texture2D image;

        public ImageScreen(Game game, string asset)
        {
            image = game.Content.Load<Texture2D>(asset);
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Rectangle drawArea)
        {
            sb.Draw(image, drawArea, Color.White);
        }
    }
}

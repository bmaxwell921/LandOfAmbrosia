using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LandOfAmbrosia.UI
{
    class TextScreen : Screen
    {
        string message;
        SpriteFont font;

        public TextScreen(string message, SpriteFont font)
        {
            this.message = message;
            this.font = font;
        }

        public void Draw(SpriteBatch sb, Rectangle drawArea)
        {
            sb.DrawString(font, message, new Vector2(drawArea.X, drawArea.Y), Color.White);
        }
    }
}

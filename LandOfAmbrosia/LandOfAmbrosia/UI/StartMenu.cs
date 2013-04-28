using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace LandOfAmbrosia.UI
{
    class StartMenu : Menu
    {
        SpriteFont titleFont;
        SpriteFont startScreenFont;

        Texture2D selectedText;
        Texture2D backgroundText;

        bool leftSelected;

        public StartMenu(Game game)
        {
            startScreenFont = game.Content.Load<SpriteFont>(@"Fonts\StartScreen");
            titleFont = game.Content.Load<SpriteFont>(@"Fonts\Title");

            selectedText = new Texture2D(game.GraphicsDevice, 1, 1);
            selectedText.SetData(new Color[] { Color.White });

            backgroundText = new Texture2D(game.GraphicsDevice, 1, 1);
            backgroundText.SetData(new Color[] { Color.Black });

            leftSelected = true;
        }

        public void Draw(SpriteBatch sb, Game game)
        {
            game.GraphicsDevice.Clear(Color.Black);

            string title = "Land of Ambrosia";
            string one = "One Player";
            string two = "Two Player";

            int windowWidth = game.GraphicsDevice.Viewport.Width;
            int windowHeight = game.GraphicsDevice.Viewport.Height;
            Vector2 titleSize = titleFont.MeasureString(title);

            sb.DrawString(titleFont, title, new Vector2(windowWidth / 2 - titleSize.X / 2, windowHeight * (3 / 8f) - titleSize.Y / 2), Color.White);

            Vector2 oneSize = startScreenFont.MeasureString(one);
            Vector2 twoSize = startScreenFont.MeasureString(two);

            float extraSpace = windowWidth - (oneSize.X + twoSize.X + 20);

            float oneStartX = extraSpace / 2;
            float twoStartX = windowWidth - (extraSpace / 2 + twoSize.X);

            Rectangle selectedArea = new Rectangle((int)(leftSelected ? oneStartX : twoStartX) - 2, (int)(windowHeight * (5 / 8f) - 2), (int)(leftSelected ? oneSize.X : twoSize.X) + 2, (int)oneSize.Y + 2);
            Rectangle selectedMidArea = new Rectangle((int)(leftSelected ? oneStartX : twoStartX), (int)(windowHeight * (5 / 8f)), (int)(leftSelected ? oneSize.X : twoSize.X) - 2, (int)oneSize.Y - 2);

            sb.Draw(selectedText, selectedArea, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            sb.Draw(backgroundText, selectedMidArea, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.5f);

            //Wouldn't show up when I used this call
            //sb.DrawString(startScreenFont, one, new Vector2(oneStartX, windowHeight * (5 / 8f)), Color.White, 0, Vector2.Zero, 0, SpriteEffects.None, 1);
            //sb.DrawString(startScreenFont, two, new Vector2(twoStartX, windowHeight * (5 / 8f)), Color.White, 0, Vector2.Zero, 0, SpriteEffects.None, 1);

            sb.DrawString(startScreenFont, one, new Vector2(oneStartX, windowHeight * (5 / 8f)), Color.White);
            sb.DrawString(startScreenFont, two, new Vector2(twoStartX, windowHeight * (5 / 8f)), Color.White);
        }

        public void Update(bool leftPressed, bool rightPressed)
        {
            if (leftPressed && !leftSelected)
            {
                leftSelected = !leftSelected;
            }
            else if (rightPressed && leftSelected)
            {
                leftSelected = !leftSelected;
            }
        }
    }
}

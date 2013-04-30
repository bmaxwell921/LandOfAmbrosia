using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LandOfAmbrosia.UI
{
    //TODO this is also a 2 choice menu!!!
    class PauseMenu : Menu
    {
        IList<Screen> screens;
        int screen;
        Game game;

        SpriteFont titleFont;
        SpriteFont choiceFont;
        SpriteFont detailsFont;

        public PauseMenu(Game game)
        {
            this.game = game;
            screens = new List<Screen>();
            screen = 0;
            titleFont = game.Content.Load<SpriteFont>(@"Fonts\Title");
            choiceFont = game.Content.Load<SpriteFont>(@"Fonts\Options");
            detailsFont = game.Content.Load<SpriteFont>(@"Fonts\Details");
            LoadImages();
        }

        private void LoadImages()
        {
            screens.Add(new ImageScreen(game, @"Images\xboxController"));
            screens.Add(new ImageScreen(game, @"Images\inGame"));
            screens.Add(new ImageScreen(game, @"Images\text"));
        }

        public void Draw(SpriteBatch sb, Game game)
        {
            //Draw the string at the top, then the image, then the stuff at the bottom
            game.GraphicsDevice.Clear(Color.Black);

            string title = "Pause";

            int windowWidth = game.GraphicsDevice.Viewport.Width;
            int windowHeight = game.GraphicsDevice.Viewport.Height;
            Vector2 titleSize = titleFont.MeasureString(title);

            sb.DrawString(titleFont, title, new Vector2(windowWidth / 2 - titleSize.X / 2, windowHeight * (1f / 8f) - titleSize.Y / 2f), Color.White);

            Rectangle imageLoc = new Rectangle((int) (windowWidth * (1.5f/8f)), (int) (windowHeight * (2f/8f)), 
                (int) (windowWidth * (5f / 8f)), (int) (windowHeight * (5f / 8f)));

            screens[screen].Draw(sb, imageLoc);

            //Draw left arrow
            if (screen != 0)
            {
                string left = " Prev\n<--";
                Vector2 leftSize = choiceFont.MeasureString(left);

                sb.DrawString(choiceFont, left, new Vector2(5, windowHeight / 2), Color.White); 
            }

            if (screen != screens.Count - 1)
            {
                string right = "Next\n  -->";
                Vector2 rightSize = choiceFont.MeasureString(right);

                sb.DrawString(choiceFont, right, new Vector2(windowWidth - (rightSize.X) - 5, windowHeight / 2), Color.White);
            }
        }

        public void Update(bool leftPressed, bool rightPressed)
        {
            if (leftPressed && screen != 0)
            {
                --screen;
            }
            else if (rightPressed && screen != screens.Count - 1)
            {
                ++screen;
            }
        }

        public void ConfirmSelection()
        {
            ((LandOfAmbrosiaGame)game).TransitionToState(Common.GameState.PLAYING);
        }
    }
}

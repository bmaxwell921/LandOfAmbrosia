﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LandOfAmbrosia.UI
{
    abstract class TwoChoiceMenu : Menu
    {
        private SpriteFont titleFont;
        private SpriteFont choicesFont;

        private Texture2D selectedText;
        private Texture2D backgroundSelected;

        protected Game game;

        protected bool leftSelected;

        public TwoChoiceMenu(Game game)
        {
            selectedText = new Texture2D(game.GraphicsDevice, 1, 1);
            selectedText.SetData(new Color[] { Color.White });

            backgroundSelected = new Texture2D(game.GraphicsDevice, 1, 1);
            backgroundSelected.SetData(new Color[] { Color.Black });

            leftSelected = true;

            this.game = game;
        }

        protected void setTitleFont(SpriteFont font)
        {
            titleFont = font;
        }

        protected void setChoicesFont(SpriteFont font)
        {
            choicesFont = font;
        }

        protected abstract string getTitleMessage();

        protected abstract IList<string> getChoices();

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Microsoft.Xna.Framework.Game game)
        {
            game.GraphicsDevice.Clear(Color.Black);

            string title = getTitleMessage();
            IList<string> choices = getChoices();

            int windowWidth = game.GraphicsDevice.Viewport.Width;
            int windowHeight = game.GraphicsDevice.Viewport.Height;
            Vector2 titleSize = titleFont.MeasureString(title);

            sb.DrawString(titleFont, title, new Vector2(windowWidth / 2 - titleSize.X / 2, windowHeight * (3 / 8f) - titleSize.Y / 2), Color.White);

            Vector2 oneSize = choicesFont.MeasureString(choices[0]);
            Vector2 twoSize = choicesFont.MeasureString(choices[1]);

            float extraSpace = windowWidth - (oneSize.X + twoSize.X + 20);

            float oneStartX = extraSpace / 2;
            float twoStartX = windowWidth - (extraSpace / 2 + twoSize.X);

            Rectangle selectedArea = new Rectangle((int)(leftSelected ? oneStartX : twoStartX) - 2, (int)(windowHeight * (5 / 8f) - 2), (int)(leftSelected ? oneSize.X : twoSize.X) + 2, (int)oneSize.Y + 2);
            Rectangle selectedMidArea = new Rectangle((int)(leftSelected ? oneStartX : twoStartX), (int)(windowHeight * (5 / 8f)), (int)(leftSelected ? oneSize.X : twoSize.X) - 2, (int)oneSize.Y - 2);

            sb.Draw(selectedText, selectedArea, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            sb.Draw(backgroundSelected, selectedMidArea, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.5f);

            sb.DrawString(choicesFont, choices[0], new Vector2(oneStartX, windowHeight * (5 / 8f)), Color.White);
            sb.DrawString(choicesFont, choices[1], new Vector2(twoStartX, windowHeight * (5 / 8f)), Color.White);

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

        public abstract void ConfirmSelection();
    }
}

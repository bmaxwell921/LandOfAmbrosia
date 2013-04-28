using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LandOfAmbrosia.Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using LandOfAmbrosia.UI;

namespace LandOfAmbrosia.Managers
{
    /// <summary>
    /// Class to handle all the menu interactions of the game. Player1 has control of the menus
    /// </summary>
    class MenuManager : DrawableGameComponent
    {
        private SpriteBatch sb;

        IList<Menu> menus;

        int currentMenu;

        bool leftPressed = true;
        bool rightPressed = false;

        bool isXbox;

        public MenuManager(Game game, SpriteBatch sb, bool isXbox)
            : base(game)
        {
            this.sb = sb;
            menus = new List<Menu>();
            menus.Add(new StartMenu(Game));
            this.isXbox = isXbox;

            currentMenu = 0;
        }

        public override void Update(GameTime gameTime)
        {
            GameState curState = ((LandOfAmbrosiaGame)Game).curState;

            checkInput();

            if (curState == GameState.START_SCREEN || curState == GameState.PAUSE || curState == GameState.GAME_OVER || curState == GameState.VICTORY)
            {
                menus[currentMenu].Update(leftPressed, rightPressed);
            }
        }


        private void checkInput()
        {
            if (!isXbox)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.J))
                {
                    Console.WriteLine("Moving Left!");
                    leftPressed = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.L))
                {
                    rightPressed = true;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.J))
                {
                    leftPressed = false;
                }
                if (Keyboard.GetState().IsKeyUp(Keys.L))
                {
                    rightPressed = false;
                }
            }
            else
            {
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadLeft))
                {
                    leftPressed = true;
                }
                if (GamePad.GetState(PlayerIndex.One).IsButtonUp(Buttons.DPadLeft))
                {
                    leftPressed = false;
                }
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadRight))
                {
                    rightPressed = true;
                }
                if (GamePad.GetState(PlayerIndex.One).IsButtonUp(Buttons.DPadRight))
                {
                    rightPressed = false;
                }
            }
        }

        //The Menu Manager only gets enabled when we are in start screen mode, game over mode, or victory mode. So only draw it then
        public override void Draw(GameTime gameTime)
        {
            GameState curState = ((LandOfAmbrosiaGame)Game).curState;

            //sb.Begin(SpriteSortMode.FrontToBack, BlendState.Opaque);
            sb.Begin();

            if (curState == GameState.START_SCREEN || curState == GameState.PAUSE || curState == GameState.GAME_OVER || curState == GameState.VICTORY)
            {
                menus[currentMenu].Draw(sb, Game);
            }

            sb.End();
            base.Draw(gameTime);
        }

        private void DrawVictory()
        {
            throw new NotImplementedException();
        }

        private void DrawGameOver()
        {
            throw new NotImplementedException();
        }

        private void DrawPauseMenu()
        {
            throw new NotImplementedException();
        }


    }
}

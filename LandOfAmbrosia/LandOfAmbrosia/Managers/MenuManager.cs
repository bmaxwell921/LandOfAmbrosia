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
        private readonly int START_SCREEN = 0;
        private readonly int GAME_OVER = 1;
        private readonly int VICTORY = 2;
        private readonly int RESPAWN = 3;
        private readonly int PAUSE = 4;
        int currentMenu;

        //IList<Menu> menus;
        IDictionary<int, Menu> menus;

        private readonly int WAIT = 75;
        private int lastRead;

        bool leftPressed = true;
        bool rightPressed = false;
        bool confirmPressed = false;

        bool isXbox;

        public MenuManager(Game game, SpriteBatch sb, bool isXbox)
            : base(game)
        {
            this.sb = sb;
            //menus = new List<Menu>();
            //menus.Add(new StartMenu(Game));
            //menus.Add(new GameOverMenu(Game));

            menus = new Dictionary<int, Menu>();
            menus.Add(START_SCREEN, new StartMenu(Game));
            menus.Add(GAME_OVER, new GameOverMenu(Game));
            menus.Add(RESPAWN, new RespawnMenu(Game));
            menus.Add(VICTORY, new VictoryMenu(Game));
            menus.Add(PAUSE, new PauseMenu(Game));

            this.isXbox = isXbox;
            currentMenu = START_SCREEN;
        }

        private void updateCurrentMenu()
        {
            GameState curState = ((LandOfAmbrosiaGame)Game).curState;
            if (curState == GameState.START_SCREEN)
            {
                currentMenu = START_SCREEN;
            }
            else if (curState == GameState.GAME_OVER)
            {
                currentMenu = GAME_OVER;
            }
            else if (curState == GameState.VICTORY)
            {
                currentMenu = VICTORY;
            }
            else if (curState == GameState.RESPAWN)
            {
                currentMenu = RESPAWN;
            }
            else
            {
                currentMenu = PAUSE;
            }
        }

        public override void Update(GameTime gameTime)
        {
            GameState curState = ((LandOfAmbrosiaGame)Game).curState;

            checkInput(gameTime);
            updateCurrentMenu();

            if (curState == GameState.START_SCREEN || curState == GameState.PAUSE || curState == GameState.GAME_OVER || curState == GameState.VICTORY || curState == GameState.RESPAWN)
            {
                menus[currentMenu].Update(leftPressed, rightPressed);

                if (confirmPressed)
                {
                    menus[currentMenu].ConfirmSelection();
                    confirmPressed = false;
                }
            }
        }


        private void checkInput(GameTime gameTime)
        {
            if (lastRead <= 0)
            {
                if (!isXbox)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.J))
                    {
                        leftPressed = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.L))
                    {
                        rightPressed = true;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Y))
                    {
                        confirmPressed = true;
                    }
                }
                else
                {
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0)
                    {
                        leftPressed = true;
                    }

                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0)
                    {
                        rightPressed = true;
                    }

                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Y))
                    {
                        Console.WriteLine("Confirm pressed");
                        confirmPressed = true;
                    }
                }
                lastRead = WAIT;
            }
            else
            {
                lastRead -= gameTime.ElapsedGameTime.Milliseconds;
                leftPressed = false;
                rightPressed = false;
                confirmPressed = false;
            }
        }

        //The Menu Manager only gets enabled when we are in start screen mode, game over mode, or victory mode. So only draw it then
        public override void Draw(GameTime gameTime)
        {
            GameState curState = ((LandOfAmbrosiaGame)Game).curState;
            updateCurrentMenu();
            //sb.Begin(SpriteSortMode.FrontToBack, BlendState.Opaque);
            sb.Begin();

            if (curState == GameState.START_SCREEN || curState == GameState.PAUSE || curState == GameState.GAME_OVER || curState == GameState.VICTORY || curState == GameState.RESPAWN)
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

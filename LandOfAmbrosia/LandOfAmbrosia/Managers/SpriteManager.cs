using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LandOfAmbrosia.Common;
using Microsoft.Xna.Framework.Content;
using LandOfAmbrosia.Characters;

namespace LandOfAmbrosia.Managers
{
    class SpriteManager : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Texture2D healthTexture;
        Texture2D expTexture;
        Texture2D statBackground;
        SpriteFont font;

        private readonly int PLAYER_MAX_STAT_WIDTH;// = 200;
        private readonly int PLAYER_STAT_HEIGHT = 10;

        private readonly int MINION_MAX_HEALTH_WIDTH;// = 50;
        private readonly int MINION_HEALTH_HEIGHT = 5;

        private readonly int BUFFER = 5;

        public SpriteManager(Game game, SpriteBatch sb)
            : base(game)
        {
            spriteBatch = sb;
            healthTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            healthTexture.SetData(new Color[] { Color.Red });

            expTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            expTexture.SetData(new Color[] { Color.Gold });

            statBackground = new Texture2D(Game.GraphicsDevice, 1, 1);
            statBackground.SetData(new Color[] { Color.Gray });
            font = Game.Content.Load<SpriteFont>(@"Fonts\UIFont");

            PLAYER_MAX_STAT_WIDTH = (int) (game.GraphicsDevice.Viewport.Width / 4f);
            MINION_MAX_HEALTH_WIDTH = PLAYER_MAX_STAT_WIDTH / 4;
            
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.Opaque);
            if (((LandOfAmbrosiaGame)Game).curState == GameState.PLAYING || (((LandOfAmbrosiaGame)Game).curState == GameState.NEXT_LEVEL_WAIT))
            {
                //Draw the lives left in the level
                this.DrawLivesLeft();

                //Draw the health bars over the minions
                this.DrawEnemyHealth();

                //Draw the health of the players
                this.DrawPlayerHealth();

                //Draw the number of enemies left
                this.DrawEnemiesLeft();
            }

            if (((LandOfAmbrosiaGame)Game).curState == GameState.NEXT_LEVEL_WAIT)
            {
                //Draw the message to move to the next level
                this.DrawGotoNextMessage();
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawGotoNextMessage()
        {
            string message = "Press Y to progress to the next level";
            int height = Game.GraphicsDevice.Viewport.Height;
            int width = Game.GraphicsDevice.Viewport.Width;

            Vector2 messageSize = font.MeasureString(message);

            spriteBatch.DrawString(font, message, new Vector2((width / 2f) - (messageSize.X / 2), height * (3f / 8f)), Color.White);
        }

        private void DrawLivesLeft()
        {
            LevelManager lm = ((LevelManager)Game.Services.GetService(typeof(LevelManager)));
            int lives = lm.levels[lm.curLevelInfo].numLives;

            string message = "Lives Left: " + lives;
            int height = Game.GraphicsDevice.Viewport.Height;
            int width = Game.GraphicsDevice.Viewport.Width;
            Vector2 messageSize = font.MeasureString(message);
            spriteBatch.DrawString(font, message, new Vector2((width / 2) - (messageSize.X / 2), height - font.MeasureString(message).Y - BUFFER), Color.Yellow);
        }

        private void DrawEnemyHealth()
        {
            IList<Character> enemies = ((LevelManager)Game.Services.GetService(typeof(LevelManager))).getEnemies();

            CameraComponent cam = ((LandOfAmbrosiaGame)Game).camera;

            foreach (Character e in enemies)
            {
                Vector3 enemyPos = new Vector3(e.getX(), e.getY(), Constants.CHARACTER_DEPTH);
                Vector3 drawPos = Game.GraphicsDevice.Viewport.Project(new Vector3(e.getX(), e.getY(), Constants.CHARACTER_DEPTH), cam.ProjectionMatrix, cam.ViewMatrix, Matrix.Identity);
                this.DrawCharacterInfo(e, new Vector2(drawPos.X, drawPos.Y));
            }
        }

        //Draws health bars for both players at the top of the screen
        private void DrawPlayerHealth()
        {
            IList<Character> players = ((LevelManager) Game.Services.GetService(typeof(LevelManager))).getPlayers();

            //Draw player1's health
            if (players.Count >= 1 && players[0] != null)
            {
                this.DrawCharacterInfo(players[0], new Vector2(BUFFER, BUFFER));
            }

            if (players.Count >= 2 && players[1] != null)
            {
                this.DrawCharacterInfo(players[1], new Vector2(Game.GraphicsDevice.Viewport.Width - this.PLAYER_MAX_STAT_WIDTH - BUFFER, BUFFER));
            }
        }

        private void DrawEnemiesLeft()
        {
            int enemiesLeft = ((LevelManager)Game.Services.GetService(typeof(LevelManager))).getEnemies().Count;
            int height = Game.GraphicsDevice.Viewport.Height;
            string message = "Enemies left: " + enemiesLeft;
            spriteBatch.DrawString(font, message, new Vector2(BUFFER, height - font.MeasureString(message).Y - BUFFER), Color.Yellow); 
        }

        private void DrawCharacterInfo(Character c, Vector2 location)
        {
            float curHealth = c.stats.getStatCurrentVal(Constants.HEALTH_KEY);
            float baseHealth = c.stats.getStatBaseVal(Constants.HEALTH_KEY);
            int maxWidth = (c is UserControlledCharacter) ? PLAYER_MAX_STAT_WIDTH : MINION_MAX_HEALTH_WIDTH;
            int height = (c is UserControlledCharacter) ? PLAYER_STAT_HEIGHT : MINION_HEALTH_HEIGHT;

            int pixWide = (int) ((curHealth / baseHealth) * maxWidth);

            spriteBatch.Draw(statBackground, new Rectangle((int)location.X, (int)location.Y, maxWidth, height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(healthTexture, new Rectangle((int)location.X, (int)location.Y, pixWide, height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);

            if (c is UserControlledCharacter)
            {
                //Draw experience stuff
                float curExp = c.stats.getStatCurrentVal(Constants.EXPERIENCE_KEY);
                float neededExp = c.stats.getStatBaseVal(Constants.EXPERIENCE_KEY);

                int expPixWide = (int)((curExp / neededExp) * PLAYER_MAX_STAT_WIDTH);

                spriteBatch.Draw(statBackground, new Rectangle((int)location.X, (int)location.Y + BUFFER + height, maxWidth, height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                spriteBatch.Draw(expTexture, new Rectangle((int)location.X, (int)location.Y + BUFFER + height, expPixWide, height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            }
        }
    }
}

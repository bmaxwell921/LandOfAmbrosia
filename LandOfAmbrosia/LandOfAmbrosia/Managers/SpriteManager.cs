using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LandOfAmbrosia.Common;
using Microsoft.Xna.Framework.Content;

namespace LandOfAmbrosia.Managers
{
    class SpriteManager : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Texture2D healthTexture;
        Texture2D healthBackground;
        SpriteFont font;

        private readonly int PLAYER_MAX_HEALTH_WIDTH = 200;
        private readonly int PLAYER_HEALTH_HEIGHT = 20;

        private readonly int MINION_MAX_HEALTH_WIDTH = 50;
        private readonly int MINION_HEALTH_HEIGHT = 5;

        public SpriteManager(Game game, SpriteBatch sb, ContentManager content )
            : base(game)
        {
            spriteBatch = sb;
            healthTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            healthTexture.SetData(new Color[] { Color.Red });

            healthBackground = new Texture2D(Game.GraphicsDevice, 1, 1);
            healthBackground.SetData(new Color[] { Color.Gray });
            font = content.Load<SpriteFont>(@"Fonts\UIFont");
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.Opaque);
            //Draw the health bars over the minions
            this.DrawEnemyHealth();

            //Draw the health of the players
            this.DrawPlayerHealth();

            //Draw the number of enemies left
            this.DrawEnemiesLeft();
            //Draw the Level number

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawEnemyHealth()
        {
            IList<Character> enemies = ((LevelManager)Game.Services.GetService(typeof(LevelManager))).getEnemies();

            CameraComponent cam = ((LandOfAmbrosiaGame)Game).camera;

            foreach (Character e in enemies)
            {
                Vector3 enemyPos = new Vector3(e.getX(), e.getY(), Constants.CHARACTER_DEPTH);
                Vector3 drawPos = Game.GraphicsDevice.Viewport.Project(new Vector3(e.getX(), e.getY(), Constants.CHARACTER_DEPTH), cam.ProjectionMatrix, cam.ViewMatrix, Matrix.Identity);

                spriteBatch.Draw(healthBackground, new Rectangle((int) drawPos.X, (int) drawPos.Y, MINION_MAX_HEALTH_WIDTH, MINION_HEALTH_HEIGHT), Color.White);

                float curHealth = e.stats.getStatCurrentVal(Constants.HEALTH_KEY);
                float baseHealth = e.stats.getStatBaseVal(Constants.HEALTH_KEY);

                int pixWide = (int)((curHealth / baseHealth) * this.MINION_MAX_HEALTH_WIDTH);
                spriteBatch.Draw(healthTexture, new Rectangle((int) drawPos.X, (int) drawPos.Y, pixWide, MINION_HEALTH_HEIGHT), Color.White);
            }
        }

        //Draws health bars for both players at the top of the screen
        private void DrawPlayerHealth()
        {
            IList<Character> players = ((LevelManager) Game.Services.GetService(typeof(LevelManager))).getPlayers();

            //Draw player1's health
            if (players.Count >= 1 && players[0] != null)
            {
                //Drawing background color TODO why is this in front of the red?
                spriteBatch.Draw(healthBackground, new Rectangle(5, 5, PLAYER_MAX_HEALTH_WIDTH, PLAYER_HEALTH_HEIGHT), Color.White);

                float p1CurHealth = players[0].stats.getStatCurrentVal(Constants.HEALTH_KEY);
                float p1BaseHealth = players[0].stats.getStatBaseVal(Constants.HEALTH_KEY);

                int pixWide = (int) ((p1CurHealth / p1BaseHealth) * this.PLAYER_MAX_HEALTH_WIDTH);
                spriteBatch.Draw(healthTexture, new Rectangle(5, 5, pixWide, PLAYER_HEALTH_HEIGHT), Color.White);
            }

            if (players.Count >= 2 && players[1] != null)
            {
                //Drawing background color
                spriteBatch.Draw(healthBackground, new Rectangle(5, 5, PLAYER_MAX_HEALTH_WIDTH, PLAYER_HEALTH_HEIGHT), Color.White);

                float p2CurHealth = players[1].stats.getStatCurrentVal(Constants.HEALTH_KEY);
                float p2BaseHealth = players[1].stats.getStatBaseVal(Constants.HEALTH_KEY);

                int pixWide = (int) ((p2CurHealth / p2BaseHealth) * this.PLAYER_MAX_HEALTH_WIDTH);
                int leftStart = Game.GraphicsDevice.Viewport.Width - this.PLAYER_MAX_HEALTH_WIDTH - 5;
                Console.WriteLine("Starting player 2 health at: " + leftStart);
                spriteBatch.Draw(healthTexture, new Rectangle(leftStart, 5, pixWide, PLAYER_HEALTH_HEIGHT), Color.White);
            }
        }

        private void DrawEnemiesLeft()
        {
            int enemiesLeft = ((LevelManager)Game.Services.GetService(typeof(LevelManager))).getEnemies().Count;
            int height = Game.GraphicsDevice.Viewport.Height;
            string message = "Enemies left: " + enemiesLeft;
            spriteBatch.DrawString(font, message, new Vector2(5, height - font.MeasureString(message).Y - 5), Color.Yellow); 
        }
    }
}

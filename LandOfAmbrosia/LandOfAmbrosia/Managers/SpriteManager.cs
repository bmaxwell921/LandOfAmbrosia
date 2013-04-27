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
        Texture2D healthBackground;
        SpriteFont font;

        private readonly int PLAYER_MAX_HEALTH_WIDTH = 200;
        private readonly int PLAYER_HEALTH_HEIGHT = 20;

        private readonly int MINION_MAX_HEALTH_WIDTH = 50;
        private readonly int MINION_HEALTH_HEIGHT = 5;

        private readonly int BUFFER = 5;

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
                this.DrawCharacterInfo(players[1], new Vector2(Game.GraphicsDevice.Viewport.Width - this.PLAYER_MAX_HEALTH_WIDTH - BUFFER, BUFFER));
            }
        }

        private void DrawEnemiesLeft()
        {
            int enemiesLeft = ((LevelManager)Game.Services.GetService(typeof(LevelManager))).getEnemies().Count;
            int height = Game.GraphicsDevice.Viewport.Height;
            string message = "Enemies left: " + enemiesLeft;
            spriteBatch.DrawString(font, message, new Vector2(5, height - font.MeasureString(message).Y - 5), Color.Yellow); 
        }

        private void DrawCharacterInfo(Character c, Vector2 location)
        {
            float curHealth = c.stats.getStatCurrentVal(Constants.HEALTH_KEY);
            float baseHealth = c.stats.getStatBaseVal(Constants.HEALTH_KEY);
            int maxWidth = (c is UserControlledCharacter) ? PLAYER_MAX_HEALTH_WIDTH : MINION_MAX_HEALTH_WIDTH;
            int height = (c is UserControlledCharacter) ? PLAYER_HEALTH_HEIGHT : MINION_HEALTH_HEIGHT;

            int pixWide = (int) ((curHealth / baseHealth) * maxWidth);

            spriteBatch.Draw(healthBackground, new Rectangle((int)location.X, (int)location.Y, maxWidth, height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(healthTexture, new Rectangle((int)location.X, (int)location.Y, pixWide, height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);

            if (c is UserControlledCharacter)
            {
                //Draw experience stuff
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LandOfAmbrosia.Managers
{
    class LevelManager : DrawableGameComponent
    {
        #region Level Fields
        private Level currentLevel;
        private Model[] tileModels;
        private String[] tileModelAssets = { null, @"Models/platform"};
        //Empty, Ground
        private const int numTileTypes = 2;
        #endregion

        /// <summary>
        /// Constructs a new LevelManager with the default Level
        /// </summary>
        public LevelManager(Game game) :
            base(game)
        {
            currentLevel = new Level();
        }

        protected override void LoadContent()
        {
            this.LoadTileModels();
            currentLevel.GenerateLevel(tileModels);
            base.LoadContent();
        }

        private void LoadTileModels()
        {
            tileModels = new Model[numTileTypes];
            for (int i = 0; i < numTileTypes; ++i)
            {
                tileModels[i] = tileModelAssets[i] == null ? null : Game.Content.Load<Model>(tileModelAssets[i]);
            }
        }

        /// <summary>
        /// Constructor that creates a level by reading in the level from a file
        /// </summary>
        /// <param name="game"></param>
        /// <param name="levelFileLoc"></param>
        public LevelManager(Game game, String levelFileLoc) : 
            base(game)
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            currentLevel.Draw(((Game1)Game).camera);

            base.Draw(gameTime);
        }
    }
}

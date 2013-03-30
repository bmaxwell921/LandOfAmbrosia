using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LandOfAmbrosia.Common;
using LandOfAmbrosia.Logic;

namespace LandOfAmbrosia.Managers
{
    class LevelManager : DrawableGameComponent
    {
        #region Level Fields
        private Level currentLevel;
        //Empty, Ground
        #endregion
        public Random gen;

        /// <summary>
        /// Constructs a new LevelManager with the default Level
        /// </summary>
        public LevelManager(Game game) :
            this(game, Constants.DEFAULT_SEED)
        {
            
        }

        public LevelManager(Game game, int seed): base(game)
        {
            currentLevel = new Level();
            gen = new Random(seed);
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
            currentLevel.Draw(((Game1)Game).camera, Game.GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}

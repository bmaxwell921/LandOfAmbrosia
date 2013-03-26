using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Levels;
using Microsoft.Xna.Framework;

namespace LandOfAmbrosia.Managers
{
    class LevelManager : DrawableGameComponent
    {
        private Level currentLevel;

        /// <summary>
        /// Constructs a new LevelManager with the default Level
        /// </summary>
        public LevelManager(Game game) :
            base(game)
        {
            currentLevel = new Level();
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

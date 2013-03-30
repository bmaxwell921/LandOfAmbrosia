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

        public LevelManager(Game game): base(game)
        {
            currentLevel = new Level();
            this.SetUpCamera();
        }

        /// <summary>
        /// Constructor that creates a level by reading in the level from a file
        /// </summary>
        /// <param name="game"></param>
        /// <param name="levelFileLoc"></param>
        public LevelManager(Game game, String levelFileLoc) : 
            base(game)
        {
            currentLevel = new Level(levelFileLoc);
            this.SetUpCamera();
        }

        private void SetUpCamera()
        {
            CameraComponent camera = ((LandOfAmbrosiaGame)Game).camera;
            Vector3 eye, target, up;

            eye = new Vector3(Constants.CAMERA_FRAME_WIDTH_BLOCKS * Constants.TILE_WIDTH, currentLevel.height / 2 * Constants.TILE_HEIGHT, 70);
            target = new Vector3(Constants.CAMERA_FRAME_WIDTH_BLOCKS * Constants.TILE_WIDTH, currentLevel.height / 2 * Constants.TILE_HEIGHT, 0);
            up = Vector3.Up;

            camera.LookAt(eye, target, up);
        }

        public override void Draw(GameTime gameTime)
        {
            currentLevel.Draw(((LandOfAmbrosiaGame)Game).camera, Game.GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}

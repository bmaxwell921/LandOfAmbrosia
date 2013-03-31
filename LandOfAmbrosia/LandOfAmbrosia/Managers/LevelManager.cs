using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LandOfAmbrosia.Common;
using LandOfAmbrosia.Logic;
using LandOfAmbrosia.Characters;

namespace LandOfAmbrosia.Managers
{
    /// <summary>
    /// A class to manage a level of the game. The Level is made up of Tiles 
    /// </summary>
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
            //this.SetUpCamera();
        }

        private void SetUpCamera()
        {
            CameraComponent camera = ((LandOfAmbrosiaGame)Game).camera;
            Vector3 eye, target, up;

            eye = new Vector3(Constants.CAMERA_FRAME_WIDTH_BLOCKS * Constants.TILE_SIZE, currentLevel.height / 2 * Constants.TILE_SIZE, 70);
            target = new Vector3(Constants.CAMERA_FRAME_WIDTH_BLOCKS * Constants.TILE_SIZE, currentLevel.height / 2 * Constants.TILE_SIZE, 0);
            up = Vector3.Up;

            camera.LookAt(eye, target, up);
        }

        public override void Draw(GameTime gameTime)
        {
            currentLevel.Draw(((LandOfAmbrosiaGame)Game).camera, Game.GraphicsDevice);

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            this.UpdatePlayers(gameTime);
            base.Update(gameTime);
        }

        private void UpdatePlayers(GameTime gameTime)
        {
            UserControlledCharacter player1 = currentLevel.player1;
            UserControlledCharacter player2 = currentLevel.player2;
            if (player1 != null)
            {
                player1.CheckInput();
                this.UpdateCharacter(player1, gameTime);
                //player1.update(gameTime) updates the animation
            }

            if (player2 != null)
            {
                player2.CheckInput();
                this.UpdateCharacter(currentLevel.player2, gameTime);
                //player2.update(gameTime) updates the animation
            }
        }

        //Updates the player position and handles collisions
        private void UpdateCharacter(Character character, GameTime gameTime)
        {
            if (!character.isFlying() && !character.onGround)
            {
                //update force of gravity
                character.setVelocityY(character.getVelocityY() + Constants.GRAVITY);
            }

            float dx = character.getVelocityX();
            float oldX = character.getX();
            float newX = oldX + dx * gameTime.ElapsedGameTime.Milliseconds;

            Vector3 tile = getTileCollision(character, newX, character.getY());
            if (tile == Vector3.Zero)
            {
                character.setX(newX);
            }
            else
            {
                //Fix tile collisions by sliding
                if (dx > 0)
                {
                    character.setX(currentLevel.tileIndexToPos((int)tile.X) - character.width);
                }
                else if (dx < 0)
                {
                    character.setX(currentLevel.tileIndexToPos((int)tile.X + 1));
                }
                character.collideHorizontal();
            }

            float dy = character.getVelocityY();
            float oldY = character.getY() - character.height;
            float newY = oldY + dy * gameTime.ElapsedGameTime.Milliseconds;
            tile = getTileCollision(character, character.getX(), newY);
            if (tile == Vector3.Zero)
            {
                character.setY(newY);
            }
            else
            {
                //Line up with the tile boundary
                //Going down
                if (dy < 0)
                {
                    character.setY(currentLevel.tileIndexToPos((int)tile.Y) + character.height / 2);
                }
                else if (dy > 0) //Going up
                {
                    character.setY(currentLevel.tileIndexToPos((int)tile.Y - 1));
                }
                character.collideVertical();
            }
        }

        private Vector3 getTileCollision(Character c, float newX, float newY)
        {
            float fromX = Math.Min(c.getX(), newX);
            float fromY = Math.Min(c.getY(), newY);
            float toX = Math.Max(c.getX(), newX);
            float toY = Math.Max(c.getY(), newY);

            int fromTileX = currentLevel.posToTileIndex(fromX);
            int fromTileY = currentLevel.posToTileIndex(fromY);

            int toTileX = currentLevel.posToTileIndex(toX + c.width / 2);
            int toTileY = currentLevel.posToTileIndex(toY + c.height);
            for (int x = fromTileX; x <= toTileX; ++x)
            {
                for (int y = fromTileY; y <= toTileY; ++y)
                {
                    if (x < 0 || x >= currentLevel.width|| currentLevel.GetTile(x, y) != null)
                    {
                        return new Vector3(x, y, 0);
                    }
                }
            }

            //No collision
            return Vector3.Zero;
        }
    }
}

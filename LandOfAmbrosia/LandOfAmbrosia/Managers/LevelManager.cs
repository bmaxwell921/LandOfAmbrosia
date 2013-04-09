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
            this.SetUpCameraDefault();
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

        //TODO this method doesn't work right
        //private void SetUpCamera()
        //{
        //    CameraComponent camera = ((LandOfAmbrosiaGame)Game).camera;
        //    Vector3 eye, target, up;

        //    eye = new Vector3(Constants.CAMERA_FRAME_WIDTH_BLOCKS * Constants.TILE_SIZE, currentLevel.height / 2 * Constants.TILE_SIZE, 20);
        //    target = new Vector3(Constants.CAMERA_FRAME_WIDTH_BLOCKS * Constants.TILE_SIZE, currentLevel.height / 2 * Constants.TILE_SIZE, 0);
        //    up = Vector3.Up;

        //    camera.LookAt(eye, target, up);
        //}

        
        private void SetUpCameraDefault()
        {
            CameraComponent camera = ((LandOfAmbrosiaGame)Game).camera;
            Vector3 eye, target, up;
            eye = new Vector3(0, 0, 20);
            target = new Vector3(0, 0, 0);
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
            float newX = oldX + dx;
            bool hasCollision;
            Vector3 posFix = getTileCollision(character, newX, character.getY(), true, out hasCollision);
            if (!hasCollision)
            {
                character.setX(newX);
            }
            else
            {
                //posFix will be the vector needed to fix the collision, so just add the fix to the vector that's messing everything up
                character.setX(oldX + posFix.X);
                character.collideHorizontal();
            }

            float dy = character.getVelocityY();
            //The center of the character is in the middle, so to check the feet we need to move the point down half the size of the character
            float oldY = character.getY();
            float newY = oldY + dy;
            posFix = getTileCollision(character, character.getX(), newY, false, out hasCollision);
            if (!hasCollision)
            {
                character.setY(newY);
            }
            else
            {
                character.setY(oldY + posFix.Y);
                character.collideVertical();
            }

            if (character.getY() < -20)
            {
                character.position = Constants.ConvertToXNAScene(Constants.DEFAULT_PLAYER1_START);
                character.velocity = Vector3.Zero;
            }
        }
        float oldNewX = 0;
        float oldNewY = 0;
        private Vector3 getTileCollision(Character c, float newX, float newY, bool leftRight, out bool hasCollision)
        {
            float divValue = 2.0f;
            //Get the four corners of the 'bounding box' and check those tile locations for objects
            IList<Vector3> cornerPositions = new List<Vector3>();
            
            //Top left corner
            cornerPositions.Add(new Vector3(newX - c.width / 2, newY + c.height / divValue, 0));

            //Top right corner
            cornerPositions.Add(new Vector3(newX + c.width / 2, newY + c.height / divValue, 0));

            //Bottom left corner
            cornerPositions.Add(new Vector3(newX - c.width / 2, newY - c.height / divValue, 0));

            //Bottom right corner
            cornerPositions.Add(new Vector3(newX + c.width / 2, newY - c.height / divValue, 0));

            if (newX != oldNewX)
            {
                Console.WriteLine("X changed from: " + oldNewX + " to: "+ newX);
                oldNewX = newX;
            }
            if (newY != oldNewY)
            {
                Console.WriteLine("Y changed from: " + oldNewY + " to: " + newY);
                oldNewY = newY;
            }

            for (int i = 0; i < cornerPositions.Count; ++i)
            {
                Vector3 curPos = cornerPositions.ElementAt(i);

                //TODO figure out why we need to add 1
                Vector2 curTile = new Vector2(currentLevel.posToTileIndex(curPos.X), currentLevel.posToTileIndex(curPos.Y)) + new Vector2(0, 1);

                if (i % cornerPositions.Count == 2)
                {
                    //Console.WriteLine(curTile);
                }

                if (curTile == new Vector2(0, 1))
                {
                    int stop;
                }

                Tile intersectingTile = currentLevel.GetTile((int)curTile.X, (int)curTile.Y);
                if (intersectingTile != null)
                {
                    //Collision, return the vector to fix the movement

                    //Will return the point on the tile that can be used to fix the movement
                    Vector3 tilePt = this.findTilePoint(c, intersectingTile, newX, newY, leftRight);

                    //Based on my math...if we subtract the current point from the colliding tile point we will get the correct movement
                    hasCollision = true;
                    return tilePt - curPos;
                }
            }

            hasCollision = false;
            return Vector3.Zero;
        }

        private Vector3 findTilePoint(Character c, Tile intersectingTile, float newX, float newY, bool leftRight)
        {
            Vector3 tilePt = Vector3.Zero;
            if (leftRight)
            {
                float tileLeft = intersectingTile.getX();// -intersectingTile.width / 2;
                float tileRight = intersectingTile.getX() + intersectingTile.width;// / 2;
                //If we are moving to the left, ie newX < curX, the tile point we need is the right side 
                tilePt.X = (newX < c.getX()) ? tileRight : tileLeft;
            }
            else
            {
                float tileTop = intersectingTile.getY() * Constants.TILE_HEIGHT;// -(intersectingTile.height / 2);
                float tileBottom = intersectingTile.getY() * Constants.TILE_HEIGHT - intersectingTile.height;
                tilePt.Y = (newY < c.getY()) ? tileTop : tileBottom;
            }
            return tilePt;
        }

        private Vector3 getTileCollision2(Character c, float newX, float newY, out bool hasCollision)
        {
            float fromX = Math.Min(c.getX(), newX);
            float fromY = Math.Min(c.getY(), newY);
            float toX = Math.Max(c.getX(), newX);
            float toY = Math.Max(c.getY(), newY);

            int fromTileX = currentLevel.posToTileIndex(fromX);
            int fromTileY = currentLevel.posToTileIndex(fromY);

            int toTileX = currentLevel.posToTileIndex(toX + c.height / 2 - 1);
            int toTileY = currentLevel.posToTileIndex(toY + c.height / 2 - 1);

            if (toTileX < 0 || fromTileX < 0)
            {
                Console.WriteLine();
            }

            /*
             * fromTileX must be <= toTileX and fromTileY must be <= toTileY
             */
            for (int x = fromTileX; x <= toTileX; ++x)
            {
                for (int y = fromTileY; y <= toTileY; ++y)
                {
                    if (x < 0 || x >= currentLevel.width|| currentLevel.GetTile(x, y) != null)
                    {
                        hasCollision = true;
                        return new Vector3(x, y, 0);
                    }
                }
            }

            //No collision
            hasCollision = false;
            return Vector3.Zero;
        }
    }
}

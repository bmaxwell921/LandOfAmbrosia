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
        /// Constructor for testing purposes
        /// </summary>
        /// <param name="game"></param>
        /// <param name="testConstructor"></param>
        public LevelManager(Game game, bool testConstructor) : base(game)
        {
            currentLevel = new Level(testConstructor);
            this.MakeStairs();
            this.AddMinion();
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
            this.SetUpCameraDefault();
        }

        private void MakeStairs()
        {
            currentLevel.SetTile(3, 1, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), new Vector3(3 * Constants.TILE_SIZE, 1 * Constants.TILE_SIZE, 0)));

            currentLevel.SetTile(4, 1, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), new Vector3(4 * Constants.TILE_SIZE, 1 * Constants.TILE_SIZE, 0)));
            currentLevel.SetTile(4, 2, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), new Vector3(4 * Constants.TILE_SIZE, 2 * Constants.TILE_SIZE, 0)));

            currentLevel.SetTile(5, 1, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), new Vector3(5 * Constants.TILE_SIZE, 1 * Constants.TILE_SIZE, 0)));
            currentLevel.SetTile(5, 2, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), new Vector3(5 * Constants.TILE_SIZE, 2 * Constants.TILE_SIZE, 0)));
            currentLevel.SetTile(5, 3, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), new Vector3(5 * Constants.TILE_SIZE, 3 * Constants.TILE_SIZE, 0)));

            currentLevel.SetTile(6, 1, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), new Vector3(6 * Constants.TILE_SIZE, 1 * Constants.TILE_SIZE, 0)));
            currentLevel.SetTile(6, 2, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), new Vector3(6 * Constants.TILE_SIZE, 2 * Constants.TILE_SIZE, 0)));

            currentLevel.SetTile(7, 1, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), new Vector3(7 * Constants.TILE_SIZE, 1 * Constants.TILE_SIZE, 0)));
        }

        private void AddMinion()
        {
            //currentLevel.enemies.Add(new Minion(AssetUtil.GetEnemyModel(Constants.MINION_CHAR), new Vector3(4, 0, 2)));//Constants.DEFAULT_PLAYER1_START));
            currentLevel.enemies.Add(new Minion(AssetUtil.GetEnemyModel(Constants.MINION_CHAR), new Vector3(4, 1, 2))); // Hack this to the right spot for draws
            //currentLevel.enemies.Add(new Minion(AssetUtil.GetEnemyModel(Constants.MINION_CHAR), Constants.DEFAULT_PLAYER1_START));
        }
        
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
            //this.UpdateEnemies(gameTime);
            this.UpdateCamera();
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
                player1.Update(gameTime);//updates the animation
                this.CheckTurnOnGravity(player1);
            }

            if (player2 != null)
            {
                player2.CheckInput();
                this.UpdateCharacter(currentLevel.player2, gameTime);
                player2.Update(gameTime); //updates the animation
                this.CheckTurnOnGravity(player2);
            }
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            foreach (Character enemy in currentLevel.enemies)
            {
                if (enemy != null)
                {
                    this.UpdateCharacter(enemy, gameTime);
                    //enemy.Update(gameTime);
                }
            }
        }

        private void UpdateCamera()
        {
            //Centers the camera on the average position between the two characters
            Vector3 target = Vector3.Zero;
            int numPlayers = 0;
            float dist = 0;
            if (currentLevel.player1 != null)
            {
                target += Constants.UnconvertFromXNAScene(currentLevel.player1.position);
                ++numPlayers;
                dist = target.X;
            }

            if (currentLevel.player2 != null)
            {
                target += Constants.UnconvertFromXNAScene(currentLevel.player2.position);
                ++numPlayers;
                dist = Math.Abs(dist - currentLevel.player2.getX());
            }

            target /= numPlayers;
            Vector3 eye = target + new Vector3(0, 0, 20 + dist / 2);

            ((LandOfAmbrosiaGame)Game).camera.LookAt(eye, target, Vector3.Up);
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
                character.setX(newX + posFix.X);
                character.collideHorizontal();
            }

            float dy = character.getVelocityY();
            float oldY = character.getY();
            float newY = oldY + dy;
            posFix = getTileCollision(character, character.getX(), newY, false, out hasCollision);
            if (!hasCollision)
            {
                character.setY(newY);
            }
            else
            {
                character.setY(newY + posFix.Y);
                character.collideVertical();
            }

            if (character.getY() < -20)
            {
                character.position = Constants.ConvertToXNAScene(Constants.DEFAULT_PLAYER1_START);
                character.velocity = Vector3.Zero;
            }
        }

        private void CheckTurnOnGravity(Character c)
        {
            Vector2 belowTile = new Vector2(currentLevel.GetTileIndexFromXPos(c.getX() + Constants.BUFFER), currentLevel.GetTileIndexFromYPos(c.getY() - Constants.BUFFER)) - new Vector2(0, 1);
            Tile curTile = currentLevel.GetTile((int)belowTile.X, (int)belowTile.Y);
            if (curTile == null && c.onGround == true)
            {
                c.onGround = false;
            }
        }

        private Vector3 getTileCollision(Character c, float newX, float newY, bool leftRight, out bool hasCollision)
        {
            if (c is Minion)
            {
                int stop;
            }
            //Get the four corners of the model and check those tile locations for objects
            IList<Vector3> cornerPositions = new List<Vector3>();
            
            //Top left corner
            cornerPositions.Add(new Vector3(newX + Constants.BUFFER, newY - Constants.BUFFER, 0));

            //Top right corner
            cornerPositions.Add(new Vector3(newX + c.width - Constants.BUFFER, newY - Constants.BUFFER, 0));
         
            //Bottom left corner
            cornerPositions.Add(new Vector3(newX + Constants.BUFFER, newY - c.height + Constants.BUFFER, 0));

            //Bottom right corner
            cornerPositions.Add(new Vector3(newX + c.width - Constants.BUFFER, newY - c.height + Constants.BUFFER, 0));

            for (int i = 0; i < cornerPositions.Count; ++i)
            {
                Vector3 curPos = cornerPositions.ElementAt(i);
                Vector2 curTile = new Vector2(currentLevel.GetTileIndexFromXPos(curPos.X), currentLevel.GetTileIndexFromYPos(curPos.Y));

                Tile intersectingTile = currentLevel.GetTile((int)curTile.X, (int)curTile.Y);
                if (intersectingTile != null)
                {
                    if (c is Minion)
                    {
                        int stop;
                    }
                    //Collision, return the vector to fix the movement

                    //Will return the point on the tile that can be used to fix the movement
                    Vector3 tilePt = this.findTilePoint(c, intersectingTile, newX, newY, leftRight);

                    //Based on my math...if we subtract the current point from the colliding tile point we will get the correct movement
                    hasCollision = true;
                    return tilePt - curPos;
                }

                //Check that we aren't off the edge of the level
            }

            hasCollision = false;
            return Vector3.Zero;
        }

        //Returns the Vector3 of the point that we need to move the character to
        private Vector3 findTilePoint(Character c, Tile intersectingTile, float newX, float newY, bool leftRight)
        {
            Vector3 tilePt = Vector3.Zero;
            if (leftRight)
            {
                float tileLeft = intersectingTile.getX() - Constants.BUFFER;
                float tileRight = intersectingTile.getX() + intersectingTile.width + Constants.BUFFER;
                //If we are moving to the left, ie newX < curX, the tile point we need is the right side 
                tilePt.X = (newX < c.getX()) ? tileRight : tileLeft;
            }
            else
            {
                float tileTop = intersectingTile.getY() + Constants.BUFFER;
                float tileBottom = intersectingTile.getY() - intersectingTile.height - Constants.BUFFER;
                tilePt.Y = (newY < c.getY()) ? tileTop : tileBottom;
            }
            return tilePt;
        }
    }
}

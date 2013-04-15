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
using LandOfAmbrosia.Weapons;

namespace LandOfAmbrosia.Managers
{
    /// <summary>
    /// A class to manage a level of the game. The Level is made up of Tiles 
    /// </summary>
    class LevelManager : DrawableGameComponent
    {
        #region Level Fields
        private Level currentLevel;
        private IList<Projectile> projectiles;
        //Empty, Ground
        #endregion

        public LevelManager(Game game)
            : base(game)
        {
            currentLevel = LevelGenerator.GenerateNewLevel(Constants.DEFAULT_WIDTH, Constants.DEFAULT_HEIGHT, Constants.DEFAULT_SEED);
            this.SetUpCameraDefault();
            this.projectiles = new List<Projectile>();
        }

        /// <summary>
        /// Constructor for testing purposes
        /// </summary>
        /// <param name="game"></param>
        /// <param name="testConstructor"></param>
        public LevelManager(Game game, bool testConstructor)
            : base(game)
        {
            currentLevel = LevelGenerator.GenerateNewLevel(8, 8, Environment.TickCount);
            //currentLevel.enemies.Add(new Minion(AssetUtil.GetEnemyModel(Constants.MINION_CHAR), Constants.DEFAULT_PLAYER1_START + new Vector3(4 * Constants.TILE_SIZE, 0, 0)));
            currentLevel.enemies.Add(new Minion(AssetUtil.GetEnemyModel(Constants.MINION_CHAR), Constants.DEFAULT_PLAYER1_START + new Vector3(6 * Constants.TILE_SIZE, 0, 0)));
            currentLevel.enemies.Add(new Minion(AssetUtil.GetEnemyModel(Constants.MINION_CHAR), Constants.DEFAULT_PLAYER1_START + new Vector3(0 * Constants.TILE_SIZE, 0, 0)));
            this.SetUpCameraDefault();
            this.projectiles = new List<Projectile>();
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
            this.projectiles = new List<Projectile>();
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
            DrawProjectiles(((LandOfAmbrosiaGame)Game).camera);
            base.Draw(gameTime);
        }

        private void DrawProjectiles(CameraComponent c)
        {
            foreach (Projectile proj in projectiles)
            {
                if (proj != null)
                {
                    proj.Draw(c);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            this.UpdateProjectiles(gameTime);
            this.UpdatePlayers(gameTime);
            this.UpdateEnemies(gameTime);
            this.UpdateCamera();
            base.Update(gameTime);
        }

        private void UpdateProjectiles(GameTime gameTime)
        {
            IList<Projectile> remainingHack = new List<Projectile>();
            foreach (Projectile proj in projectiles)
            {
                if (proj != null && !proj.ReadyToDie())
                {
                    remainingHack.Add(proj);
                    proj.Update(gameTime);
                }
            }
            projectiles = remainingHack;
        }

        private void UpdatePlayers(GameTime gameTime)
        {
            IList<Character> remainingHack = new List<Character>();
            foreach (UserControlledCharacter player in currentLevel.players)
            {
                if (player != null && !player.isDead())
                {
                    remainingHack.Add(player);
                    player.CheckInput();
                    this.UpdateCharacter(player, gameTime);
                    player.Update(gameTime);
                    this.CheckTurnOnGravity(player);
                }
            }
            currentLevel.players = remainingHack;
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            IList<Character> remainingHack = new List<Character>();
            foreach (Character enemy in currentLevel.enemies)
            {
                if (enemy != null && !enemy.isDead())
                {
                    remainingHack.Add(enemy);
                    this.UpdateCharacter(enemy, gameTime);
                    enemy.Update(gameTime);
                }
            }
            currentLevel.enemies = remainingHack;
        }

        private void UpdateCamera()
        {
            if (currentLevel.players.Count == 0)
            {
                return;
            }
            float maxX = currentLevel.players.ElementAt(0).getX();
            float minX = currentLevel.players.ElementAt(0).getX();
            float maxY = currentLevel.players.ElementAt(0).getY();
            float minY = currentLevel.players.ElementAt(0).getY();
            foreach (UserControlledCharacter player in currentLevel.players)
            {
                if (player.getX() > maxX)
                {
                    maxX = player.getX();
                }
                if (player.getX() < minX)
                {
                    minX = player.getX();
                }
                if (player.getY() > maxY)
                {
                    maxY = player.getY();
                }
                if (player.getY() < minY)
                {
                    minY = player.getY();
                }
            }

            float xCam = (maxX + minX) / 2;
            float yCam = (maxY + minY) / 2;

            Vector3 target = new Vector3(xCam, yCam, 0);
            Vector3 eye = target + new Vector3(0, 0, 30 + (maxX - minX) / 2);
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

            //Check if they attacked with a projectile and add it if they did
            if (character.WantsRangeAttack())
            {

                Projectile proj = character.rangeAttack(gameTime, GetClosestEnemy(character));
                if (proj != null)
                {
                    projectiles.Add(proj);
                }
            }
        }

        private Character GetClosestEnemy(Character c)
        {
            IList<Character> enemies = (c is UserControlledCharacter) ? currentLevel.enemies : currentLevel.players;

            if (enemies.Count != 0)
            {
                Character ret = enemies.ElementAt(0);
                float dist = Vector3.Distance(ret.position, c.position);

                foreach (Character enemy in enemies)
                {
                    float newDist = Vector3.Distance(c.position, enemy.position);
                    if (newDist < dist)
                    {
                        dist = newDist;
                        ret = enemy;
                    }
                }
                return ret;
            }
            return null;
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

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
using Microsoft.Xna.Framework.Input;

namespace LandOfAmbrosia.Managers
{
    public enum LevelState { BEGINNING, PLAYING, NEXT_LEVEL, GAME_OVER, VICTORY };
    /// <summary>
    /// A class to manage a level of the game. The Level is made up of Tiles 
    /// </summary>
    class LevelManager : DrawableGameComponent
    {
        private Level currentLevel;
        public IList<LevelInfo> levels;
        public int curLevelInfo;

        private IList<Projectile> projectiles;
        private bool updateCam;

        private LevelGenerator generator;

        public LevelState curState;

        public LevelManager(Game game, int numPlayers)
            : base(game)
        {
            curState = LevelState.PLAYING;
            updateCam = true;
            curLevelInfo = 0;
            setUpLevels();
            generator = new LevelGenerator(levels, Constants.DEFAULT_SEED);
            currentLevel = generator.GenerateNewLevel(curLevelInfo, numPlayers);
            this.SetUpCameraDefault();
            this.projectiles = new List<Projectile>();
        }

        private void setUpLevels()
        {
            levels = new List<LevelInfo>();
            levels.Add(new LevelInfo(15,  64,   8,  4, 100,  50,  0, Constants.BLUE_PLATFORM));   // blue1
            levels.Add(new LevelInfo(15,  64,   8,  8, 100,  50,  0, Constants.BLUE_PLATFORM));   // blue2
            levels.Add(new LevelInfo(15,  64,   8, 12, 100,  50,  0, Constants.BLUE_PLATFORM));   // blue3
            levels.Add(new LevelInfo(10, 128,  16, 20, 200,  75, 25, Constants.GREEN_PLATFORM));  // green1
            levels.Add(new LevelInfo( 8, 128,  16, 28, 200,  75, 25, Constants.GREEN_PLATFORM));  // green2
            levels.Add(new LevelInfo( 6, 128,  16, 36, 200,  75, 25, Constants.GREEN_PLATFORM));  // green3
            levels.Add(new LevelInfo( 4, 256,  32, 52, 400, 150, 50, Constants.RED_PLATFORM));    // red1
            levels.Add(new LevelInfo( 2, 256,  32, 68, 400, 150, 50, Constants.RED_PLATFORM));    // red2
            levels.Add(new LevelInfo( 2, 256,  32, 84, 400, 150, 50, Constants.RED_PLATFORM));    // red2
        }

        /// <summary>
        /// Constructor for testing purposes
        /// </summary>
        /// <param name="game"></param>
        /// <param name="testConstructor"></param>
        public LevelManager(Game game, bool testConstructor)
            : base(game)
        {
            curState = LevelState.PLAYING;
            ChunkType[,] chunks = { { ChunkType.FLOOR, ChunkType.FLOATING_PLATFORMS_NOT_SAFE }, { ChunkType.STAIRS, ChunkType.EMPTY } };
            levels = new List<LevelInfo>();
            levels.Add(new LevelInfo(1, 16, 16, 1, 100, 0, 0, Constants.RED_PLATFORM));
            levels.Add(new LevelInfo(1, 16, 16, 1, 100, 0, 0, Constants.GREEN_PLATFORM));

            generator = new LevelGenerator(levels, Constants.DEFAULT_SEED, chunks);
            currentLevel = generator.GenerateNewLevel(0, 1);
            updateCam = true;
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
            if (curState == LevelState.PLAYING)
            {
                currentLevel.Draw(((LandOfAmbrosiaGame)Game).camera, Game.GraphicsDevice);
                DrawProjectiles(((LandOfAmbrosiaGame)Game).camera);
            }
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
            if (curState == LevelState.PLAYING)
            {
                this.UpdateProjectiles(gameTime);
                this.UpdatePlayers(gameTime);
                this.UpdateEnemies(gameTime);
                if (updateCam)
                {
                    this.UpdateCamera();
                }
                this.checkEndGame();
            }
            else if (curState == LevelState.NEXT_LEVEL)
            {
                currentLevel = generator.GenerateNewLevel(curLevelInfo, currentLevel.players.Count);
                curState = LevelState.PLAYING;
            }
            base.Update(gameTime);
        }

        private void checkEndGame()
        {
            //Victory!
            if (currentLevel.enemies.Count <= 0)
            {
                curState = ++curLevelInfo >= levels.Count ? LevelState.VICTORY : LevelState.NEXT_LEVEL;
            }
            //Failure
            else if (levels[curLevelInfo].numLives < 0)
            {
                curState = LevelState.GAME_OVER;
            }
        }

        private void UpdateProjectiles(GameTime gameTime)
        {
            IList<Projectile> remainingHack = new List<Projectile>();
            foreach (Projectile proj in projectiles)
            {
                if (proj != null && (!proj.ReadyToDie() && !ProjectileTileCollision(proj)))
                {
                    remainingHack.Add(proj);
                    proj.Update(gameTime);
                }           
            }
            projectiles = remainingHack;
        }

        private bool ProjectileTileCollision(Projectile proj)
        {
            Vector3 position = Constants.UnconvertFromXNAScene(proj.position);
            IList<Vector3> cornerPositions = new List<Vector3>();

            //Top left corner
            cornerPositions.Add(new Vector3(position.X, position.Y, position.Z));

            for (int i = 0; i < cornerPositions.Count; ++i)
            {
                Vector3 curPos = cornerPositions.ElementAt(i);
                Vector2 curTile = new Vector2(currentLevel.GetTileIndexFromXPos(curPos.X), currentLevel.GetTileIndexFromYPos(curPos.Y));

                Tile intersectingTile = currentLevel.GetTile((int)curTile.X, (int)curTile.Y);
                if (intersectingTile != null)
                {
                    return true;
                }
            }
            return false;
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
                if (player != null && player.isDead())
                {
                    --levels[curLevelInfo].numLives;
                    if (levels[curLevelInfo].numLives >= 0)
                    {
                        remainingHack.Add(player);
                        resetCharacters();
                    }
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
                    if (Constants.InRange(enemy, ((LandOfAmbrosiaGame)Game).camera)) {
                        enemy.Update(gameTime);
                    }
                    this.UpdateCharacter(enemy, gameTime);
                    this.CheckTurnOnGravity(enemy);
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
                if (character is UserControlledCharacter)
                {
                    --levels[curLevelInfo].numLives;
                    resetCharacters();
                }
                else
                {
                    character.stats.changeCurrentStat(Constants.HEALTH_KEY, -character.stats.getStatCurrentVal(Constants.HEALTH_KEY));
                }
            }

            //Check if they attacked with a projectile and add it if they did
            if (character.WantsRangeAttack())
            {
                //Ai characters will handle knowing who to attack, so don't bother passing in a value.
                //TODO now that the players have a reference to the level they can find the closest enemy themselves
                Projectile proj = character.rangeAttack(gameTime, character is AICharacter ? null : GetClosestEnemy(character));
                if (proj != null)
                {
                    projectiles.Add(proj);
                }
            }
        }

        private void resetCharacters()
        {
            if (currentLevel.players.Count >= 1)
            {
                currentLevel.players[0].position = Constants.ConvertToXNAScene(levels[curLevelInfo].player1Spawn);
                currentLevel.players[0].stats.resetAllStats();
            }
            if (currentLevel.players.Count >= 2)
            {
                currentLevel.players[1].position = Constants.ConvertToXNAScene(levels[curLevelInfo].player2Spawn);
                currentLevel.players[1].stats.resetAllStats();
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

        public IList<Character> getEnemies()
        {
            return currentLevel.enemies;
        }

        public IList<Character> getPlayers()
        {
            return currentLevel.players;
        }
    }
}

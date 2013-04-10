using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LandOfAmbrosia.Logic;
using LandOfAmbrosia.Common;
using LandOfAmbrosia.Characters;

namespace LandOfAmbrosia.Levels
{
    /// <summary>
    /// I think the level is the one who holds the enemies and characters
    /// </summary>
    class Level
    {

        readonly bool DEBUGGING = false;
        //Map data
        public int width, height;
        public Tile[,] tiles;

        //Character data
        public UserControlledCharacter player1;
        public UserControlledCharacter player2;
        public IList<Character> enemies;

        //Made this a whole skybox cause I already had skybox code
        public Skybox skybox;

        /// <summary>
        /// Constructs a new Level with the default width and height
        /// </summary>
        /// <param name="seed"></param>
        public Level()
            : this(Constants.DEFAULT_WIDTH, Constants.DEFAULT_HEIGHT)
        {
        }

        public Level(bool testConstructor) : this(Constants.DEFAULT_WIDTH, Constants.DEFAULT_HEIGHT)
        {
            TestLevelSetUp();
        }

        //Just for testing, obviously
        private void TestLevelSetUp()
        {
        }

        /// <summary>
        /// Creates an empty Level with the given height and width
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Level(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.tiles = new Tile[width, height];
            this.skybox = new Skybox(AssetUtil.skyboxModel, AssetUtil.skyboxTextures);
            this.FillCage();

            enemies = new List<Character>();
            player1 = new UserControlledCharacter(Constants.PLAYER1_CHAR, AssetUtil.GetPlayerModel(Constants.PLAYER1_CHAR), Constants.DEFAULT_PLAYER1_START);
            player2 = new UserControlledCharacter(Constants.PLAYER2_CHAR, AssetUtil.GetPlayerModel(Constants.PLAYER2_CHAR), Constants.DEFAULT_PLAYER2_START);
        }

        /// <summary>
        /// Constructs a new Level by reading the given file
        /// </summary>
        /// <param name="filePath"></param>
        public Level(String filePath)
        {
            tiles = LevelReader.readLevel(filePath, out width, out height);
            this.skybox = new Skybox(AssetUtil.skyboxModel, AssetUtil.skyboxTextures);
        }

        //For auto generation we automatically fill in the box around the level
        private void FillCage()
        {
            //TODO fill the rest of the cage
            for (int i = 0; i < width; ++i)
            {          
                //Passing in the location unconverted
                //Bottom
                SetTile(i, 0, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), new Vector3(i * Constants.TILE_SIZE, 0, 0)));

                //Left
                SetTile(0, i, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), new Vector3(0, i * Constants.TILE_SIZE, 0)));

                //Right
                SetTile(width - 1, i, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), new Vector3 ((width - 1) * Constants.TILE_SIZE, i * Constants.TILE_SIZE, 0)));

                //Top
                SetTile(i, height - 1, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), new Vector3(i * Constants.TILE_SIZE, (height - 1) * Constants.TILE_SIZE, 0)));
            }
        }

        /// <summary>
        /// Sets the tile in position tiles[width, height] to the newTile
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="newTile"></param>
        public void SetTile(int width, int height, Tile newTile)
        {
            if (width < 0 || width >= this.width || height < 0 || height >= this.height)
            {
                return;
            }
            tiles[width, height] = newTile; 
        }

        /// <summary>
        /// Gets the tile object located at the given location
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Tile GetTile(int width, int height)
        {
            if (width < 0 || width >= this.width || height < 0 || height >= this.height)
            {
                return null;
            }
            return tiles[width, height];
        }

        /// <summary>
        /// Converts an x coordinate from 3D space into an index in the array
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int GetTileIndexFromXPos(float x)
        {
            return (int)(x / Constants.TILE_SIZE);
        }

        /// <summary>
        /// Converts a y coordinate from 3D space into an index in the array
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public int GetTileIndexFromYPos(float y)
        {
            return (y > 0) ? (int)(y / Constants.TILE_SIZE) + 1 : 0;
        }

        /// <summary>
        /// Returns the 3D space point where an index in the array starts
        /// </summary>
        /// <param name="numTiles"></param>
        /// <returns></returns>
        public int tileIndexToPos(int numTiles)
        {
            return numTiles * Constants.TILE_SIZE;
        }

        bool once = false;

        public void Draw(CameraComponent c, GraphicsDevice device)
        {
            skybox.Draw(c, device);

            this.DrawTiles(c);
            this.DrawPlayers(c);
            this.DrawEnemies(c);
        }

        //Draws all the tiles to the screen
        private void DrawTiles(CameraComponent c)
        {
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    if (tiles[i, j] != null)
                    {
                        tiles[i, j].Draw(c);
                    }
                }
            }
        }

        //Draws the players
        private void DrawPlayers(CameraComponent c)
        {
            if (player1 != null)
            {
                Console.WriteLine("Player1 at: " + Constants.UnconvertFromXNAScene(player1.position));
                player1.Draw(c);
            }

            if (player2 != null)
            {
                player2.Draw(c);
            }
        }

        //Draw the enemies
        private void DrawEnemies(CameraComponent c)
        {
            foreach (Character enemy in enemies)
            {
                if (enemy != null)
                {
                    Console.WriteLine("Enemy at: " + Constants.UnconvertFromXNAScene(enemy.position));
                    enemy.Draw(c);
                }
            }
        }
    }
}

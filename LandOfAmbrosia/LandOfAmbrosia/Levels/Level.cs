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
        //Map data
        public int width, height;
        public Tile[,] tiles;

        //Character data
        public IList<Character> players;
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

        public Level(bool testConstructor)
        {
            players = new List<Character>();
            enemies = new List<Character>();
            this.width = 16;
            this.height = 16;
            this.skybox = new Skybox(AssetUtil.skyboxModel, AssetUtil.skyboxTextures);
            TestLevelSetUp();
        }

        //Just for testing, obviously
        private void TestLevelSetUp()
        {
            this.tiles = new Tile[width, height];

            for (int i = 0; i < width; ++i)
            {
                SetTile(i, 0, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR),
                    new Vector3(i * Constants.TILE_SIZE, 0 * Constants.TILE_SIZE, 0)));
            }

            enemies.Add(new Minion(this, AssetUtil.GetEnemyModel(Constants.MINION_CHAR),
                new Vector3(14 * Constants.TILE_SIZE, 4 * Constants.TILE_SIZE, 2 * Constants.CHARACTER_DEPTH), this.players));

            players.Add(new UserControlledCharacter(this, Constants.PLAYER1_CHAR, AssetUtil.GetPlayerModel(Constants.PLAYER1_CHAR), Constants.DEFAULT_PLAYER1_START));  
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

            enemies = new List<Character>();
            players = new List<Character>();
            players.Add(new UserControlledCharacter(this, Constants.PLAYER1_CHAR, AssetUtil.GetPlayerModel(Constants.PLAYER1_CHAR), Constants.DEFAULT_PLAYER1_START));    
            //players.Add(new UserControlledCharacter(this, Constants.PLAYER2_CHAR, AssetUtil.GetPlayerModel(Constants.PLAYER2_CHAR), Constants.DEFAULT_PLAYER2_START));
        }

        /// <summary>
        /// Constructs a new Level by reading the given file
        /// </summary>
        /// <param name="filePath"></param>
        //public Level(String filePath)
        //{
        //    tiles = LevelReader.readLevel(filePath, out width, out height, out players, out enemies);
        //    this.skybox = new Skybox(AssetUtil.skyboxModel, AssetUtil.skyboxTextures);
        //}

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
                    if (tiles[i, j] != null && tileInRange(i, c))
                    {
                        tiles[i, j].Draw(c);
                    }
                }
            }
        }

        private bool tileInRange(int i, CameraComponent c)
        {
            return (i * Constants.TILE_SIZE) - c.Position.X < Constants.TILE_SIZE * 30;
        }

        //Draws the players
        private void DrawPlayers(CameraComponent c)
        {
            foreach (UserControlledCharacter player in players)
            {
                if (player != null)
                {
                    player.Draw(c);
                }
            }
        }

        //Draw the enemies
        private void DrawEnemies(CameraComponent c)
        {
            foreach (Character enemy in enemies)
            {
                if (enemy != null && Constants.InRange(enemy, c))
                {
                    enemy.Draw(c);
                }
            }
        }

        /// <summary>
        /// Returns a list of vertices that will get someone from the starting location to the ending location.
        /// These points should represent the top left corner of a tile
        /// 
        /// M      1 (this row is height 2, because of tile size, width from 0 to 14)
        /// PPPPPPPP (this row is height 0, width from 0 to 14)
        /// 
        /// Returned path should be {(2,2), (3,2), (4,2), (5,2), (6,2), (7,2)}
        /// The numbers are UNCONVERTED!!!
        /// </summary>
        /// <param name="startLoc"></param>
        /// <param name="endLoc"></param>
        /// <returns></returns>
        public IList<Vector2> calculatePath(Vector3 startLoc, Vector3 endLoc)
        {
            IList<Vector2> path = new List<Vector2>();
            /*
             * Perform A* (Might be able to get away with a heuristic dfs. Heuristic based off straightline distance) to find the shortest path. Have a get neighbors method that 
             * returns the 3x3 neighbor hood of a tile? Valid edges are only those that are empty.
             */ 

            return path;
        }
    }
}

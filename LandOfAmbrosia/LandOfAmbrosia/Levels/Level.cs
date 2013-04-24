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
            PriorityQueue<float, Vector2> pq = new PriorityQueue<float, Vector2>(10, new Comparator());

            //Key is a node, value is the parent of the node?
            IDictionary<Vector2, Vector2> parents = new Dictionary<Vector2, Vector2>();
            //IDictionary<Vector2, float> closedSet = new Dictionary<Vector2, float>();
            ISet<Vector2> closedSet = new HashSet<Vector2>();

            Vector2 startTile = new Vector2(GetTileIndexFromXPos(startLoc.X), GetTileIndexFromYPos(startLoc.Y));
            Vector2 endTile = new Vector2(GetTileIndexFromXPos(endLoc.X), GetTileIndexFromYPos(endLoc.Y));

            float h = heuristic(startTile, endTile);
            foreach (Vector2 neighbor in getEmptyNeighborsOf(startTile))
            {
                pq.Enqueue(heuristic(neighbor, endTile) + h, neighbor);
                parents.Add(neighbor, startTile);
            }

            closedSet.Add(startTile);

            while (!pq.IsEmpty)
            {
                
            }

            return path;
        }

        private float heuristic(Vector2 destination, Vector2 currentLoc)
        {
            return Math.Abs(Vector2.Distance(destination, currentLoc));
        }

        class Comparator : IComparer<float>
        {
            public int Compare(float lhs, float rhs)
            {
                return (int) (lhs - rhs);
            }
        }

        //Gets the empty neighbors of a given tile location in the array
        //Only gets north, south, east, and west. No diagonals
        private IList<Vector2> getEmptyNeighborsOf(Vector2 tile)
        {
            //Lol how do we handle holes in the ground?
            IList<Vector2> neigh = new List<Vector2>();

            //for (int i = (int) tile.X - 1; i < (int) tile.X + 1; ++i)
            //{
            //    for (int j = (int) tile.Y - 1; j < (int) tile.Y + 1; ++j)
            //    {
            //        if (i >= 0 && i < width && j >= 0 && j < height && tiles[i,j] == null)
            //        {
            //            neigh.Add(new Vector2(i, j));
            //        }
            //    }
            //}

            Vector2 north = new Vector2(tile.X, tile.Y + 1);
            Vector2 south = new Vector2(tile.X, tile.Y - 1);
            Vector2 east = new Vector2(tile.X - 1, tile.Y);
            Vector2 west = new Vector2(tile.X + 1, tile.Y);

            //Only want empty tiles
            if (tiles[(int)north.X, (int)north.Y] == null && !isOB(north))
            {
                neigh.Add(north);
            }
            if (tiles[(int)south.X, (int)south.Y] == null && !isOB(south))
            {
                neigh.Add(south);
            }
            if (tiles[(int)east.X, (int)east.Y] == null && !isOB(east))
            {
                neigh.Add(east);
            }
            if (tiles[(int)west.X, (int)west.Y] == null && !isOB(west))
            {
                neigh.Add(west);
            }
            return neigh;
        }

        private bool isOB(Vector2 tileIndex)
        {
            return tileIndex.X < 0 || tileIndex.X >= width || tileIndex.Y < 0 || tileIndex.Y >= height;
        }
    }
}

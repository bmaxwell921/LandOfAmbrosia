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
            this.FillFloor();

            enemies = new List<Character>();
            player1 = new UserControlledCharacter(Constants.PLAYER1_CHAR, AssetUtil.GetPlayerModel(Constants.PLAYER1_CHAR), Constants.DEFAULT_PLAYER1_START);
            //player2 = new UserControlledCharacter(Constants.PLAYER2_CHAR, AssetUtil.GetPlayerModel(Constants.PLAYER1_CHAR), Constants.DEFAULT_PLAYER2_START);
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

        //For auto generation we automatically fill in the floor with platforms
        private void FillFloor()
        {
            for (int i = 0; i < width; ++i)
            {          
                //Passing in the location unconverted

                SetTile(i, 0, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), new Vector3(i, 0, 0)));

                //if (i % 4 == 0)
                //    SetTile(i, 1, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), new Vector3(i, 1, 0)));
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

        public Tile GetTile(int width, int height)
        {
            if (width < 0 || width >= this.width || height < 0 || height >= this.height)
            {
                return null;
            }
            return tiles[width, height];
        }

        public int posToTileIndex(float pix)
        {
            return posToTileIndex((int) Math.Round(pix));
        }

        public int posToTileIndex(int pix)
        {
            return (int) Math.Floor((float) pix / Constants.TILE_HEIGHT);
        }

        public int tileIndexToPos(int numTiles)
        {
            return numTiles * Constants.TILE_HEIGHT;
        }

        public void Draw(CameraComponent c, GraphicsDevice device)
        {
            skybox.Draw(c, device);

            this.DrawTiles(c);
            this.DrawPlayers(c);
            this.DrawEnemies(c);
        }

        bool output = false;

        //Draws all the tiles to the screen
        private void DrawTiles(CameraComponent c)
        {
            if (!output)
            {
                Console.WriteLine("Tile at: " + Constants.ConvertToXNAScene(tiles[0, 0].location));
                output = true;
            }
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

        bool output2 = false;
        //Draws the players
        private void DrawPlayers(CameraComponent c)
        {
            if (!output2)
            {
                Console.WriteLine("Player at: " + player1.getX() + ", " + player1.getY());
                output2 = true;
            }
            if (player1 != null)
            {
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
                    enemy.Draw(c);
                }
            }
        }
    }
}

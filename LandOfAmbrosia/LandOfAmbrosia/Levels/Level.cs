using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LandOfAmbrosia.Levels
{
    class Level
    {
        private const int DEFAULT_WIDTH = 10;
        private const int DEFAULT_HEIGHT = 5;
        private const int DEFAULT_SEED = 42;

        private Model[] possibleTileModels;
        
        int width, height;

        Random gen;

        public Tile[,] tiles;

        //TODO this might not be a whole skybox, just a background image since the game is a side scroller
        //private Skybox skybox;

        /// <summary>
        /// Creates a new, randomly generated level with the default height, width, and seed
        /// </summary>
        public Level() : this(DEFAULT_WIDTH, DEFAULT_HEIGHT, DEFAULT_SEED)
        {
        }

        /// <summary>
        /// Constructs a new Level with the default width and height and the given seed
        /// </summary>
        /// <param name="seed"></param>
        public Level(int seed)
            : this(DEFAULT_WIDTH, DEFAULT_HEIGHT, seed)
        {
        }

        /// <summary>
        /// Creates a new Level with the given width and height and the default seed
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Level(int width, int height)
            : this(width, height, DEFAULT_SEED)
        {
        }

        /// <summary>
        /// Creates a new randomly generated level with the given width, height, and seed
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Level(int width, int height, int seed)
        {
            tiles = new Tile[width, height];
            this.width = width;
            this.height = height;
            gen = new Random(seed);

            this.GenerateLevel();
            //this.SetUpSkybox();
        }

        private void GenerateLevel()
        {
            int numNonEmptySpaces = gen.Next(width * height / 2);

            for (int i = 0; i < numNonEmptySpaces; ++i)
            {
                Vector2 addLoc = this.GetRandomTileLocation();
                tiles[(int)addLoc.X, (int) addLoc.Y] = this.GetRandomTile();
            }
        }

        private Vector2 GetRandomTileLocation()
        {
            return new Vector2(gen.Next(width), gen.Next(height));
        }

        private Tile GetRandomTile()
        {
            //TODO
            return null;
            //int tileType = gen.Next(numTileTypes);
            
        }

        private void SetUpSkybox()
        {
            throw new NotImplementedException();
        }

        public void SetTile(int width, int height, Tile newTile)
        {
            if (width < 0 || width > this.width || height < 0 || height > this.height)
            {
                throw new IndexOutOfRangeException();
            }
            tiles[width, height] = newTile; 
        }

        public void Draw(Camera c)
        {
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    tiles[i, j].Draw(c);
                }
            }
        }
    }
}

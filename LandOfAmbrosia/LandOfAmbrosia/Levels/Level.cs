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
        
        public int width, height;

        public Tile[,] tiles;

        //Made this a whole skybox cause I already had skybox code
        public Skybox skybox;

        /// <summary>
        /// Constructs a new Level with the default width and height and the given seed
        /// </summary>
        /// <param name="seed"></param>
        public Level()
            : this(DEFAULT_WIDTH, DEFAULT_HEIGHT)
        {
        }

        /// <summary>
        /// Creates an empty Level with the given height, width, and seed
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Level(int width, int height)
        {
            tiles = new Tile[width, height];
            this.width = width;
            this.height = height;
        }

        private void SetUpSkybox()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the tile in position tiles[width, height] to the newTile
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="newTile"></param>
        public void SetTile(int width, int height, Tile newTile)
        {
            if (width < 0 || width > this.width || height < 0 || height > this.height)
            {
                throw new IndexOutOfRangeException();
            }
            tiles[width, height] = newTile; 
        }

        public void Draw(CameraComponent c, GraphicsDevice device)
        {
            skybox.Draw(c, device);

            this.DrawTiles(c);
        }

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
    }
}

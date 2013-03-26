using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandOfAmbrosia.Levels
{
    class Level
    {
        private const int DEFAULT_WIDTH = 100;
        private const int DEFAULT_HEIGHT = 20;
        
        int width, height;

        public Tile[,] tiles;
        //private Skybox skybox;

        /// <summary>
        /// Creates a new level with the default height and width
        /// </summary>
        public Level() : this(DEFAULT_WIDTH, DEFAULT_HEIGHT)
        {
        }

        /// <summary>
        /// Creates an empty level with the given width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Level(int width, int height)
        {
            tiles = new Tile[width, height];
            this.SetUpSkybox();
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
            
        }
    }
}

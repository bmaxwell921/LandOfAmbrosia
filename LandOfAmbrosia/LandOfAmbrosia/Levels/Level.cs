using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LandOfAmbrosia.Logic;
using LandOfAmbrosia.Common;

namespace LandOfAmbrosia.Levels
{
    /// <summary>
    /// I think the level is the one who holds the enemies and characters
    /// </summary>
    class Level
    {     
        public int width, height;

        public Tile[,] tiles;

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

        private void FillFloor()
        {
            for (int i = 0; i < width; ++i)
            {
                SetTile(i, 1, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), Constants.ConvertToXNAScene(new Vector3(i, 1, 0))));
                SetTile(i, 0, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), Constants.ConvertToXNAScene(new Vector3(i, 0, 0))));
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
            if (width < 0 || width > this.width || height < 0 || height > this.height)
            {
                throw new IndexOutOfRangeException();
            }
            tiles[width, height] = newTile; 
        }

        //TODO how to handle collision detection?
        public int XPosToTileX(float xPos)
        {
            throw new NotImplementedException();
        }

        public int YPosToTileY(float yPos)
        {
            throw new NotImplementedException();
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

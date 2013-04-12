using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Levels;
using LandOfAmbrosia.Common;
using Microsoft.Xna.Framework;

namespace LandOfAmbrosia.Logic
{
    class LevelGenerator
    {
        private enum ChunkType { FLOOR, STAIRS, FLOATING_PLATFORMS, MOUNTAIN_LEFT, MOUNTAIN_RIGHT, EMPTY }
        /// <summary>
        /// Creates a new, randomly generated level with the given width, height, and seed
        /// 
        /// Precondition: The map size much divide evenly into 8x8 chunks
        /// </summary>
        /// <param name="width">The width of the map</param>
        /// <param name="height">The height of the map</param>
        /// <param name="seed">The seed used for random generation</param>
        /// <returns>The level that was randomly created</returns>
        public static Level GenerateNewLevel(int width, int height, int seed)
        {
            Level ret = new Level(width, height);
            FillLevel(ret, seed);
            return ret;
        }

        public static void FillLevel(Level level, int seed)
        {
            Random gen = new Random(seed);
            //This should hold an array of the chunks, which will tell what chunks are all around the current chunk we are creating
            //Fill the left and right sides at the end
            ChunkType[,] chunks;
            PrepareGeneration(level, out chunks);
            //0,0 is the bottom left!!!

            for (int i = 0; i < level.width / Constants.CHUNK_SIZE; ++i)
            {
                for (int j = 0; j < level.height / Constants.CHUNK_SIZE; ++j)
                {
                    FillChunk(level, chunks, gen);
                }
            }
        }

        private static void PrepareGeneration(Level level, out ChunkType[,] chunks)
        {
            chunks = new ChunkType[level.width / Constants.CHUNK_SIZE, level.height / Constants.CHUNK_SIZE];
            //Clear everything out
            for (int i = 0; i < level.width; ++i)
            {
                for (int j = 0; j < level.height; ++j)
                {
                    level.SetTile(i, j, null);
                    chunks[i / Constants.CHUNK_SIZE, j / Constants.CHUNK_SIZE] = ChunkType.EMPTY;
                }
            }

            //Then fill the floor
            for (int i = 0; i < level.width; ++i)
            {
                level.SetTile(i, 0, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), new Vector3(i, 0, 0)));
            }
        }

        private static void FillChunk(Level level, ChunkType[,] chunks, Random gen)
        {
            //First, we need to check the chunks to the left and bottom to see if they bind what we need to choose
                //If they do, then choose the chunk that takes care of the most binding chunk
            //Otherwise, randomly choose a chunktype
            //Fill in the level and the chunks array to reflect what was chosen
            
        }
    }
}

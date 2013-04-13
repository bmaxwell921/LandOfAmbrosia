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
        private static Random gen;

        private enum ChunkType { FLOOR, STAIRS, FLOATING_PLATFORMS_SAFE, FLOATING_PLATFORMS_NOT_SAFE, MOUNTAIN_LEFT, MOUNTAIN_RIGHT, JAGGIES_LEFT, JAGGIES_RIGHT, TALL_GROUND, EMPTY }
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
            gen = new Random(seed);
            //This should hold an array of the chunks, which will tell what chunks are all around the current chunk we are creating
            //Fill the left and right sides at the end
            ChunkType[,] chunks;
            PrepareGeneration(level, out chunks);
            //0,0 is the bottom left!!!

            for (int i = 1; i < level.width / Constants.CHUNK_SIZE; ++i)
            {
                for (int j = 0; j < level.height / Constants.CHUNK_SIZE; ++j)
                {
                    GenerateChunk(level, chunks, new Vector2(i, j));
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

            //Fill the floor of the First chunk to be safe
            FillFloor(level, new Vector2(0, 0));
        }

        private static void GenerateChunk(Level level, ChunkType[,] chunks, Vector2 chunkLocation)
        {
            //First, we need to check the chunks to the left and bottom to see if they bind what we need to choose
                //If they do, then choose the chunk that takes care of the most binding chunk
            ChunkType chunkType = ChooseChunkType(chunks, chunkLocation);
            //Otherwise, randomly choose a chunktype
            //Fill in the level and the chunks array to reflect what was chosen
            FillChunkWith(level, chunkLocation * Constants.CHUNK_SIZE, chunkType);
            chunks[(int)chunkLocation.X, (int)chunkLocation.Y] = chunkType;
        }

        private static ChunkType ChooseChunkType(ChunkType[,] chunks, Vector2 chunkLoc)
        {
            //Checks to see if there are any bounds on the chunk type we need to generate and lets us choose randomly from the given stuff
            IList<ChunkType> possibleChunks = GetPossibleChunks(chunks, chunkLoc);

            return possibleChunks[gen.Next(possibleChunks.Count)];
        }

        private static IList<ChunkType> GetPossibleChunks(ChunkType[,] chunks, Vector2 chunkLoc)
        {
            IList<ChunkType> possibilities = new List<ChunkType>();

            //only these 4 chunk types are ok to choose at any time
            possibilities.Add(ChunkType.FLOATING_PLATFORMS_SAFE);
            possibilities.Add(ChunkType.JAGGIES_LEFT);
            possibilities.Add(ChunkType.JAGGIES_RIGHT);

            if (chunkLoc.X - 1 < 0)
            {
                return possibilities;
            }

            ChunkType left = chunks[(int)chunkLoc.X - 1, (int) chunkLoc.Y];
            //We can only choose from chunk types that let us move backward or forward, depending on which chunk type was to our left
            if (EndsAtTop(left))
            {
                possibilities.Add(ChunkType.MOUNTAIN_RIGHT);
                possibilities.Add(ChunkType.TALL_GROUND);
            }
            else
            {
                possibilities.Add(ChunkType.FLOOR);
                possibilities.Add(ChunkType.STAIRS);
                possibilities.Add(ChunkType.MOUNTAIN_LEFT);
            }

            //Unsafe floating is the only one that is a special case since it generates randomly
            if (left != ChunkType.FLOATING_PLATFORMS_NOT_SAFE)
            {
                possibilities.Add(ChunkType.FLOATING_PLATFORMS_NOT_SAFE);
            }
            return possibilities;
        }

        private static bool EndsAtTop(ChunkType leftTile)
        {
            return leftTile == ChunkType.MOUNTAIN_LEFT || leftTile == ChunkType.TALL_GROUND;
        }

        private static void FillChunkWith(Level level, Vector2 bottomLeftLoc, ChunkType chunkType)
        {
            if (chunkType == ChunkType.FLOATING_PLATFORMS_SAFE)
            {
                FillFloatingPlatformsSafe(level, bottomLeftLoc);
            }
            else if (chunkType == ChunkType.FLOATING_PLATFORMS_NOT_SAFE)
            {
                FillFloatingPlatformsNotSafe(level, bottomLeftLoc);
            }
            else if (chunkType == ChunkType.FLOOR)
            {
                FillFloor(level, bottomLeftLoc);
            }
            else if (chunkType == ChunkType.MOUNTAIN_LEFT)
            {
                FillMountainLeft(level, bottomLeftLoc);
            }
            else if (chunkType == ChunkType.MOUNTAIN_RIGHT)
            {
                FillMountainRight(level, bottomLeftLoc);
            }
            else if (chunkType == ChunkType.STAIRS)
            {
                FillStairs(level, bottomLeftLoc);
            }
            else if (chunkType == ChunkType.TALL_GROUND)
            {
                FillTallGround(level, bottomLeftLoc);
            }
            else if (chunkType == ChunkType.JAGGIES_LEFT)
            {
                FillJaggiesLeft(level, bottomLeftLoc);
            }
            else if (chunkType == ChunkType.JAGGIES_RIGHT)
            {
                FillJaggiesRight(level, bottomLeftLoc);
            }
            else
            {
                //Empty
                return;
            }
        }

        /*
         * Ends up with a Chunk like this:
         *        XX
         * XX
         *     XX
         *   XX
         *       XX
         * XX
         *      XX
         * XXXXXXXX
         * 
         * The bottom is filled, then for each layer higher, a location is chosen that is 2-3
         * blocks away from the platform lower than it
         */
        private static void FillFloatingPlatformsSafe(Level level, Vector2 bottomLeft)
        {
            FillFloor(level, bottomLeft);
            FillFloatingPlatformsNotSafe(level, bottomLeft);
        }

        /*
         * Ends up with a Chunk like this:
         *        XX
         * XX
         *     XX
         *   XX
         *       XX
         * XX
         *      XX
         * <Empty>
         */
        private static void FillFloatingPlatformsNotSafe(Level level, Vector2 bottomLeft)
        {
            //Start out with a platform somewhere in the chunk
            //Minus 2 so we don't go out of bounds on our chunk
            int startX = (int)(gen.Next(Constants.CHUNK_SIZE - 2));
            
            //Left-most tile, compensating for where we are in the chunk
            level.SetTile(startX + (int) bottomLeft.X, (int)(bottomLeft.Y), new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), 
                new Vector3((startX + bottomLeft.X) * Constants.TILE_SIZE, (bottomLeft.Y) * Constants.TILE_SIZE, 0)));
            //Right-most tile
            level.SetTile(startX + 1 + (int)bottomLeft.X, (int)(bottomLeft.Y), new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR),
                new Vector3((startX + 1 + bottomLeft.X) * Constants.TILE_SIZE, (bottomLeft.Y) * Constants.TILE_SIZE, 0)));

            int lastStart = startX;
            for (int i = 2; i < Constants.CHUNK_SIZE; i+=2)
            {
                //Get a random starting location that is 2-3 blocks left or right of the last starting loc
                bool goRight = false;
                if (gen.NextDouble() < 0.5)
                {
                    goRight = true;
                }

                //Adjust the generated value by 1 if we are going right bc everything is in terms of the left start
                int dist = (int)gen.Next(2, 3) + ((goRight) ? 1 : 0);
                int newX = lastStart + ((goRight) ? dist : -dist);

                //Minus 2 again here, -1 for the right block and -2 for the left block
                newX = ((goRight) ? ((newX >= Constants.CHUNK_SIZE) ? Constants.CHUNK_SIZE - 2 : newX) : ((newX < 0) ? 0 : newX));

                level.SetTile(newX + (int)bottomLeft.X, (int)(bottomLeft.Y + i), new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR),
                new Vector3((newX + (int) bottomLeft.X) * Constants.TILE_SIZE, (bottomLeft.Y + i) * Constants.TILE_SIZE, 0)));
                //Right-most tile
                level.SetTile(newX + 1 + (int)bottomLeft.X, (int)(bottomLeft.Y + i), new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR),
                    new Vector3((newX + 1 + bottomLeft.X) * Constants.TILE_SIZE, (bottomLeft.Y + i) * Constants.TILE_SIZE, 0)));

                lastStart = newX;
            }
        }

        /*
         * Ends up with a Chunk like this:
         * 
         * XXXXXXXX
         */
        private static void FillFloor(Level level, Vector2 bottomLeftLoc)
        {
            for (int i = 0; i < Constants.CHUNK_SIZE; ++i)
            {
                level.SetTile((int)(i + bottomLeftLoc.X), (int)bottomLeftLoc.Y, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), 
                    new Vector3((i + bottomLeftLoc.X) * Constants.TILE_SIZE, bottomLeftLoc.Y * Constants.TILE_SIZE, 0)));
            }
        }

        /*
         * Ends up with Chunk like this:
         *  <Empty>
         *        X
         *       XX
         *      XXX
         *     XXXX
         *    XXXXX
         *   XXXXXX
         *  XXXXXXX
         */
        private static void FillMountainLeft(Level level, Vector2 bottomLeftLoc)
        {
            FillMountainCommon(level, bottomLeftLoc, true);
        }

        /*
         * Ends up with a Chunk like this:
         * <Empty>
         * X
         * XX
         * XXX
         * XXXX
         * XXXXX
         * XXXXXX
         * XXXXXXX
         */
        private static void FillMountainRight(Level level, Vector2 bottomLeftLoc)
        {
            FillMountainCommon(level, bottomLeftLoc, false);
        }

        private static void FillMountainCommon(Level level, Vector2 bottomLeftLoc, bool isLeft)
        {
            for (int i = 0; i < Constants.CHUNK_SIZE; ++i)
            {
                for (int j = 0; j < i; ++j)
                {
                    int xPos = (isLeft) ? i : Constants.CHUNK_SIZE - 1 - i;
                    level.SetTile(xPos + (int)bottomLeftLoc.X, j + (int)bottomLeftLoc.Y, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR),
                        new Vector3((xPos + bottomLeftLoc.X) * Constants.TILE_SIZE, (j + bottomLeftLoc.Y) * Constants.TILE_SIZE, 0))); 
                }
            }
            FillFloor(level, bottomLeftLoc);
        }

        /*
         * Ends up with a Chunk like this:
         * 
         *    XX
         *   XXXX
         *  XXXXXX
         * XXXXXXXX
         * 
         * Anything not shown is an empty space
         */
        private static void FillStairs(Level level, Vector2 bottomLeftLoc)
        {
            for (int i = 0; i < Constants.CHUNK_SIZE / 2; ++i)
            {
                for (int j = 0; j < i + 1; ++j)
                {
                    //Left side of the stairs
                    level.SetTile((int)bottomLeftLoc.X + i, (int)bottomLeftLoc.Y + j, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR), 
                        new Vector3((bottomLeftLoc.X + i) * Constants.TILE_SIZE, (bottomLeftLoc.Y + j) * Constants.TILE_SIZE, 0))); 

                    //Right side of the stairs
                    //Vector2 rightSide = new Vector2((bottomLeftLoc.X + (Constants.CHUNK_SIZE - 1 - i)) * Constants.CHUNK_SIZE, (bottomLeftLoc.Y + j) * Constants.TILE_SIZE);
                    level.SetTile((int)bottomLeftLoc.X + (Constants.CHUNK_SIZE - 1 - i), (int)bottomLeftLoc.Y + j, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR),
                        new Vector3((bottomLeftLoc.X + (Constants.CHUNK_SIZE - 1 - i)) * Constants.TILE_SIZE, (bottomLeftLoc.Y + j) * Constants.TILE_SIZE, 0)));
                }
            }
        }

        /*
         * Creates a Chunk like this:
         * <Empty>
         * <Empty>
         * XXXXXXXX
         * XXXXXXXX
         * XXXXXXXX
         * XXXXXXXX
         * XXXXXXXX
         * XXXXXXXX
         */
        private static void FillTallGround(Level level, Vector2 bottomLeftLoc)
        {
            for (int i = 0; i < Constants.CHUNK_SIZE; ++i)
            {
                for (int j = 0; j <= 6; ++j)
                {
                    level.SetTile((int)(i + bottomLeftLoc.X), j + (int)bottomLeftLoc.Y, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR),
                    new Vector3((i + bottomLeftLoc.X) * Constants.TILE_SIZE, (j + bottomLeftLoc.Y) * Constants.TILE_SIZE, 0)));
                }
            }
        }

        /*
         * Creates a Chunk like this
         * 
         * XXXX
         * 
         *     XXXX
         * 
         * XXXX
         * 
         * XXXXXXXX
         */
        private static void FillJaggiesLeft(Level level, Vector2 bottomLeftLoc)
        {
            FillJaggiesCommon(level, bottomLeftLoc, true);
        }

        /*
         * Creates a Chunk like this:
         * 
         *     XXXX
         * 
         * XXXX
         * 
         *     XXXX
         * 
         * XXXXXXXX
         */
        private static void FillJaggiesRight(Level level, Vector2 bottomLeftLoc)
        {
            FillJaggiesCommon(level, bottomLeftLoc, false);
        }

        private static void FillJaggiesCommon(Level level, Vector2 bottomLeftLoc, bool isLeft)
        {
            bool fromLeft = isLeft;
            for (int i = 0; i < Constants.CHUNK_SIZE; i += 2)
            {
                //J is the x Position
                for (int j = 0; j < Constants.CHUNK_SIZE / 2; ++j)
                {
                    int xPos = (fromLeft) ? j : Constants.CHUNK_SIZE - 1 - j;
                    level.SetTile(xPos + (int)bottomLeftLoc.X, i + (int) bottomLeftLoc.Y, new Tile(AssetUtil.GetTileModel(Constants.PLATFORM_CHAR),
                        new Vector3((xPos + (int)bottomLeftLoc.X) * Constants.TILE_SIZE, (i + (int)bottomLeftLoc.Y) * Constants.TILE_SIZE, 0)));

                }

                fromLeft = !fromLeft;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Levels;
using LandOfAmbrosia.Common;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Characters;

namespace LandOfAmbrosia.Logic
{
    class LevelGenerator
    {
        private Random gen;
        private LevelInfo currentLevelInfo;
        private Level curLevel;
        private ChunkType[,] chunks;
        private IList<LevelInfo> levelInfos;

        //If this is null, choose randomly what chunks to generate, otherwise use the chunks this says
        private ChunkType[,] chunksToChoose;

        public LevelGenerator(Level workingLevel, IList<LevelInfo> lis, int seed) : this(workingLevel, lis, seed, null)
        {
            
        }

        public LevelGenerator(Level workingLevel, IList<LevelInfo> lis, int seed, ChunkType[,] chunksToChoose)
        {
            curLevel = workingLevel;
            this.gen = new Random(seed);
            this.levelInfos = lis;
            this.chunksToChoose = chunksToChoose;
        }
        /// <summary>
        /// Creates a new, randomly generated level with the given width, height, and seed
        /// 
        /// Precondition: The map size much divide evenly into 8x8 chunks
        /// </summary>
        /// <returns>The level that was randomly created</returns>
        public Level GenerateNewLevel(int level, int numPlayers)
        {
            currentLevelInfo = levelInfos[level];
            //this.curLevel = new Level(currentLevelInfo.width, currentLevelInfo.height, numPlayers);
            this.curLevel.updateLevelTo(currentLevelInfo);
            this.chunks = new ChunkType[curLevel.width / Constants.CHUNK_SIZE, curLevel.height / Constants.CHUNK_SIZE];
            FillLevel();
            return curLevel;
        }

        public void FillLevel()
        {
            //This should hold an array of the chunks, which will tell what chunks are all around the current chunk we are creating
            //Fill the left and right sides at the end
            PrepareGeneration();
            
            for (int i = chunksToChoose == null ? 1 : 0; i < curLevel.width / Constants.CHUNK_SIZE; ++i)
            {
                for (int j = 0; j < curLevel.height / Constants.CHUNK_SIZE; ++j)
                {
                    GenerateChunk(new Vector2(i, j));
                }
            }

            FillInMinions();   
        }

        private void FillInMinions()
        {
            //First find all the "safe blocks": mean blocks that are on top of a platform
            IList<Vector2> safes = new List<Vector2>();
            for (int i = 0; i < curLevel.width; ++i)
            {
                for (int j = 0; j < curLevel.height; ++j)
                {
                    // If this is a platform, check the spot above
                    if (i >= 8 && curLevel.GetTile(i, j) != null)
                    {
                        //If it's off the bounds of the level it's safe and if it's not a platform it is also safe
                        if (curLevel.isOB(new Vector2(i, j + 1)) || curLevel.GetTile(i, j + 1) == null)
                        {
                            safes.Add(new Vector2(i, j + 1));
                        }
                    }
                }
            }

            for (int i = 0; i < currentLevelInfo.numEnemies; ++i)
            {
                int index = gen.Next(safes.Count);
                Vector2 addLoc = safes[index];
                curLevel.enemies.Add(new Minion(curLevel, AssetUtil.GetEnemyModel(Constants.MINION_CHAR),
                        new Vector3(addLoc.X * Constants.TILE_SIZE, addLoc.Y * Constants.TILE_SIZE, 2 * Constants.CHARACTER_DEPTH), curLevel.players, currentLevelInfo));

            }
        }

        private void PrepareGeneration()
        {
            //Clear everything out
            for (int i = 0; i < curLevel.width; ++i)
            {
                for (int j = 0; j < curLevel.height; ++j)
                {
                    curLevel.SetTile(i, j, null);
                    chunks[i / Constants.CHUNK_SIZE, j / Constants.CHUNK_SIZE] = ChunkType.EMPTY;
                }
            }

            //Fill the floor of the First chunk to be safe
            FillFloor(new Vector2(0, 0));
        }

        private void GenerateChunk(Vector2 chunkLocation)
        {
            //First, we need to check the chunks to the left and bottom to see if they bind what we need to choose
            //If they do, then choose the chunk that takes care of the most binding chunk
            ChunkType chunkType = chunksToChoose == null ? ChooseChunkType(chunkLocation) : chunksToChoose[(int)chunkLocation.X, (int)chunkLocation.Y];
            //Otherwise, randomly choose a chunktype
            //Fill in the level and the chunks array to reflect what was chosen
            FillChunkWith(chunkLocation * Constants.CHUNK_SIZE, chunkType);
            chunks[(int)chunkLocation.X, (int)chunkLocation.Y] = chunkType;
        }

        private ChunkType ChooseChunkType(Vector2 chunkLoc)
        {
            //Checks to see if there are any bounds on the chunk type we need to generate and lets us choose randomly from the given stuff
            IList<ChunkType> possibleChunks = GetPossibleChunks(chunkLoc);

            return possibleChunks[gen.Next(possibleChunks.Count)];
        }

        private IList<ChunkType> GetPossibleChunks(Vector2 chunkLoc)
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

            ChunkType left = chunks[(int)chunkLoc.X - 1, (int)chunkLoc.Y];
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

        private bool EndsAtTop(ChunkType leftTile)
        {
            return leftTile == ChunkType.MOUNTAIN_LEFT || leftTile == ChunkType.TALL_GROUND;
        }

        private void FillChunkWith(Vector2 bottomLeftLoc, ChunkType chunkType)
        {
            if (chunkType == ChunkType.FLOATING_PLATFORMS_SAFE)
            {
                FillFloatingPlatformsSafe(bottomLeftLoc);
            }
            else if (chunkType == ChunkType.FLOATING_PLATFORMS_NOT_SAFE)
            {
                FillFloatingPlatformsNotSafe(bottomLeftLoc);
            }
            else if (chunkType == ChunkType.FLOOR)
            {
                FillFloor(bottomLeftLoc);
            }
            else if (chunkType == ChunkType.MOUNTAIN_LEFT)
            {
                FillMountainLeft(bottomLeftLoc);
            }
            else if (chunkType == ChunkType.MOUNTAIN_RIGHT)
            {
                FillMountainRight(bottomLeftLoc);
            }
            else if (chunkType == ChunkType.STAIRS)
            {
                FillStairs(bottomLeftLoc);
            }
            else if (chunkType == ChunkType.TALL_GROUND)
            {
                FillTallGround(bottomLeftLoc);
            }
            else if (chunkType == ChunkType.JAGGIES_LEFT)
            {
                FillJaggiesLeft(bottomLeftLoc);
            }
            else if (chunkType == ChunkType.JAGGIES_RIGHT)
            {
                FillJaggiesRight(bottomLeftLoc);
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
        private void FillFloatingPlatformsSafe(Vector2 bottomLeft)
        {
            FillFloor(bottomLeft);
            FillFloatingPlatformsNotSafe(bottomLeft);
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
        private void FillFloatingPlatformsNotSafe(Vector2 bottomLeft)
        {
            //Start out with a platform somewhere in the chunk
            //Minus 2 so we don't go out of bounds on our chunk
            int startX = (int)(gen.Next(Constants.CHUNK_SIZE - 2));
            
            //Left-most tile, compensating for where we are in the chunk
            curLevel.SetTile(startX + (int) bottomLeft.X, (int)(bottomLeft.Y), new Tile(AssetUtil.GetTileModel(currentLevelInfo.platformType), 
                new Vector3((startX + bottomLeft.X) * Constants.TILE_SIZE, (bottomLeft.Y) * Constants.TILE_SIZE, 0)));
            //Right-most tile
            curLevel.SetTile(startX + 1 + (int)bottomLeft.X, (int)(bottomLeft.Y), new Tile(AssetUtil.GetTileModel(currentLevelInfo.platformType),
                new Vector3((startX + 1 + bottomLeft.X) * Constants.TILE_SIZE, (bottomLeft.Y) * Constants.TILE_SIZE, 0)));

            bool lastWasLeft = startX < Constants.CHUNK_SIZE / 2;
            for (int i = 2; i < Constants.CHUNK_SIZE; i+=2)
            {
                //If last time we went to the left, choose a spot on the right and vice versa
                int newX = (lastWasLeft) ? gen.Next(4, 6) : gen.Next(0, 3);

                curLevel.SetTile(newX + (int)bottomLeft.X, (int)(bottomLeft.Y + i), new Tile(AssetUtil.GetTileModel(currentLevelInfo.platformType),
                new Vector3((newX + (int) bottomLeft.X) * Constants.TILE_SIZE, (bottomLeft.Y + i) * Constants.TILE_SIZE, 0)));
                //Right-most tile
                curLevel.SetTile(newX + 1 + (int)bottomLeft.X, (int)(bottomLeft.Y + i), new Tile(AssetUtil.GetTileModel(currentLevelInfo.platformType),
                    new Vector3((newX + 1 + bottomLeft.X) * Constants.TILE_SIZE, (bottomLeft.Y + i) * Constants.TILE_SIZE, 0)));

                lastWasLeft = !lastWasLeft;
            }
        }

        /*
         * Ends up with a Chunk like this:
         * 
         * XXXXXXXX
         */
        private void FillFloor(Vector2 bottomLeftLoc)
        {
            for (int i = 0; i < Constants.CHUNK_SIZE; ++i)
            {
                curLevel.SetTile((int)(i + bottomLeftLoc.X), (int)bottomLeftLoc.Y, new Tile(AssetUtil.GetTileModel(currentLevelInfo.platformType), 
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
        private void FillMountainLeft(Vector2 bottomLeftLoc)
        {
            FillMountainCommon(bottomLeftLoc, true);
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
        private void FillMountainRight(Vector2 bottomLeftLoc)
        {
            FillMountainCommon(bottomLeftLoc, false);
        }

        private void FillMountainCommon(Vector2 bottomLeftLoc, bool isLeft)
        {
            for (int i = 0; i < Constants.CHUNK_SIZE; ++i)
            {
                for (int j = 0; j < i; ++j)
                {
                    int xPos = (isLeft) ? i : Constants.CHUNK_SIZE - 1 - i;
                    curLevel.SetTile(xPos + (int)bottomLeftLoc.X, j + (int)bottomLeftLoc.Y, new Tile(AssetUtil.GetTileModel(currentLevelInfo.platformType),
                        new Vector3((xPos + bottomLeftLoc.X) * Constants.TILE_SIZE, (j + bottomLeftLoc.Y) * Constants.TILE_SIZE, 0))); 
                }
            }
            FillFloor(bottomLeftLoc);
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
        private void FillStairs(Vector2 bottomLeftLoc)
        {
            for (int i = 0; i < Constants.CHUNK_SIZE / 2; ++i)
            {
                for (int j = 0; j < i + 1; ++j)
                {
                    //Left side of the stairs
                    curLevel.SetTile((int)bottomLeftLoc.X + i, (int)bottomLeftLoc.Y + j, new Tile(AssetUtil.GetTileModel(currentLevelInfo.platformType), 
                        new Vector3((bottomLeftLoc.X + i) * Constants.TILE_SIZE, (bottomLeftLoc.Y + j) * Constants.TILE_SIZE, 0))); 

                    //Right side of the stairs
                    //Vector2 rightSide = new Vector2((bottomLeftLoc.X + (Constants.CHUNK_SIZE - 1 - i)) * Constants.CHUNK_SIZE, (bottomLeftLoc.Y + j) * Constants.TILE_SIZE);
                    curLevel.SetTile((int)bottomLeftLoc.X + (Constants.CHUNK_SIZE - 1 - i), (int)bottomLeftLoc.Y + j, new Tile(AssetUtil.GetTileModel(currentLevelInfo.platformType),
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
        private void FillTallGround(Vector2 bottomLeftLoc)
        {
            for (int i = 0; i < Constants.CHUNK_SIZE; ++i)
            {
                for (int j = 0; j <= 6; ++j)
                {
                    curLevel.SetTile((int)(i + bottomLeftLoc.X), j + (int)bottomLeftLoc.Y, new Tile(AssetUtil.GetTileModel(currentLevelInfo.platformType),
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
        private void FillJaggiesLeft(Vector2 bottomLeftLoc)
        {
            FillJaggiesCommon(bottomLeftLoc, true);
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
        private void FillJaggiesRight(Vector2 bottomLeftLoc)
        {
            FillJaggiesCommon(bottomLeftLoc, false);
        }

        private void FillJaggiesCommon(Vector2 bottomLeftLoc, bool isLeft)
        {
            bool fromLeft = isLeft;
            for (int i = 0; i < Constants.CHUNK_SIZE; i += 2)
            {
                //J is the x Position
                for (int j = 0; j < Constants.CHUNK_SIZE / 2; ++j)
                {
                    int xPos = (fromLeft) ? j : Constants.CHUNK_SIZE - 1 - j;
                    curLevel.SetTile(xPos + (int)bottomLeftLoc.X, i + (int)bottomLeftLoc.Y, new Tile(AssetUtil.GetTileModel(currentLevelInfo.platformType),
                        new Vector3((xPos + (int)bottomLeftLoc.X) * Constants.TILE_SIZE, (i + (int)bottomLeftLoc.Y) * Constants.TILE_SIZE, 0)));
                }

                fromLeft = !fromLeft;
            }
        }
    }
}

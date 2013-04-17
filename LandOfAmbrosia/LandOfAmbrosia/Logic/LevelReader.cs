using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Levels;
using LandOfAmbrosia.Common;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Characters;

namespace LandOfAmbrosia.Logic
{
    /// <summary>
    /// Util to read a file to create a level
    /// </summary>
    class LevelReader
    {
        public static String COMMENT = "#";

        public static Tile[,] readLevel(String filePath, out int width, out int height, out IList<Character> players, out IList<Character> enemies)
        {
            /*
             * First we read the whole level into a list so we can know
             * how long and how wide to make the tiles array. Then we go
             * thru that list to fill in the tile array
            */
            IList<String> stringLevel = new List<String>();
            TextReader scanner = new StreamReader(filePath);
            int maxWidth = 0;
            String readLine = null;
            while ((readLine = scanner.ReadLine()) != null)
            {
                //We ignore comments when creating the level
                if (!readLine.StartsWith(COMMENT) && (readLine.Count() != 0))
                {
                    stringLevel.Add(readLine);
                    maxWidth = Math.Max(maxWidth, readLine.Count());
                }
            }

            Tile[,] tiles = new Tile[maxWidth, stringLevel.Count];
            enemies = new List<Character>();
            players = new List<Character>();

            for (int i = 0; i < stringLevel.Count; ++i )
            {
                String line = stringLevel.ElementAt(i);
                for (int j = 0; j < line.Length; ++j)
                {
                    if (line.ElementAt(j) == 'P' || line.ElementAt(j) == 'p')
                    {
                        tiles[i, j] = new Tile(AssetUtil.GetTileModel(line.ElementAt(j)), Constants.ConvertToXNAScene(new Vector3(i * Constants.TILE_SIZE, j * Constants.TILE_SIZE, 0)));
                    }
                    else if (line.ElementAt(j) == 'm' || line.ElementAt(j) == 'M')
                    {
                        enemies.Add(new Minion(AssetUtil.GetEnemyModel(Constants.MINION_CHAR), 
                            new Vector3(j * Constants.TILE_SIZE, i * Constants.TILE_SIZE, Constants.CHARACTER_DEPTH), players));
                    }
                    else if (line.ElementAt(j) == '1')
                    {
                        players.Add(new UserControlledCharacter(Constants.PLAYER1_CHAR, AssetUtil.GetPlayerModel(Constants.PLAYER1_CHAR), 
                            new Vector3(j * Constants.TILE_SIZE, i * Constants.TILE_SIZE, Constants.CHARACTER_DEPTH)));
                    }
                    else if (line.ElementAt(j) == '2')
                    {
                        players.Add(new UserControlledCharacter(Constants.PLAYER2_CHAR, AssetUtil.GetPlayerModel(Constants.PLAYER2_CHAR), 
                            new Vector3(j * Constants.TILE_SIZE, i * Constants.TILE_SIZE, Constants.CHARACTER_DEPTH)));
                    }
                    else
                    {
                        tiles[i, j] = null;
                    }
                }
            }

            width = maxWidth;
            height = stringLevel.Count;

            return tiles;
        }
    }
}

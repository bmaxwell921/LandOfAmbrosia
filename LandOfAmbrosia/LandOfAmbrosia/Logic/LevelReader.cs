using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Levels;
using LandOfAmbrosia.Common;
using Microsoft.Xna.Framework;

namespace LandOfAmbrosia.Logic
{
    /// <summary>
    /// Util to read a file to create a level
    /// </summary>
    class LevelReader
    {
        public static String COMMENT = "#";

        public static Tile[,] readLevel(String filePath, out int width, out int height)
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

            for (int i = 0; i < stringLevel.Count; ++i )
            {
                String line = stringLevel.ElementAt(i);
                for (int j = 0; j < line.Length; ++j)
                {
                    tiles[j, i] = new Tile(AssetUtil.GetTileModel(line.ElementAt(j)), Constants.ConvertToXNAScene(new Vector3(j, i, 0)));
                }
            }

            width = maxWidth;
            height = stringLevel.Count;

            return tiles;
        }
    }
}

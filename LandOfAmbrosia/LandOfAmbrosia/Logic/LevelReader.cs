using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Levels;

namespace LandOfAmbrosia.Logic
{
    /// <summary>
    /// Util to read a file to create a level
    /// </summary>
    class LevelReader
    {
        public static String COMMENT = "#";

        public static Tile[,] readLevel(String filePath)
        {
            /*
             * First we read the whole level into a list so we can know
             * how long and how wide to make the tiles array. Then we go
             * thru that list to fill in the tile array
            */
            IList<String> stringLevel = new List<String>();
            TextReader scanner = new StreamReader(filePath);

            String readLine = scanner.ReadLine();
            if (readLine == null)
            {
                return null;
            }

            int maxWidth = readLine.Length;
            while ((readLine = scanner.ReadLine()) != null)
            {
                //We ignore comments when creating the level
                if (!readLine.StartsWith(COMMENT))
                {
                    stringLevel.Add(readLine);
                    Math.Max(maxWidth, readLine.Count());
                }
            }

            Tile[,] tiles = new Tile[maxWidth, stringLevel.Count];

            for (int i = 0; i < stringLevel.Count; ++i )
            {
                String line = stringLevel.ElementAt(i);
                for (int j = 0; j < line.Length; ++j)
                {
                    tiles[i, j] = ParseCharacter(line.ElementAt(j));
                }
            }

            return tiles;
        }

        public static Tile ParseCharacter(char c)
        {
            //Tile ret = null;
            //switch (c)
            //{
            //    case 'T':
            //        ret = new Tile(
            //}
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Weapons;
using LandOfAmbrosia.Levels;
using LandOfAmbrosia.Decision;
using LandOfAmbrosia.Common;

namespace LandOfAmbrosia.Characters
{
    abstract class AICharacter : Character
    {
        public static Vector2 NO_IDLE = new Vector2(-100, -100);
        // This Ai's target
        public Character target;

        //This is in grid coordinates
        public Queue<Vector2> pathToTarget;

        //This is going to be in grid coordinates
        public Vector2 idleTimeTarget;

        //This controlls how often enemies 'patrol'
        public static readonly int IDLE_TIME = 5000;

        public int lastMoved;

        public Random gen;

        protected DecisionTree dt;

        public AICharacter(Level level, Model model, Vector3 position, Weapon meleeWeapon, Weapon rangeWeapon, IList<Character> players)
            : base(level, model, Vector3.Zero, position, meleeWeapon, rangeWeapon)
        {
            target = null;
            pathToTarget = new Queue<Vector2>();
            gen = new Random();
            this.gotoIdleState();
        }

        public void gotoIdleState()
        {
            target = null;
            this.chooseNewIdlePoint();
        }

        protected void chooseNewIdlePoint()
        {
            // TODO have the minion choose between 4 - 6 tiles away from it's current location 

            //This is going to be a list of grid indicies
            IList<Vector2> allCloseTiles = new List<Vector2>();

            int idleChoiceRange = 4;

            int minionTileX = containingLevel.GetTileIndexFromXPos(getX());
            int minionTileY = containingLevel.GetTileIndexFromYPos(getY()) - 1; //Is it bad if this is off by +1?

            for (int i = minionTileX - idleChoiceRange; i < minionTileX + idleChoiceRange; ++i)
            {
                for (int j = minionTileY - idleChoiceRange; j < minionTileY + idleChoiceRange; ++j)
                {
                    if (i >= 0 && i < containingLevel.width && j >= 0 && j < containingLevel.height)
                    {
                        Tile checkTile = containingLevel.GetTile(i, j);
                        if (checkTile != null && !checkTile.IsEmpty())
                        {
                            allCloseTiles.Add(new Vector2(i, j));
                        }
                    }
                }
            }

            //Now we've got a list of all the nonEmpty tiles, but we only want the tiles that have nothing above them
            //Also a list of grid indices
            IList<Vector2> closeTopTiles = new List<Vector2>();
            foreach (Vector2 tile in allCloseTiles)
            {
                Vector2 aboveTile = tile + new Vector2(0, 1);
                //So if the aboveTile is actually on the grid, check to see that it's empty
                if (aboveTile.X >= 0 && aboveTile.X < containingLevel.width && aboveTile.Y >= 0 && aboveTile.Y < containingLevel.height)
                {
                    //If it is, then we can try to move there
                    if (containingLevel.GetTile((int)aboveTile.X, (int)aboveTile.Y) == null || containingLevel.GetTile((int)aboveTile.X, (int)aboveTile.Y).IsEmpty())
                    {
                        closeTopTiles.Add(aboveTile);
                    }
                }
                else // but if the aboveTile isn't on the grid we know it's still a safe spot to move to
                {
                    closeTopTiles.Add(aboveTile);
                }
            }

            if (closeTopTiles.Count != 0)
            {
                //Now we have all the tiles that are on the top, so we can choose a random one to move to.
                idleTimeTarget = closeTopTiles.ElementAt(gen.Next(closeTopTiles.Count));
            }
        }

        public override void meleeAttack(GameTime gameTime)
        {
            //TODON'T
        }

        public override bool WantsMeleeAttack()
        {
            return false;
        }
    }
}

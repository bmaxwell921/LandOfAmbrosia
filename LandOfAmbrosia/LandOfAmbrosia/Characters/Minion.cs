using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Common;
using LandOfAmbrosia.Stats;
using LandOfAmbrosia.Levels;
using LandOfAmbrosia.Decision;
using LandOfAmbrosia.Logic;

namespace LandOfAmbrosia.Characters
{
    class Minion : AICharacter
    {
        private readonly float START_HEALTH = 100;
        private readonly float START_ATTACK = 50;
        private readonly float START_DEFENCE = 0;

        private readonly int ATTACK_SPEED = 1600;
        private int lastAttacked;

        private bool wantsRange;

        public Minion(Level level, Model model, Vector3 position, IList<Character> players)
            : base(level, model, position, null, null, players)
        {
            width = .5f;
            height = 1f;
            wantsRange = false;
            dt = new DecisionTree(Constants.MINION_TREE);
            lastAttacked = 0;
        }

        protected override void SetUpStats()
        {
            this.stats = new StatBox(START_HEALTH, START_ATTACK, START_DEFENCE);
        }

        public override Matrix GetWorld()
        {
            Vector3 hackedPos = Constants.ConvertToXNAScene(Constants.UnconvertFromXNAScene(position) + Constants.MINION_POSITION_HACK);
            return Matrix.CreateTranslation(hackedPos);
        }

        public override void Update(GameTime gameTime)
        {
            //This should just do animation stuff...if i add it...
            base.Update(gameTime);
            wantsRange = false;
            AI_STATE decision = dt.evaluateTree(this);
            //Console.WriteLine("Minion decided to: " + decision);

            if (decision == AI_STATE.CONTINUE_MOVE)
            {
                //Just keep moving toward our target
                moveToPoint(idleTimeTarget);
            }
            else if (decision == AI_STATE.NEW_MOVE)
            {
                lastMoved = IDLE_TIME;
                base.chooseNewIdlePoint();
                moveToPoint(idleTimeTarget);
            }
            else if (decision == AI_STATE.WAIT)
            {
                setVelocityX(0);
                setVelocityY(0);
                lastMoved -= gameTime.ElapsedGameTime.Milliseconds;
            }
            else if (decision == AI_STATE.ATTACK)
            {
                wantsRange = true;
                setVelocityX(0);
                setVelocityY(0);
            }
            else if (decision == AI_STATE.FOLLOW_PATH)
            {
                // We already have a path to the target, so move to the closets point
                Vector2 targetPoint = pathToTarget.Peek();
                //If we are already close to the first spot in the path, pop it off and move to the next one
                if (closeTo(targetPoint))
                {
                    pathToTarget.Dequeue();
                    moveToPoint(pathToTarget.Peek());
                }
                moveToPoint(targetPoint);
            }
            else if (decision == AI_STATE.CALC_PATH)
            {
                // Ask the currentLevel for a path to the target
                //IList<Vector2> path = containingLevel.calculatePath(Constants.UnconvertFromXNAScene(position), Constants.UnconvertFromXNAScene(target.position));
                //pathToTarget.Clear();
                //foreach (Vector2 point in path)
                //{
                //    pathToTarget.Enqueue(point);
                //}
            }
        }

        public bool closeTo(Vector2 targetPos)
        {
            //Let's say we are close to the point if we are within a tile from it
            //return Vector2.Distance(new Vector2(getX(), getY()), targetPos) < Constants.TILE_SIZE;

            //If the target is in terms of grid location, we can just check to see if we're in the same tile
            int tileX = containingLevel.GetTileIndexFromXPos(getX());
            int tileY = containingLevel.GetTileIndexFromYPos(getY());
            //Console.WriteLine("Checking if we're close to: " + targetPos + " And we're at: " + new Vector2(tileX, tileY));
            return new Vector2(tileX, tileY) == targetPos;
        }

        private void moveToPoint(Vector2 moveTo)
        {
            // Dynamic seek here? Just does left right
            //Check where I am compared to moveTo. If we are left of it, go right. If we are right of it, go left

            //int xTile = containingLevel.GetTileIndexFromXPos(getX());
            //setVelocityX(Constants.AI_MAX_SPEED_X * ((xTile < moveTo.X) ? 1 : -1));


            //Dynamic seek
            Vector2 myTile = new Vector2(containingLevel.GetTileIndexFromXPos(getX()), containingLevel.GetTileIndexFromYPos(getY()));
            Vector2 desiredVel = moveTo - myTile;

            if (desiredVel.Length() < 2)
            {
                desiredVel /= 2;
                if (desiredVel.Length() > Constants.AI_MAX_SPEED_X)
                {
                    desiredVel /= desiredVel.Length();
                }
            }
            else
            {
                desiredVel /= desiredVel.Length();
            }
            setVelocityX(desiredVel.X * Constants.AI_MAX_SPEED_X);
        }

        public override bool WantsRangeAttack()
        {
            return wantsRange;
        }

        public override Weapons.Projectile rangeAttack(GameTime gameTime, Character closestEnemy)
        {

            if (WantsRangeAttack() && lastAttacked <= 0)
            {
                lastAttacked = ATTACK_SPEED;
                //Vector3 projStart = Constants.UnconvertFromXNAScene(this.position) + Constants.MINION_POSITION_HACK + new Vector3(0, 0, Constants.CHARACTER_DEPTH);
                Vector3 projStart = Constants.UnconvertFromXNAScene(this.position) + new Vector3(0, 0, Constants.CHARACTER_DEPTH);
                //wantsRange = false;
                return new Weapons.Projectile(AssetUtil.GetProjectileModel(Constants.MAGIC_CHAR), projStart, this, Constants.UnconvertFromXNAScene(target.position));
            }
            //wantsRange = false;
            lastAttacked -= gameTime.ElapsedGameTime.Milliseconds;
            return null;
        }
    }
}
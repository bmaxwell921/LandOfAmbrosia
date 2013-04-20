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

            if (decision == AI_STATE.ATTACK && target == null)
            {
                dt.evaluateTree(this);
            }

            if (decision == AI_STATE.CONTINUE_MOVE)
            {
                //Just keep moving toward our target
                moveToPoint(idleTimeTarget);
            }
            else if (decision == AI_STATE.NEW_MOVE)
            {
                base.chooseNewIdlePoint();
                moveToPoint(idleTimeTarget);
            }
            else if (decision == AI_STATE.WAIT)
            {
                lastMoved -= gameTime.ElapsedGameTime.Milliseconds;
            }
            else if (decision == AI_STATE.ATTACK)
            {
                wantsRange = true;
            }
            else if (decision == AI_STATE.FOLLOW_PATH)
            {
                //TODO
            }
            else if (decision == AI_STATE.CALC_PATH)
            {
                //TODO
            }
        }

        private void moveToPoint(Vector3 moveTo)
        {
            //TODO
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
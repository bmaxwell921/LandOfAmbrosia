using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Weapons;
using LandOfAmbrosia.Levels;

namespace LandOfAmbrosia.Characters
{
    abstract class AICharacter : Character
    {
        #region Constants
        public static Vector3 DEFAULT_SPEED = new Vector3(-5f, 0, 0);
        #endregion

        protected enum AIState { ATTACKING, PATROLLING, NONE }

        protected AIState currentState;

        public AICharacter(Level level, Model model, Vector3 position, Weapon meleeWeapon, Weapon rangeWeapon, IList<Character> players)
            : base(level, model, DEFAULT_SPEED, position, meleeWeapon, rangeWeapon)
        {
            currentState = AIState.NONE;
        }
        public override void Update(GameTime gameTime)
        {
            UpdateState(gameTime);
            MakeDecision();
        }

        protected abstract void UpdateState(GameTime gameTime);

        protected abstract void MakeDecision();

        public override Projectile rangeAttack(GameTime gametime, Character closestEnemy)
        {
            return null;
        }

        public override bool WantsRangeAttack()
        {
            return false;
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

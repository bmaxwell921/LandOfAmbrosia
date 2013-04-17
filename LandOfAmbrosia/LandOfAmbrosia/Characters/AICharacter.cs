using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Weapons;

namespace LandOfAmbrosia.Characters
{
    abstract class AICharacter : Character
    {
        #region Constants
        public static Vector3 DEFAULT_SPEED = new Vector3(-5f, 0, 0);
        #endregion

        protected enum AIState { ATTACKING, PATROLLING, NONE }

        protected AIState currentState;

        public AICharacter(Model model, Vector3 position, Weapon meleeWeapon, Weapon rangeWeapon, int maxHealth, IList<Character> players)
            : base(model, DEFAULT_SPEED, position, meleeWeapon, rangeWeapon, maxHealth)
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

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
        // This Ai's target
        public Character target;

        //The sequence of points that will take this ai to the target's last known location
        //public IList<Vector3> moveToPoints;
        public Queue<Vector3> pathToTarget;

        public Vector3 idleTimeTarget;

        //This controlls how often enemies 'patrol'
        public static readonly int IDLE_TIME = 5000;

        public int lastMoved;

        public Random gen;

        public AI_STATE curState;

        protected DecisionTree dt;

        public AICharacter(Level level, Model model, Vector3 position, Weapon meleeWeapon, Weapon rangeWeapon, IList<Character> players)
            : base(level, model, Vector3.Zero, position, meleeWeapon, rangeWeapon)
        {
            target = null;
            //moveToPoints = new List<Vector3>();
            gen = new Random();
            this.gotoIdleState();
        }

        public void gotoIdleState()
        {
            //TODO
            pathToTarget = null;
            target = null;
            //Chooses a random position within the chunk for this ai to move to, then sets it as the idleTimeTarget
            curState = AI_STATE.WAIT;
        }

        protected void chooseNewIdlePoint()
        {
            //TODO
        }

        //public override Projectile rangeAttack(GameTime gametime, Character closestEnemy)
        //{
        //    return null;
        //}

        //public override bool WantsRangeAttack()
        //{
        //    return false;
        //}

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

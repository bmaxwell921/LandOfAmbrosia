using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Common;
using LandOfAmbrosia.Characters;

namespace LandOfAmbrosia.Weapons
{
    /// <summary>
    /// A projectile that tracks to a character, not just a position
    /// </summary>
    class SmartProjectile : Projectile
    {
        public Character target;

        public SmartProjectile(Model model, Vector3 position, Character target) : base(model, position, Constants.UnconvertFromXNAScene(target.position) + Constants.MINION_POSITION_HACK)
        {
            this.target = target;
        }

        protected override void UpdateTarget()
        {
            this.targetPosition = (target is Minion) ? Constants.UnconvertFromXNAScene(target.position) + Constants.MINION_POSITION_HACK : Constants.UnconvertFromXNAScene(target.position);
        }

        protected override void CheckKill()
        {
            base.CheckKill();
            if (timeToDie)
            {
                target.health -= Constants.DEFAULT_MINION_HEALTH;
            }
        }
    }
}

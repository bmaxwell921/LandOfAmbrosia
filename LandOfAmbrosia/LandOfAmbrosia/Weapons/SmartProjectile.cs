using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Common;
using LandOfAmbrosia.Characters;
using LandOfAmbrosia.Levels;

namespace LandOfAmbrosia.Weapons
{
    /// <summary>
    /// A projectile that tracks to a character, not just a position
    /// </summary>
    class SmartProjectile : Projectile
    {
        public Character target;

        public SmartProjectile(Level l, Model model, Vector3 position, Character source, Character target)
            : base(l ,model, position, source, Constants.UnconvertFromXNAScene(target.position) + Constants.MINION_POSITION_HACK)
        {
            this.target = target;
        }

        protected override void UpdateTarget()
        {
            this.targetPosition = Constants.UnconvertFromXNAScene(target.position);
        }
    }
}

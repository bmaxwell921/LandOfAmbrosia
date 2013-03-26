using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandOfAmbrosia.Weapons
{
    class Projectile : Weapon
    {
        public override bool CollidesWith(Characters.ICollidable other)
        {
            throw new NotImplementedException();
        }
    }
}

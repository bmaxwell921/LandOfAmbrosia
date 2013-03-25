using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Characters;

namespace LandOfAmbrosia.Weapons
{
    abstract class Weapon : ICollidable
    {
        public abstract bool CollidesWith(ICollidable other);
    }
}

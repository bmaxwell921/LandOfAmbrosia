using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandOfAmbrosia.Characters
{
    interface ICollidable
    {
        bool CollidesWith(ICollidable other);
    }
}

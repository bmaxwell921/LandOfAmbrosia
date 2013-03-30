using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Common;

namespace LandOfAmbrosia.Managers
{
    /// <summary>
    /// Super class
    /// </summary>
    abstract class AbstractInputController
    {
        public abstract bool PressedJump();

        public abstract Vector2 GetMovement();

        public abstract ATTACK_TYPE GetAttackType();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LandOfAmbrosia.Characters
{
    class UserControlledCharacter : Character
    {
        #region Constants
        public const int DEFAULT_MAX_HEALTH = 100;
        #endregion

        public UserControlledCharacter(Model model, Vector3 position) :
            base(model, Vector3.Zero, position, null, null, DEFAULT_MAX_HEALTH)
        {
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}

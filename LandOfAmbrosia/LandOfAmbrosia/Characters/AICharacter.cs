using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Weapons;

namespace LandOfAmbrosia.Characters
{
    class AICharacter : Character
    {
        #region Constants
        public static Vector3 DEFAULT_SPEED = new Vector3(-5f, 0, 0);
        #endregion

        public AICharacter(Model model, Vector3 position, Weapon meleeWeapon, Weapon rangeWeapon, int maxHealth)
            : base(model, DEFAULT_SPEED, position, meleeWeapon, rangeWeapon, maxHealth)
        {
        }
        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}

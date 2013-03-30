using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Common;
using LandOfAmbrosia.Controllers;

namespace LandOfAmbrosia.Characters
{
    class UserControlledCharacter : Character
    {
        private AbstractInputController inputController;

        public UserControlledCharacter(char character, Model model, Vector3 position) :
            base(model, Vector3.Zero, position, null, null, Constants.DEFAULT_MAX_HEALTH)
        {
            inputController = new KeyboardInput();
        }

        public override void Update()
        {
            Vector2 gravityComponent = this.calculateGravity();
            this.position += gravityComponent;
        }

        private Vector3 calculateGravity()
        {
            //TODO
            return new Vector3(0, -1, 0);
        }
    }
}

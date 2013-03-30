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
            //inputController = new KeyboardInput();
            inputController = new XboxController((character == Constants.PLAYER1_CHAR) ? PlayerIndex.One : PlayerIndex.Two);
            onGround = true;
        }

        public override void Update()
        {
            Vector3 change = Vector3.Zero;
            if (!onGround)
            {
                change += this.calculateGravity();
            }

            change += Constants.ConvertToXNAScene(inputController.GetMovement());
            
            this.position += change;
        }

        private Vector3 calculateGravity()
        {
            //TODO make this proper
            Vector3 gravity = new Vector3(0, -0.25f, 0);



            return Constants.ConvertToXNAScene(gravity);
        }
    }
}

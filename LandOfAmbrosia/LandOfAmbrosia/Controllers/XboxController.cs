using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Managers;
using Microsoft.Xna.Framework.Input;
using LandOfAmbrosia.Common;

namespace LandOfAmbrosia.Controllers
{
    class XboxController : AbstractInputController
    {
        //TODO pause stuff?

        PlayerIndex player;

        public XboxController(PlayerIndex player)
        {
            this.player = player;
        }

        /// <summary>
        /// The A button means jump so check if that button was pressed
        /// </summary>
        /// <returns></returns>
        public override bool PressedJump()
        {
            return GamePad.GetState(player).Buttons.A == ButtonState.Pressed;
        }

        /// <summary>
        /// Characters have a max speed they can travel at, so we see how
        /// far the thumbstick has been pressed and set the movement accordingly
        /// </summary>
        /// <returns></returns>
        public override Vector2 GetMovement()
        {
            Vector2 move = Vector2.Zero;

            move.X = Constants.MAX_SPEED_X * GamePad.GetState(player).ThumbSticks.Left.X;

            return move;
        }

        /// <summary>
        /// XboxControllers are registered with user controlled characters so
        /// they will only be able to do either magic attacks or melee attacks.
        /// 
        /// X is for melee attacks and B is for magic attacks
        /// </summary>
        /// <returns></returns>
        public override ATTACK_TYPE GetAttackType()
        {
            if (GamePad.GetState(player).Buttons.X == ButtonState.Pressed)
            {
                return ATTACK_TYPE.MELEE;
            }
            if (GamePad.GetState(player).Buttons.B == ButtonState.Pressed)
            {
                return ATTACK_TYPE.MAGIC;
            }
            return ATTACK_TYPE.NONE;
        }
    }
}

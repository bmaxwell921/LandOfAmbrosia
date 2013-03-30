using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Managers;
using LandOfAmbrosia.Common;
using Microsoft.Xna.Framework.Input;

namespace LandOfAmbrosia.Controllers
{
    /// <summary>
    /// Character control for testing purposes only
    /// </summary>
    class KeyboardInput : AbstractInputController
    {
        /// <summary>
        /// This class is for testing purposes only so we don't need to worry about multiple players
        /// </summary>
        public KeyboardInput()
        {
        }

        /// <summary>
        /// Space to jump
        /// </summary>
        /// <returns></returns>
        public override bool PressedJump()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Space);
        }

        /// <summary>
        /// Arrow keys to move
        /// </summary>
        /// <returns></returns>
        public override Microsoft.Xna.Framework.Vector2 GetMovement()
        {
            //Maybe if I feel like it, make them run faster when you hold down the key...this can be applied to the xbox controller to asymptotically get them to the max speed
            int dir;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                dir = 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                dir = -1;
            }
            else
            {
                dir = 0;
            }

            return new Microsoft.Xna.Framework.Vector2(Constants.MAX_SPEED_X * dir, 0);
        }

        /// <summary>
        /// Z for melee attack, X for magic attack
        /// </summary>
        /// <returns></returns>
        public override ATTACK_TYPE GetAttackType()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                return ATTACK_TYPE.MELEE;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                return ATTACK_TYPE.MAGIC;
            }
            return ATTACK_TYPE.NONE;
        }
    }
}

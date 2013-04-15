using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Common;
using LandOfAmbrosia.Controllers;
using LandOfAmbrosia.Weapons;
using LandOfAmbrosia.Logic;

namespace LandOfAmbrosia.Characters
{
    class UserControlledCharacter : Character
    {
        private AbstractInputController inputController;

        private readonly float JUMP_VELOCITY = 0.3f;

        public UserControlledCharacter(char character, Model model, Vector3 position) :
            base(model, Vector3.Zero, position, null, null, Constants.DEFAULT_MAX_HEALTH)
        {
            inputController = new KeyboardInput();
            //inputController = new XboxController((character == Constants.PLAYER1_CHAR) ? PlayerIndex.One : PlayerIndex.Two);
            width = Constants.CHARACTER_WIDTH;
            height = Constants.CHARACTER_HEIGHT;
        }

        public void CheckInput()
        {
            float xVel = 0;
            xVel = inputController.GetMovement().X;
            if (inputController.PressedJump())
            {
                this.jump(false);
            }
            this.setVelocityX(xVel);
        }

        //Makes the character jump. Set forceJump to true for an in-air jump
        private void jump(bool forceJump)
        {
            if (onGround || forceJump)
            {
                onGround = false;
                this.setVelocityY(JUMP_VELOCITY);
            }
        }

        public override Projectile rangeAttack(Character closestEnemy)
        {
            if (inputController.GetAttackType() == ATTACK_TYPE.MAGIC)
            {
                //TODO make sure the added vector is correct, maybe we can have the projectile start on the correct side
                Vector3 projStart = Constants.UnconvertFromXNAScene(this.position) + new Vector3(1, -0.5f, Constants.CHARACTER_DEPTH);
                return new Projectile(AssetUtil.GetProjectileModel(Constants.MAGIC_CHAR), projStart, closestEnemy);
            }
            return null;
        }

        public override bool WantsRangeAttack()
        {
            return inputController.GetAttackType() == ATTACK_TYPE.MAGIC;
        }

        public override void meleeAttack()
        {
            if (inputController.GetAttackType() == ATTACK_TYPE.MELEE)
            {
                //TODO
            }
        }

        public override bool WantsMeleeAttack()
        {
            return inputController.GetAttackType() == ATTACK_TYPE.MELEE;
        }
    }

}

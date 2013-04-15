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

        private readonly long ATTACK_SPEED = 500;
        private long lastAttacked;

        public UserControlledCharacter(char character, Model model, Vector3 position) :
            base(model, Vector3.Zero, position, null, null, Constants.DEFAULT_MAX_HEALTH)
        {
            inputController = new KeyboardInput();
            //inputController = new XboxController((character == Constants.PLAYER1_CHAR) ? PlayerIndex.One : PlayerIndex.Two);
            width = Constants.CHARACTER_WIDTH;
            height = Constants.CHARACTER_HEIGHT;
            lastAttacked = 0;
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

        public override Projectile rangeAttack(GameTime gameTime, Character closestEnemy)
        {
            if (inputController.GetAttackType() == ATTACK_TYPE.MAGIC && lastAttacked <= 0)
            {
                lastAttacked = ATTACK_SPEED;
                //TODO make sure the added vector is correct, maybe we can have the projectile start on the correct side
                Vector3 projStart = Constants.UnconvertFromXNAScene(this.position) + new Vector3(Constants.TILE_SIZE, -Constants.TILE_SIZE, Constants.CHARACTER_DEPTH);
                return new Projectile(AssetUtil.GetProjectileModel(Constants.MAGIC_CHAR), projStart, closestEnemy);
            }
            lastAttacked -= gameTime.ElapsedGameTime.Milliseconds;
            return null;
        }

        public override bool WantsRangeAttack()
        {
            return inputController.GetAttackType() == ATTACK_TYPE.MAGIC;
        }

        public override void meleeAttack(GameTime gametime)
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

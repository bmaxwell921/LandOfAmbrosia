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
using LandOfAmbrosia.Stats;

namespace LandOfAmbrosia.Characters
{
    class UserControlledCharacter : Character
    {
        private AbstractInputController inputController;

        private readonly float JUMP_VELOCITY = 0.3f;

        private readonly long ATTACK_SPEED = 100;
        private long lastAttacked;

        private bool lastDirWasLeft;

        private readonly float STARTING_HEALTH = 200;
        private readonly float STARTING_ATTACK = 75;
        private readonly float STARTING_DEFENCE = 25;

        public UserControlledCharacter(char character, Model model, Vector3 position) :
            base(model, Vector3.Zero, position, null, null, Constants.DEFAULT_MAX_HEALTH)
        {
            inputController = new KeyboardInput();
            //inputController = new XboxController((character == Constants.PLAYER1_CHAR) ? PlayerIndex.One : PlayerIndex.Two);
            width = Constants.CHARACTER_WIDTH;
            height = Constants.CHARACTER_HEIGHT;
            lastAttacked = 0;
            lastDirWasLeft = false;
        }

        protected override void SetUpStats()
        {
            this.stats = new StatBox(STARTING_HEALTH, STARTING_ATTACK, STARTING_DEFENCE);
        }

        public void CheckInput()
        {
            float xVel = 0;
            xVel = inputController.GetMovement().X;
            if (inputController.PressedJump())
            {
                this.jump(false);
            }
            if (xVel != 0)
            {
                lastDirWasLeft = xVel < 0;
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
                if (closestEnemy != null)
                {
                    Vector3 projStart = Constants.UnconvertFromXNAScene(this.position) + Constants.MINION_POSITION_HACK + new Vector3(0,0,Constants.CHARACTER_DEPTH);
                    //return new SmartProjectile(AssetUtil.GetProjectileModel(Constants.MAGIC_CHAR), projStart, closestEnemy);
                    return new SmartProjectile(AssetUtil.GetProjectileModel(Constants.MAGIC_CHAR), projStart, this, closestEnemy);
                }
                else
                {
                    //Just blast some magic 'forward.' Who doesn't love just blasting spells
                    Vector3 projStart = Constants.UnconvertFromXNAScene(this.position) + Constants.MINION_POSITION_HACK + new Vector3(0, 0, Constants.CHARACTER_DEPTH);
                    Vector3 target = projStart + new Vector3(5 * ((lastDirWasLeft) ? -Constants.TILE_SIZE : Constants.TILE_SIZE), 0, 0);
                    //return new Projectile(AssetUtil.GetProjectileModel(Constants.MAGIC_CHAR), projStart, target);
                    return new Projectile(AssetUtil.GetProjectileModel(Constants.MAGIC_CHAR), projStart, this, target);
                }
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

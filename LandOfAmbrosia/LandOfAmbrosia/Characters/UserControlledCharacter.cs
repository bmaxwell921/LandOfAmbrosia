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
using LandOfAmbrosia.Levels;
using LandOfAmbrosia.Experience;

namespace LandOfAmbrosia.Characters
{
    class UserControlledCharacter : Character
    {
        private AbstractInputController inputController;

        private readonly long ATTACK_SPEED = 100;
        private long lastAttacked;

        private bool lastDirWasLeft;

        private readonly float STARTING_HEALTH = 200;
        private readonly float STARTING_ATTACK = 75;
        private readonly float STARTING_DEFENCE = 25;
        private readonly float STARTING_EXP_REQ = 100;

        public UserControlledCharacter(Level level, char character, Model model, Vector3 position) :
            base(level, model, Vector3.Zero, position, null, null)
        {
            inputController = new KeyboardInput();
            //inputController = new XboxController((character == Constants.PLAYER1_CHAR) ? PlayerIndex.One : PlayerIndex.Two);
            width = Constants.CHARACTER_WIDTH;
            height = Constants.CHARACTER_HEIGHT;
            lastAttacked = 0;
            lastDirWasLeft = false;
            SetUpStats();
        }

        protected void SetUpStats()
        {
            this.stats = new StatBox(STARTING_HEALTH, STARTING_ATTACK, STARTING_DEFENCE, STARTING_EXP_REQ);
        }

        public void CheckInput()
        {
            float xVel = 0;
            xVel = inputController.GetMovement().X;
            if (inputController.PressedJump())
            {
                jump(false);
            }
            if (xVel != 0)
            {
                lastDirWasLeft = xVel < 0;
            }
            this.setVelocityX(xVel);
        }

        public override Projectile rangeAttack(GameTime gameTime, Character closestEnemy)
        {
            if (WantsRangeAttack() && lastAttacked <= 0)
            {
                lastAttacked = ATTACK_SPEED;
                if (closestEnemy != null)
                {
                    Vector3 projStart = Constants.UnconvertFromXNAScene(this.position) + new Vector3(0, 0, Constants.CHARACTER_DEPTH);
                    return new SmartProjectile(containingLevel, AssetUtil.GetProjectileModel(Constants.MAGIC_CHAR), projStart, this, closestEnemy);
                }
                else
                {
                    //Just blast some magic 'forward.' Who doesn't love just blasting spells
                    Vector3 projStart = Constants.UnconvertFromXNAScene(this.position) + new Vector3(0, 0, Constants.CHARACTER_DEPTH);
                    Vector3 target = projStart + new Vector3(5 * ((lastDirWasLeft) ? -Constants.TILE_SIZE : Constants.TILE_SIZE), 0, 0);
                    return new Projectile(containingLevel, AssetUtil.GetProjectileModel(Constants.MAGIC_CHAR), projStart, this, target);
                }
            }
            lastAttacked -= gameTime.ElapsedGameTime.Milliseconds;
            return null;
        }

        public void applyExperience(ExperienceOrb exp)
        {
            stats.changeCurrentStat(Constants.EXPERIENCE_KEY, exp.amount);

            //Check if we leveled up
            float leftOver = stats.getStatCurrentVal(Constants.EXPERIENCE_KEY) - stats.getStatBaseVal(Constants.EXPERIENCE_KEY);
            if (leftOver >= 0)
            {
                //Level up will increase all the base values and then reset the stats, so we can just add the left overs after
                stats.levelUpAllStats();
                stats.changeCurrentStat(Constants.EXPERIENCE_KEY, leftOver);
            }
        }

        public override bool WantsRangeAttack()
        {
            return inputController.GetAttackType() == ATTACK_TYPE.MAGIC;
        }

        public override void meleeAttack(GameTime gametime)
        {
            if (inputController.GetAttackType() == ATTACK_TYPE.MELEE)
            {
                //TODON'T
            }
        }

        public override bool WantsMeleeAttack()
        {
            return inputController.GetAttackType() == ATTACK_TYPE.MELEE;
        }
    }

}

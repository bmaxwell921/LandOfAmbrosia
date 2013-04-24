using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Levels;

namespace LandOfAmbrosia.Characters
{
    class Boss : AICharacter
    {
        public Boss(Level level, Model model, Vector3 position, IList<Character> players)
            : base(level, model, position, null, null, players)
        {
        }

        protected override void SetUpStats()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override Weapons.Projectile rangeAttack(GameTime gameTime, Character closestEnemy)
        {
            throw new NotImplementedException();
        }

        public override bool WantsRangeAttack()
        {
            return false;
        }
    }
}

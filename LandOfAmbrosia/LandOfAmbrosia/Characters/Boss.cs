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
        public const int DEFAULT_BOSS_HEALTH = 500;


        public Boss(Level level, Model model, Vector3 position, IList<Character> players)
            : base(level, model, position, null, null, players)
        {
        }

        protected override void SetUpStats()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateState(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        protected override void MakeDecision()
        {
            throw new NotImplementedException();
        }
    }
}

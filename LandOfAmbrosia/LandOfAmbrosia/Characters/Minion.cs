using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Common;
using LandOfAmbrosia.Stats;
using LandOfAmbrosia.Levels;

namespace LandOfAmbrosia.Characters
{
    class Minion : AICharacter
    {
        private readonly long THINK_TIME = 200;
        private long lastDidStuff;

        private readonly float START_HEALTH = 100;
        private readonly float START_ATTACK = 50;
        private readonly float START_DEFENCE = 0;

        public Minion(Level level, Model model, Vector3 position, IList<Character> players)
            : base(level, model, position, null, null, players)
        {
            width = .5f;
            height = 1f;
        }

        protected override void SetUpStats()
        {
            this.stats = new StatBox(START_HEALTH, START_ATTACK, START_DEFENCE);
        }

        public override Matrix GetWorld()
        {
            Vector3 hackedPos = Constants.ConvertToXNAScene(Constants.UnconvertFromXNAScene(position) + Constants.MINION_POSITION_HACK);
            return Matrix.CreateTranslation(hackedPos);
        }

        protected override void UpdateState(GameTime gameTime)
        {
            if (lastDidStuff <= 0)
            {
                //Update the state based on player positions
            }
            else
            {
                lastDidStuff -= gameTime.ElapsedGameTime.Milliseconds;
            }
        }

        protected override void MakeDecision()
        {
           //throw new NotImplementedException();
        }
    }
}

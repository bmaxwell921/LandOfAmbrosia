using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Common;

namespace LandOfAmbrosia.Characters
{
    class Minion : AICharacter
    {
        private readonly long THINK_TIME = 200;
        private long lastDidStuff;

        public Minion(Model model, Vector3 position, IList<Character> players)
            : base(model, position, null, null, Constants.DEFAULT_MINION_HEALTH, players)
        {
            width = .5f;
            height = 1f;
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

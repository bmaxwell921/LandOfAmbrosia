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
        public const int DEFAULT_MINION_HEALTH = 50;

        public Minion(Model model, Vector3 position)
            : base(model, position, null, null, DEFAULT_MINION_HEALTH)
        {
            width = .5f;
            height = 1f;
        }

        public override Matrix GetWorld()
        {
            Vector3 hackedPos = Constants.ConvertToXNAScene(Constants.UnconvertFromXNAScene(position) + Constants.MINION_POSITION_HACK);
            return Matrix.CreateTranslation(hackedPos);
        }
    }
}

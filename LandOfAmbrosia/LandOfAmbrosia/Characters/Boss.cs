using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LandOfAmbrosia.Characters
{
    class Boss : AICharacter
    {
        public const int DEFAULT_BOSS_HEALTH = 500;


        public Boss(Model model, Vector3 position)
            : base(model, position, null, null, DEFAULT_BOSS_HEALTH)
        {
        }
    }
}

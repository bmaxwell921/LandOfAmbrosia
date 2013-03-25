using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandOfAmbrosia.Characters
{
    interface ICharacter
    {
        void Update();

        void Draw(Camera c);
    }
}

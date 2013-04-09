using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LandOfAmbrosia.Characters
{
    interface ICharacter
    {
        void Update(GameTime gameTime);

        void Draw(CameraComponent c);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LandOfAmbrosia.UI
{
    interface Menu
    {
        void Draw(SpriteBatch sb, Game game);

        void Update(bool leftPressed, bool rightPressed);
    }
}

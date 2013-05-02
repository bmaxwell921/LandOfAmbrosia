using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LandOfAmbrosia.UI
{
    interface Screen
    {
        void Draw(SpriteBatch sb, Rectangle drawArea);
    }
}

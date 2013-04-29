using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace LandOfAmbrosia.UI
{
    class StartMenu : TwoChoiceMenu
    {

        public StartMenu(Game game) : base(game)
        {
            base.setTitleFont(game.Content.Load<SpriteFont>(@"Fonts\Title"));
            base.setChoicesFont(game.Content.Load<SpriteFont>(@"Fonts\StartScreen"));
        }

        public override void ConfirmSelection()
        {
            ((LandOfAmbrosiaGame)game).startGame(base.leftSelected ? 1 : 2);            
        }

        protected override string getTitleMessage()
        {
            return "Land of Ambrosia";
        }

        protected override IList<string> getChoices()
        {
            IList<string> ret = new List<string>();
            ret.Add("One Player");
            ret.Add("Two Player");
            return ret;
        }
    }
}

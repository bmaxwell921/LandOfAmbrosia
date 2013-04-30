using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LandOfAmbrosia.UI
{
    class VictoryMenu : TwoChoiceMenu
    {
        public VictoryMenu(Game game)
            : base(game)
        {

        }
        protected override string getTitleMessage()
        {
            return "YOU WIN!";
        }

        protected override IList<string> getChoices()
        {
            IList<string> ret = new List<string>();
            ret.Add("Main Menu");
            ret.Add("Quit");
            return ret;
        }

        public override void ConfirmSelection()
        {
            if (leftSelected)
            {
                ((LandOfAmbrosiaGame)game).restartGame();
            }
            else
            {
                game.Exit();
            }
        }
    }
}

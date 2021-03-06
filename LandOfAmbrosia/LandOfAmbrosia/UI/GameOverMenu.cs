﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LandOfAmbrosia.UI
{
    class GameOverMenu : TwoChoiceMenu
    {

        public GameOverMenu(Game game)
            : base(game)
        {
        }

        protected override string getTitleMessage()
        {
            return "Game Over";
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

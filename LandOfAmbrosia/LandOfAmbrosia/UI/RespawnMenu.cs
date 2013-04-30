using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Managers;

namespace LandOfAmbrosia.UI
{
    class RespawnMenu : TwoChoiceMenu
    {

        public RespawnMenu(Game game)
            : base(game)
        {
        }

        protected override string getTitleMessage()
        {
            return "You are Dead";
        }

        protected override IList<string> getChoices()
        {
            IList<string> ret = new List<string>();
            ret.Add("Respawn");
            ret.Add("Rage Quit");
            return ret;
        }

        protected override string getDetailMessage()
        {
            LevelManager lm = (LevelManager)game.Services.GetService(typeof(LevelManager));
            return "Lives left: " + lm.levels[lm.curLevelInfo].numLives + ". " + base.getDetailMessage();
        }

        public override void ConfirmSelection()
        {
            if (leftSelected)
            {
                ((LandOfAmbrosiaGame)game).TransitionToState(Common.GameState.PLAYING);
            }
            else
            {
                game.Exit();
            }
        }
    }
}

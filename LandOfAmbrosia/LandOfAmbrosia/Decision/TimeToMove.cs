using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Common;
using LandOfAmbrosia.Characters;

namespace LandOfAmbrosia.Decision
{
    class TimeToMove : ADecisionTreeNode
    {
        private bool chooseNextNode(Minion m)
        {
            return m.lastMoved <= 0;
        }

        public override AI_STATE chooseAction(Characters.Minion m)
        {
            return (chooseNextNode(m)) ? AI_STATE.YES : AI_STATE.NO;
        }
    }
}

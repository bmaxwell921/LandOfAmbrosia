using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandOfAmbrosia.Decision
{
    class ContinueMoveNode : ADecisionTreeNode
    {

        public override Common.AI_STATE chooseAction(Characters.Minion m)
        {
            return Common.AI_STATE.CONTINUE_MOVE;
        }
    }
}

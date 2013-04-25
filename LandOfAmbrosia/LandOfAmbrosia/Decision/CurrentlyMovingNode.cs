using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandOfAmbrosia.Decision
{
    class CurrentlyMovingNode : ADecisionTreeNode
    {
        public override Common.AI_STATE chooseAction(Characters.Minion m)
        {
            return m.closeTo(m.idleTimeTarget) ? Common.AI_STATE.NO : Common.AI_STATE.YES;
        }
    }
}

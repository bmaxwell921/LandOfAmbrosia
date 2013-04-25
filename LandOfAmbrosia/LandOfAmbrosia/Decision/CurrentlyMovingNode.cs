using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Characters;

namespace LandOfAmbrosia.Decision
{
    class CurrentlyMovingNode : ADecisionTreeNode
    {
        public override Common.AI_STATE chooseAction(Characters.Minion m)
        {//return m.idleTimeTarget != null && ... then set it null in the calc path
            return m.idleTimeTarget == AICharacter.NO_IDLE || m.closeTo(m.idleTimeTarget) ? Common.AI_STATE.NO : Common.AI_STATE.YES;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Common;
using LandOfAmbrosia.Characters;

namespace LandOfAmbrosia.Decision
{
    class PathToEnemyNode : ADecisionTreeNode
    {
        private bool chooseNextNode(Minion m)
        {
            //We have a path to the target if the queue is not null
            return m.pathToTarget != null;
        }

        public override AI_STATE chooseAction(Minion m)
        {
            //If we have a path to the target, follow it, otherwise calculate it
            return chooseNextNode(m) ? AI_STATE.FOLLOW_PATH : AI_STATE.CALC_PATH;
        }
    }
}

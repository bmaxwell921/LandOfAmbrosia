using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandOfAmbrosia.Decision
{
    class CalcNode : ADecisionTreeNode
    {
        //public override bool chooseNextNode(Characters.Minion m)
        //{
        //    throw new Exception("Shouldn't be calling a rec method on an action node");
        //}

        public override Common.AI_STATE chooseAction(Characters.Minion m)
        {
            return Common.AI_STATE.CALC_PATH;
        }
    }
}

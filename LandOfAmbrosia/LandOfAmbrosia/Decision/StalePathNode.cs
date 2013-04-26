﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandOfAmbrosia.Decision
{
    class StalePathNode : ADecisionTreeNode
    {

        public override Common.AI_STATE chooseAction(Characters.Minion m)
        {
            //Decide whether the current path to the target is stale. If so, we need to recalculate the path, otherwise just follow the current one

            //Just check if the target is on the path?
            return Common.AI_STATE.NO;
        }
    }
}

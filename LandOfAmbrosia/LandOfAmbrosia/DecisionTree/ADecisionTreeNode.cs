using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Characters;

namespace LandOfAmbrosia.DecisionTree
{
    class ADecisionTreeNode
    {
        public ADecisionTreeNode no;
        public ADecisionTreeNode yes;

        //This will return a Decision enum type, not a boolean
        public delegate bool evaluateSituation(Minion m);

        public bool evaluateNode(evaluateSituation evalFunction, ADecisionTreeNode root)
        {
            if (root.isLeaf())
            {
                //I have no idea how to do delegates...
                //return evaluateSituation();
            }
        }

        public bool isLeaf()
        {
            return no == null && yes == null;
        }
    }
}

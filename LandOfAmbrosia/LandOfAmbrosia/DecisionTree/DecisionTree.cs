using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Common;

namespace LandOfAmbrosia.DecisionTree
{
    class DecisionTree
    {
        ADecisionTreeNode root;

        /// <summary>
        /// Creates a decision tree for the minion
        /// </summary>
        public DecisionTree(int treeType)
        {
            if (treeType == Constants.MINION_TREE)
            {
                setUpMinionTree();
            }
            else if (treeType == Constants.BOSS_TREE)
            {
                setUpBossTree();
            }
        }

        private void setUpMinionTree()
        {
        }

        private void setUpBossTree()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Common;
using LandOfAmbrosia.Characters;

namespace LandOfAmbrosia.Decision
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

        /*
         * Tree looks like:
         * (No on left, Yes on right)
         *                          Can I see an enemy?
         *                      /                       \
         *      Am I moving to a position?                  Is the enemy in attacking range?
         *     /                    \                       /                                \
         *  Is it time to move?     Continue Moving       Do I have a path to him?           Attack
         *  /              \                               /                    \
         * Wait         New Move                      Calc Path              Follow Path
         *         
         * Any 'nodes' that aren't questions are actions returned by the parent
         */ 
        private void setUpMinionTree()
        {
            root = new SeeEnemyNode();
            ADecisionTreeNode currentlyMoving = new CurrentlyMovingNode();
            root.no = currentlyMoving;
            ADecisionTreeNode attack = new AttackEnemyNode();
            root.yes = attack;

            ADecisionTreeNode timeToMove = new TimeToMove();
            currentlyMoving.no = timeToMove;
            currentlyMoving.yes = new ContinueMoveNode();

            timeToMove.no = new WaitNode();
            timeToMove.yes = new NewMoveNode();

            ADecisionTreeNode pathToEnemy = new PathToEnemyNode();
            attack.no = pathToEnemy;
            attack.yes = new AttackNode();

            pathToEnemy.no = new CalcNode();
            pathToEnemy.yes = new FollowNode();
        }

        private void setUpBossTree()
        {
        }

        public AI_STATE evaluateTree(Minion m)
        {
            return evaluateTreeRec(m, root);
        }

        private AI_STATE evaluateTreeRec(Minion m, ADecisionTreeNode root)
        {
            if (root == null)
            {
                throw new Exception("The decision tree isn't working properly. We hit a null node");
            }
            //The tree should only have leaves and 2 child nodes...hopefully
            //if (root.isLeaf())
            //{
            //    choice = root.chooseAction(m);
            //    return false;
            //}
            //else
            //{
            //    if (root.chooseNextNode(m))
            //    {
            //        return evaluateTreeRec(m, root.yes, out choice);
            //    }
            //    else
            //    {
            //        return evaluateTreeRec(m, root.no, out choice);
            //    }
            //}
            AI_STATE result = root.chooseAction(m);
            if (result == AI_STATE.NO)
            {
                return evaluateTreeRec(m, root.no);
            }
            else if (result == AI_STATE.YES)
            {
                return evaluateTreeRec(m, root.yes);
            }
            return result;
        }
    }
}

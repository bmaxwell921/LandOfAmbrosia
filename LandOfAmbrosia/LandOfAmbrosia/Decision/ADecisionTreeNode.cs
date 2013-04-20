using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Characters;
using LandOfAmbrosia.Common;

namespace LandOfAmbrosia.Decision
{
    abstract class ADecisionTreeNode
    {
        public ADecisionTreeNode no;
        public ADecisionTreeNode yes;

        /// <summary>
        /// Call this method when traversing the decision tree, it will tell you whether
        /// to go left or right, depending on the situation of the level. If this node doesn't
        /// have any children, then use the result of chooseAction
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        //public abstract bool chooseNextNode(Minion m);

        /// <summary>
        /// A method that returns the action associated with the given node. If this is
        /// an internal node it will return either left or right. Otherwise if it's a leaf
        /// it will return a specific action --HACK--
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public abstract AI_STATE chooseAction(Minion m);

        public bool isLeaf()
        {
            return no == null && yes == null;
        }
    }
}

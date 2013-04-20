using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Characters;
using LandOfAmbrosia.Common;
using Microsoft.Xna.Framework;

namespace LandOfAmbrosia.Decision
{
    class AttackEnemyNode : ADecisionTreeNode
    {
        /// <summary>
        /// This method will evaluate if the minion's target is within attacking range. The target must
        /// be set by the previous node in the tree (SeeEnemy), otherwise this returns false
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private bool chooseNextNode(Minion m)
        {
            if (m.target == null)
            {
                return false;
            }
            return (Vector3.Distance(Constants.UnconvertFromXNAScene(m.position), Constants.UnconvertFromXNAScene(m.target.position))) / Constants.TILE_SIZE < Constants.MINION_ATTACK_RANGE;
        }

        public override AI_STATE chooseAction(Characters.Minion m)
        {
            if (chooseNextNode(m))
            {
                return AI_STATE.ATTACK;
            }
            else
            {
                return AI_STATE.NO;
            }
        }
    }
}

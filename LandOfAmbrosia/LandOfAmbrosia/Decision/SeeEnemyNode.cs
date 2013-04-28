using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Common;
using LandOfAmbrosia.Characters;
using Microsoft.Xna.Framework;

namespace LandOfAmbrosia.Decision
{
    class SeeEnemyNode : ADecisionTreeNode
    {
        public override AI_STATE chooseAction(Minion m)
        {
            //If we don't see an enemy, patrol
            if (!chooseNextNode(m))
            {
                return AI_STATE.NO;
            }
            else
            {
                return AI_STATE.YES;
            }
        }

        /// <summary>
        /// Checks if any player is within 20 blocks of the minion. If they are, then this method
        /// sets the minion's target to the character and returns true, otherwise it sets the minion's
        /// target to null and returns false. 
        /// 
        /// If the minion can't see any enemies this method will put the minion in an idle time state
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool chooseNextNode(Minion m)
        {
            //Check if the closest player is within 20 blocks
            if (m.containingLevel.players.Count == 0)
            {
                return false;
            }

            float dist = Vector3.Distance(Constants.UnconvertFromXNAScene(m.position), Constants.UnconvertFromXNAScene(m.containingLevel.players[0].position));
            Character closestPlayer = m.containingLevel.players[0];

            //This probably breaks encapsulation
            for (int i = 1; i < m.containingLevel.players.Count; ++i)
            {
                float checkDist = Vector3.Distance(Constants.UnconvertFromXNAScene(m.position), Constants.UnconvertFromXNAScene(m.containingLevel.players[i].position));
                if (checkDist < dist)
                {
                    dist = checkDist;
                    closestPlayer = m.containingLevel.players[i];
                }
            }

            //Enemy out of sight
            if (dist / Constants.TILE_SIZE > Constants.MINION_SIGHT)
            {
                //m.gotoIdleState();
                return false;
            }
            else
            {
                m.target = closestPlayer;
                return true;
            }
        }
    }
}

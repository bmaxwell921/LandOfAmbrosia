using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandOfAmbrosia.Stats
{
    class AttackPower : AStat
    {
        public AttackPower(float baseValue)
            : base(baseValue)
        {
        }

        /// <summary>
        /// Attack power increases by 10% with each level
        /// </summary>
        public override void levelUpStat()
        {
            this.levelUpStat(0.1f);
        }

        public override void levelUpStat(float perc)
        {
            this.baseValue = baseValue * (1 + perc);
        }
    }
}

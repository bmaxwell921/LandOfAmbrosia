using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandOfAmbrosia.Stats
{
    class Defence : AStat
    {
        public Defence(float baseVal)
            : base(baseVal)
        {
        }

        /// <summary>
        /// Default level up is by 10%
        /// </summary>
        public override void levelUpStat()
        {
            this.levelUpStat(0.1f);
        }

        public override void levelUpStat(float perc)
        {
            this.baseValue = this.baseValue * (1 + perc);
        }
    }
}

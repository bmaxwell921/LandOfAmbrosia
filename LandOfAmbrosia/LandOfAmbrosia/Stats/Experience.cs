using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandOfAmbrosia.Stats
{
    class Experience : AStat
    {
        public Experience(float baseValue)
            : base(baseValue)
        {
            //Override what the super class does because we don't start out with experience
            this.currentValue = 0;
        }

        /// <summary>
        /// The required experience increases by 25% each time you gain a level
        /// </summary>
        public override void levelUpStat()
        {
            this.levelUpStat(0.25f);
        }

        public override void levelUpStat(float perc)
        {
            this.baseValue = baseValue * (1 + perc);
        }

        public override void resetStat()
        {
            //Do nothing!!!
        }
    }
}

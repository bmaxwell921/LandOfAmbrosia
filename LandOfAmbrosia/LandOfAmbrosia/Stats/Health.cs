using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandOfAmbrosia.Stats
{
    class Health : AStat
    {
        public Health(float baseValue)
            : base(baseValue)
        {
        }

        /// <summary>
        /// When we level up, base health will increase by 25%
        /// </summary>
        public override void levelUpStat()
        {
            levelUpStat(0.25f);
        }

        public override void levelUpStat(float perc)
        {
            this.baseValue = this.baseValue * (1 + perc);
            this.currentValue = this.currentValue + (baseValue * perc);
            this.currentValue = (currentValue > baseValue) ? baseValue : currentValue;
        }
    }
}

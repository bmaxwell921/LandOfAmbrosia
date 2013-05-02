using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandOfAmbrosia.Stats
{
    class LevelNumber : AStat
    {
        public LevelNumber() : base(1)
        {
        }

        public override void levelUpStat()
        {
            ++this.baseValue;
        }

        public override void levelUpStat(float perc)
        {
            ++this.baseValue;
        }
    }
}

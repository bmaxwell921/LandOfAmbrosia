using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandOfAmbrosia.Stats
{
    abstract class AStat : IStat
    {
        protected float baseValue;
        protected float currentValue;

        public AStat(float baseValue)
        {
            this.baseValue = baseValue;
            this.currentValue = baseValue;
        }

        public void buffStat(float perc)
        {
            buffDebuff(perc, true);
        }

        public void debuffStat(float perc)
        {
            buffDebuff(perc, false);
        }

        private void buffDebuff(float perc, bool isBuff)
        {
            float buffVal = baseValue * perc;
            currentValue += (isBuff) ? buffVal : -buffVal;
        }

        public abstract void levelUpStat();

        public abstract void levelUpStat(float perc);

        public float getStatVal()
        {
            return currentValue;
        }

        public float getBaseVal()
        {
            return baseValue;
        }

        public void resetStat()
        {
            currentValue = baseValue;
        }
    }
}

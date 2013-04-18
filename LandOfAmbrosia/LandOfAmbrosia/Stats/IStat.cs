using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandOfAmbrosia.Stats
{
    interface IStat
    {
        //Buff and Debuff may not be used...since they are only temporary. We would need to hold onto the buffs that have come thru so
        //if they level up, the given buffs are applied to the new base value.

        /// <summary>
        /// Changes the current stat by the given positive or negative amount
        /// </summary>
        /// <param name="amount"></param>
        void changeStat(float amount);

        /// <summary>
        /// Provides a temporary stat buff by the given percent
        /// </summary>
        /// <param name="perc"></param>
        void buffStat(float perc);

        /// <summary>
        /// Provides a temporary stat debuff by the given percent
        /// </summary>
        /// <param name="perc"></param>
        void debuffStat(float perc);

        /// <summary>
        /// Provides a permanent increase to the stat's base and temporary value
        /// </summary>
        void levelUpStat();

        /// <summary>
        /// Provides a permanent increase to the stat's base value, by the given percent
        /// (For if I let the user choose upgrades) -- Might actually be unnecessary
        /// </summary>
        /// <param name="perc"></param>
        void levelUpStat(float perc);

        /// <summary>
        /// Gets the current value of the stat
        /// </summary>
        /// <returns></returns>
        float getStatVal();

        /// <summary>
        /// Gets the base stat value 
        /// </summary>
        /// <returns></returns>
        float getBaseVal();

        /// <summary>
        /// Resets the current stat value back to the base value
        /// </summary>
        void resetStat();
    }

}

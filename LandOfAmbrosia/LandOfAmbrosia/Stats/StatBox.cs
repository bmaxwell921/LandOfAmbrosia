using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandOfAmbrosia.Stats
{
    /// <summary>
    /// Box to hold onto all the stats
    /// </summary>
    class StatBox
    {
        //Maps, Maps, Maps, I love Maps
        private IDictionary<Type, IStat> stats;

        public StatBox(float baseHealth, float baseAttack, float baseDefence)
        {
            stats = new Dictionary<Type, IStat>();
            stats.Add(typeof(Health), new Health(baseHealth));
            stats.Add(typeof(AttackPower), new AttackPower(baseAttack));
            stats.Add(typeof(Defence), new Defence(baseDefence));
        }

        public bool buffStat(Type statType, float percBuff)
        {
            if (!stats.ContainsKey(statType))
            {
                return false;
            }
            stats[statType].buffStat(percBuff);
            return true;
        }

        public bool debuffStat(Type statType, float percDebuff)
        {
            if (!stats.ContainsKey(statType))
            {
                return false;
            }
            stats[statType].debuffStat(percDebuff);
            return true;
        }

        public bool levelUpStat(Type statType)
        {
            if (!stats.ContainsKey(statType))
            {
                return false;
            }
            stats[statType].levelUpStat();
            return true;
        }

        public bool levelUpStat(Type statType, float perc)
        {
            if (!stats.ContainsKey(statType))
            {
                return false;
            }
            stats[statType].levelUpStat(perc);
            return true;
        }

        public float getStatCurrentVal(Type statType)
        {
            if (!stats.ContainsKey(statType))
            {
                return 0.0f;
            }
            return stats[statType].getStatVal();
        }

        public float getStatBaseVal(Type statType)
        {
            if (!stats.ContainsKey(statType))
            {
                return 0.0f;
            }
            return stats[statType].getBaseVal();
        }
    }
}

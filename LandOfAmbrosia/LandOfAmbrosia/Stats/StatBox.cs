using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandOfAmbrosia.Common;

namespace LandOfAmbrosia.Stats
{
    /// <summary>
    /// Box to hold onto all the stats
    /// </summary>
    class StatBox
    {
        //Maps, Maps, Maps, I love Maps
        private IDictionary<String, IStat> stats;

        public StatBox(float baseHealth, float baseAttack, float baseDefence, float startingExp)
        {
            stats = new Dictionary<String, IStat>();
            stats.Add(Constants.HEALTH_KEY, new Health(baseHealth));
            stats.Add(Constants.ATTACK_KEY, new AttackPower(baseAttack));
            stats.Add(Constants.DEFENCE_KEY, new Defence(baseDefence));
            stats.Add(Constants.EXPERIENCE_KEY, new ExperienceStat(startingExp));
            stats.Add(Constants.LEVEL_NUMBER_KEY, new LevelNumber());
        }

        /// <summary>
        /// Resets all the character's stats when they die, except for experience
        /// </summary>
        public void resetAllStats()
        {
            foreach (String key in stats.Keys)
            {
                if (key != Constants.EXPERIENCE_KEY)
                {
                    stats[key].resetStat();
                }
            }
        }

        public void resetAllNormalStats()
        {
            foreach (String key in stats.Keys)
            {
                if (key != Constants.HEALTH_KEY)
                {
                    stats[key].resetStat();
                }
            }
        }

        public void levelUpAllStats()
        {
            foreach (String key in stats.Keys)
            {
                stats[key].levelUpStat();
            }
            resetAllNormalStats();
        }

        public bool changeCurrentStat(String statType, float amount)
        {
            if (!stats.ContainsKey(statType))
            {
                return false;
            }
            stats[statType].changeStat(amount);
            return true;
        }

        public bool buffStat(String statType, float percBuff)
        {
            if (!stats.ContainsKey(statType))
            {
                return false;
            }
            stats[statType].buffStat(percBuff);
            return true;
        }

        public bool debuffStat(String statType, float percDebuff)
        {
            if (!stats.ContainsKey(statType))
            {
                return false;
            }
            stats[statType].debuffStat(percDebuff);
            return true;
        }

        public bool levelUpStat(String statType)
        {
            if (!stats.ContainsKey(statType))
            {
                return false;
            }
            stats[statType].levelUpStat();
            return true;
        }

        public bool levelUpStat(String statType, float perc)
        {
            if (!stats.ContainsKey(statType))
            {
                return false;
            }
            stats[statType].levelUpStat(perc);
            return true;
        }

        public float getStatCurrentVal(String statType)
        {
            if (!stats.ContainsKey(statType))
            {
                return -1.0f;
            }
            return stats[statType].getStatVal();
        }

        public float getStatBaseVal(String statType)
        {
            if (!stats.ContainsKey(statType))
            {
                return -1.0f;
            }
            return stats[statType].getBaseVal();
        }
    }
}

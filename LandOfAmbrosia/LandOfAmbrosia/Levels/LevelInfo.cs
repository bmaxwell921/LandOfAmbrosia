using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LandOfAmbrosia.Common;

namespace LandOfAmbrosia.Levels
{
    class LevelInfo
    {
        public string levelName;
        public int numLives;
        public int width;
        public int height;
        public int numEnemies;
        public float enemyHealth;
        public float enemyAttack;
        public float enemyDefence;
        public char platformType;
        public Vector3 player1Spawn;
        public Vector3 player2Spawn;

        public LevelInfo(string levelName, int numLives, int width, int height, int numEnemies, float enemyHealth, float enemyAttack, float enemyDefence, char platformType)
            : this(levelName, numLives, width, height, numEnemies, enemyHealth, enemyAttack, enemyDefence, platformType, Constants.DEFAULT_PLAYER1_START, Constants.DEFAULT_PLAYER2_START)
        {
        }

        public LevelInfo(string levelName, int numLives, int width, int height, int numEnemies, float enemyHealth, float enemyAttack, float enemyDefence, char platformType, 
            Vector3 p1Spawn, Vector3 p2Spawn)
        {
            this.levelName = levelName;
            this.numLives = numLives;
            this.width = width;
            this.height = height;
            this.numEnemies = numEnemies;
            this.enemyHealth = enemyHealth;
            this.enemyAttack = enemyAttack;
            this.enemyDefence = enemyDefence;
            this.platformType = platformType;
            this.player1Spawn = p1Spawn;
            this.player2Spawn = p2Spawn;
        }
    }
}

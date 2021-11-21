using System;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu]
    public class LevelSettings : ScriptableObject
    {
        public EnemySettings enemySettings;
        [Range(0, 1)]
        public float difficulty = .5f;

        [Serializable]
        public class EnemySettings
        {
            public int waves = 1;
            public int maxAmountOnWave = 5;
            public float spawnDelay = 0.5f;
            public float waveCooldown = 2;
        }
    }
}
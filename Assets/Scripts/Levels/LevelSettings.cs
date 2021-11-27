using System;
using Entities;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu]
    public class LevelSettings : ScriptableObject
    {
        public WaveSettings[] waves;

        [Serializable]
        public class WaveSettings
        {
            public CombatEntitySettings enemiesSettings;
            public int maxEnemiesOnWave = 5;
            public float enemySpawnDelay = 0.5f;
            public float waveCooldown = 2;
        }
    }
}
using System;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu]
    public class LevelSettings : ScriptableObject
    {
        public WaveSettings waveSettings;

        [Serializable]
        public class WaveSettings
        {
            public float startDelay = 1;
            public int waves = 1;
            public int maxEnemiesOnWave = 5;
            public float enemySpawnDelay = 0.5f;
            public float waveCooldown = 2;
        }
    }
}
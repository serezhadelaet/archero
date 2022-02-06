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
            public float cameraRotY;
            public bool joystickReversedX;
            public bool joystickReversedY;
            public bool joystickSwapYX;

            public Vector2 GetCorrectJoystickDirection(Vector2 initial)
            {
                var normalizedDirection = initial.normalized;
                normalizedDirection.x = joystickReversedX ? -normalizedDirection.x : normalizedDirection.x;
                normalizedDirection.y = joystickReversedY ? -normalizedDirection.y : normalizedDirection.y;
            
                if (joystickSwapYX)
                {
                    var x = normalizedDirection.x;
                    normalizedDirection.x = normalizedDirection.y;
                    normalizedDirection.y = x;
                }

                return normalizedDirection;
            }
        }
    }
}
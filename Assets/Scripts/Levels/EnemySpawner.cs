using System;
using Cysharp.Threading.Tasks;
using Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Levels
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Collider[] spawnColliders;
        [SerializeField] private Enemy enemyPrefab;

        private LevelSettings _levelSettings;
        private CharacterFactory _characterFactory;
        private Level _level;
        private Player _player;

        private int _currentWave;
        private int _aliveEnemiesCount;
        private int _waveSpawnedEnemiesCount;

        private void OnValidate()
        {
            spawnColliders = GetComponentsInChildren<Collider>();
        }

        public void Init(Level level, Player player, LevelSettings levelSettings, CharacterFactory characterFactory)
        {
            _level = level;
            _player = player;
            _levelSettings = levelSettings;
            _characterFactory = characterFactory;

            EnemySpawnRoutine().Forget();
        }

        private async UniTask EnemySpawnRoutine()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_levelSettings.waveSettings.startDelay),
                DelayType.DeltaTime);
            
            while (!ShouldStopSpawn())
            {
                if (ShouldSpawnNewWave())
                    await UniTask.Delay(TimeSpan.FromSeconds(_levelSettings.waveSettings.waveCooldown),
                        DelayType.DeltaTime);

                if (ShouldSpawnEnemy())
                    await UniTask.Delay(TimeSpan.FromSeconds(_levelSettings.waveSettings.enemySpawnDelay),
                        DelayType.DeltaTime);
                
                await UniTask.Yield();
            }
        }

        private bool ShouldSpawnNewWave()
        {
            if (CanSpawnNewWave())
            {
                _currentWave++;
                _waveSpawnedEnemiesCount = 0;
                return true;
            }

            return false;
        }

        private bool ShouldSpawnEnemy()
        {
            if (!CanSpawnEnemy())
                return false;

            _aliveEnemiesCount++;
            _waveSpawnedEnemiesCount++;
            var enemy = _characterFactory.Create(enemyPrefab, GetRandomPos(), default, _level.transform) as Enemy;
            enemy.Init(_player);
            enemy.OnDeath += OnEnemyDeath;
            return true;
        }

        private void OnEnemyDeath()
        {
            _aliveEnemiesCount--;
        }

        private bool CanSpawnEnemy() => _waveSpawnedEnemiesCount < _levelSettings.waveSettings.maxEnemiesOnWave;

        private bool CanSpawnNewWave()
        {
            return _waveSpawnedEnemiesCount == _levelSettings.waveSettings.maxEnemiesOnWave
                   && _levelSettings.waveSettings.waves > _currentWave + 1
                   && _aliveEnemiesCount == 0;
        }

        private bool ShouldStopSpawn()
        {
            var isLastWaveAndAllEnemiesSpawned = _currentWave + 1 == _levelSettings.waveSettings.waves
                                                 && _waveSpawnedEnemiesCount ==
                                                 _levelSettings.waveSettings.maxEnemiesOnWave;
            return _level.HasPassed || isLastWaveAndAllEnemiesSpawned;
        }

        private Vector3 GetRandomPos()
        {
            var coll = spawnColliders[_currentWave];
            var bounds = coll.bounds;
            var x = Random.Range(bounds.min.x, bounds.max.x);
            var y = 0;
            var z = Random.Range(bounds.min.z, bounds.max.z);
            return new Vector3(x, y, z);
        }
    }
}
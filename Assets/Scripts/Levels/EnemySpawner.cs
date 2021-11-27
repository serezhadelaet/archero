using System;
using Cysharp.Threading.Tasks;
using Entities;
using UI;
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

        private int _currentWaveIndex;
        private int _aliveEnemiesCount;
        private int _waveSpawnedEnemiesCount;
        private LevelSettings.WaveSettings _currentWave;
        private WinLoseOverlay _winLoseOverlay;
        
        private void OnValidate()
        {
            spawnColliders = GetComponentsInChildren<Collider>();
        }

        public void Init(Level level, Player player, LevelSettings levelSettings, CharacterFactory characterFactory,
            WinLoseOverlay winLoseOverlay)
        {
            _level = level;
            _player = player;
            _levelSettings = levelSettings;
            _characterFactory = characterFactory;
            _winLoseOverlay = winLoseOverlay;

            _currentWave = _levelSettings.waves[_currentWaveIndex];
            EnemySpawnRoutine().Forget();
        }

        private async UniTask EnemySpawnRoutine()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_currentWave.waveCooldown),DelayType.DeltaTime);
            
            while (!ShouldStopSpawn())
            {
                if (ShouldSpawnNewWave())
                    await UniTask.Delay(TimeSpan.FromSeconds(_currentWave.waveCooldown), DelayType.DeltaTime);

                if (ShouldSpawnEnemy())
                    await UniTask.Delay(TimeSpan.FromSeconds(_currentWave.enemySpawnDelay), DelayType.DeltaTime);
                
                await UniTask.Yield();
            }
        }

        private bool ShouldSpawnNewWave()
        {
            if (CanSpawnNewWave())
            {
                _currentWaveIndex++;
                _currentWave = _levelSettings.waves[_currentWaveIndex];
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
            enemy.Init(_player, _currentWave.enemiesSettings);
            enemy.OnDeath += OnEnemyDeath;
            return true;
        }

        private void OnEnemyDeath()
        {
            _aliveEnemiesCount--;
            
            if (_aliveEnemiesCount == 0 && ShouldStopSpawn())
                _winLoseOverlay.Show();
        }

        private bool CanSpawnEnemy() => _waveSpawnedEnemiesCount < _currentWave.maxEnemiesOnWave;

        private bool CanSpawnNewWave()
        {
            return _waveSpawnedEnemiesCount == _currentWave.maxEnemiesOnWave
                   && _levelSettings.waves.Length > _currentWaveIndex + 1
                   && _aliveEnemiesCount == 0;
        }

        private bool ShouldStopSpawn()
        {
            var isLastWaveAndAllEnemiesSpawned = _currentWaveIndex + 1 == _levelSettings.waves.Length
                                                 && _waveSpawnedEnemiesCount ==
                                                 _currentWave.maxEnemiesOnWave;
            return isLastWaveAndAllEnemiesSpawned;
        }

        private Vector3 GetRandomPos()
        {
            var coll = spawnColliders[_currentWaveIndex];
            var bounds = coll.bounds;
            var x = Random.Range(bounds.min.x, bounds.max.x);
            var y = 0;
            var z = Random.Range(bounds.min.z, bounds.max.z);
            return new Vector3(x, y, z);
        }
    }
}
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Levels
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Collider[] spawnColliders;
        
        private LevelSettings _levelSettings;
        private CharacterFactory _characterFactory;
        private Level _level;
        
        private int _currentWave;
        private int _aliveEnemiesCount;
        private int _waveSpawnedEnemiesCount;

        private void OnValidate()
        { 
            spawnColliders = GetComponentsInChildren<Collider>();
        }

        public void Init(Level level, LevelSettings levelSettings, CharacterFactory characterFactory)
        {
            _level = level;
            _levelSettings = levelSettings;
            _characterFactory = characterFactory;
            
            EnemySpawnRoutine().Forget();
        }
        
        private async UniTask EnemySpawnRoutine()
        {
            while (!ShouldStopSpawn())
            {
                if (ShouldSpawnNewWave())
                    await UniTask.Delay(TimeSpan.FromSeconds(_levelSettings.enemySettings.waveCooldown), DelayType.DeltaTime);
                
                if (ShouldSpawnEnemy())
                    await UniTask.Delay(TimeSpan.FromSeconds(_levelSettings.enemySettings.spawnDelay), DelayType.DeltaTime);
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
            var enemy = _characterFactory.SpawnEnemy(GetRandomPos(), default, _level.transform);
            enemy.Difficulty = _levelSettings.difficulty;
            enemy.OnDeath += OnEnemyDeath;
            return true;
        }
        
        private void OnEnemyDeath()
        {
            _aliveEnemiesCount--;
        }

        private bool CanSpawnEnemy() => _waveSpawnedEnemiesCount < _levelSettings.enemySettings.maxAmountOnWave;

        private bool CanSpawnNewWave()
        {
            return _waveSpawnedEnemiesCount == _levelSettings.enemySettings.maxAmountOnWave
                   && _levelSettings.enemySettings.waves > _currentWave + 1 
                   && _aliveEnemiesCount == 0;
        }
        
        private bool ShouldStopSpawn()
        {
            var isLastWaveAndAllEnemiesSpawned = _currentWave + 1 == _levelSettings.enemySettings.waves
                                                 && _waveSpawnedEnemiesCount == _levelSettings.enemySettings.maxAmountOnWave;
            return _level.HasPassed || isLastWaveAndAllEnemiesSpawned;
        }
        
        private Vector3 GetRandomPos()
        {
            var coll = spawnColliders[_currentWave];
            var bounds = coll.bounds;
            var x = Random.Range(bounds.min.x, bounds.max.x);
            var y = 0;
            var z = Random.Range(bounds.min.z, bounds.max.z);
            return new Vector3(x, y, z) + coll.transform.position;
        }
    }
}
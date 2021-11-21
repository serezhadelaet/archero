using System;
using System.Collections.Generic;
using Cinemachine;
using Entities;
using UnityEngine;

namespace Levels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private CharacterFactory characterFactory;
        [SerializeField] private Transform playerSpawnTr;
        [SerializeField] private LevelSettings settings;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private CinemachineVirtualCamera followCamera;
        
        [NonSerialized] public bool HasPassed;
        private List<Enemy> _aliveEnemies = new List<Enemy>();
        
        private void Awake()
        {
            InitPlayer();
            InitEnemySpawner();
        }

        private void InitEnemySpawner()
        {
            enemySpawner.Init(this, settings, characterFactory);
        }
        
        private void InitPlayer()
        {
            var player = characterFactory.SpawnPlayer(playerSpawnTr.position, playerSpawnTr.rotation, transform);
            followCamera.Follow = player.transform;
            followCamera.LookAt = player.transform;
        }
    }
}
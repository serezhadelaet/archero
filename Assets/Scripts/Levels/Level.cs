using System;
using Cinemachine;
using Entities;
using UI;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Levels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform playerSpawnTr;
        [SerializeField] private LevelSettings settings;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private CinemachineVirtualCamera followCamera;
        [SerializeField] private Player playerPrefab;
        [SerializeField] private NavMeshData navMeshData;
        [SerializeField] private TransformSo followTr;
        
        private NavMeshDataInstance _navMeshInstance;
        private CharacterFactory _characterFactory;
        private Player _player;
        
        [Inject]
        private void Construct(CharacterFactory characterFactory, WinLoseOverlay winLoseOverlay)
        {
            _characterFactory = characterFactory;

            InitNavMesh();
            InitPlayer();
            InitEnemySpawner(winLoseOverlay);
        }

        private void OnDestroy()
        {
            NavMesh.RemoveNavMeshData(_navMeshInstance);
        }

        private void InitNavMesh()
        {
            _navMeshInstance = NavMesh.AddNavMeshData(navMeshData);
        }

        private void InitEnemySpawner(WinLoseOverlay winLoseOverlay)
        {
            enemySpawner.Init(this, _player, settings, _characterFactory, winLoseOverlay);
        }
        
        private void InitPlayer()
        {
            _player = 
                _characterFactory.Create(playerPrefab, playerSpawnTr.position, playerSpawnTr.rotation, transform) as Player;
            followCamera.Follow = followTr.Tr;
            followCamera.LookAt = followTr.Tr;
        }
    }
}
using System;
using DG.Tweening;
using Entities;
using Entities.PlayerComponents;
using Levels;
using UnityEngine;

namespace Helpers
{
    public class FollowPlayer : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private TransformSo followTransformSo;
        [SerializeField] private NewWaveEventSo _waveEventSo;
        
        private void Awake()
        {
            transform.SetParent(null);
            followTransformSo.Tr = transform;
            _waveEventSo.Event += OnNewWave;
        }

        private void OnDestroy()
        {
            _waveEventSo.Event -= OnNewWave;
        }

        private void OnNewWave(LevelSettings.WaveSettings waveSettings)
        {
            transform.DORotate(new Vector3(0, waveSettings.cameraRotY, 0), 0.5f)
                .SetEase(Ease.InOutQuad);
        }

        private void Update()
        {
            transform.position = _player.transform.position;
        }

    }
}
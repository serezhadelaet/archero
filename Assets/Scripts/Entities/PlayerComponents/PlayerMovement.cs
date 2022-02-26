using System;
using Levels;
using UnityEngine;
using UnityEngine.AI;

namespace Helpers
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private NewWaveEventSo _newWaveEventSo;
        
        private Joystick _joystick;
        private NavMeshAgent _navAgent;
        private CharacterAnimations _animations;
        private PlayerDash _dash;
        private LevelSettings.WaveSettings _waveSettings;

        public void Init(Joystick joystick, NavMeshAgent navAgent, CharacterAnimations animations, PlayerDash dash)
        {
            _joystick = joystick;
            _navAgent = navAgent;
            _animations = animations;
            _dash = dash;
            _newWaveEventSo.Event += OnNewWave;
        }

        private void OnNewWave(LevelSettings.WaveSettings waveSettings)
        {
            _waveSettings = waveSettings;
        }

        private void OnDestroy()
        {
            _newWaveEventSo.Event -= OnNewWave;
        }

        private void Update()
        {
            Moving();
        }
        
        private void Moving()
        {
            if (!_navAgent.enabled || _dash.IsDashing())
                return;
            
            var normalizedDirection = _waveSettings.GetCorrectJoystickDirection(_joystick.Direction);

#if UNITY_EDITOR
            normalizedDirection += new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
#endif
            var offset = new Vector3(normalizedDirection.x, 0, normalizedDirection.y);
            _navAgent.SetDestination(transform.position + offset);
            _animations.SetRunSpeed(_navAgent.velocity.magnitude);
        }
    }
}
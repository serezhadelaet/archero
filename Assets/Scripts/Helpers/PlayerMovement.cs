using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Helpers
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private AnimationCurve dashCurve;
        [SerializeField] private float dashJoystickDistanceThreshold = 0.5f;
        [SerializeField] private float dashSpeed = 20;
        [SerializeField] private float dashTime = 0.5f;
        [SerializeField] private float dashTimeToRelease = 0.1f;
        [SerializeField] private ParticleSystem[] dashTrail;
        
        private Joystick _joystick;
        private NavMeshAgent _navAgent;
        private CharacterAnimations _animations;

        private bool IsDashing() => _dashTime > 0;
        private Vector2 _dashDirection;
        private float _dashTime;
        private Vector2 _lastJoystickDir;
        private float _dashCheckTime;
        private bool _canDash = true;

        private IEnumerator _dashCoroutine;
        private float _initialSpeed;
        private Dictionary<ParticleSystem, float> _defaultDashTrailEmissionRate = new Dictionary<ParticleSystem, float>();
        
        public void Init(Joystick joystick, NavMeshAgent navAgent, CharacterAnimations animations)
        {
            foreach (var effect in dashTrail)
                _defaultDashTrailEmissionRate[effect] = effect.emission.rateOverDistanceMultiplier;
            
            EnableTrail(false);
            _joystick = joystick;
            _navAgent = navAgent;
            _animations = animations;
            _initialSpeed = navAgent.speed;
        }
        
        private void Update()
        {
            _dashCheckTime += Time.deltaTime;
            
            TryDash();
            Moving();
            
            if (_dashCheckTime > dashTimeToRelease)
            {
                _lastJoystickDir = _joystick.Direction;
                _dashCheckTime = 0;
            }
        }
        
        private void TryDash()
        {
            if (!_canDash)
                return;
            if (Vector3.Distance(_joystick.Direction,_lastJoystickDir) > dashJoystickDistanceThreshold)
            {
                _dashDirection = _joystick.Direction.normalized;
                _dashTime = 1;
                EnableTrail(true);
                _canDash = false;
                Dash();
            }
        }

        private void Dash()
        {
            if (_dashCoroutine != null)
                StopCoroutine(_dashCoroutine);
            _dashCoroutine = DashCoroutine();
            StartCoroutine(_dashCoroutine);
        }

        private IEnumerator DashCoroutine()
        {
            while (_dashTime > 0)
            {
                var currentDir = _joystick.Direction.normalized;
                if (currentDir == Vector2.zero || !Input.GetMouseButton(0))
                    currentDir = _dashDirection;
                var offset = new Vector3(currentDir.x, 0, currentDir.y);
                _navAgent.SetDestination(transform.position + offset);
                _navAgent.speed = dashCurve.Evaluate(1 - _dashTime) * dashSpeed;
                _dashTime -= Time.deltaTime * (1 / dashTime);
                yield return null;
            }
            _navAgent.speed = _initialSpeed;

            EnableTrail(false);
            _canDash = true;
        }

        private void EnableTrail(bool f)
        {
            foreach (var effect in dashTrail)
            {
                var emission = effect.emission;
                emission.rateOverDistanceMultiplier = f ? _defaultDashTrailEmissionRate[effect] : 0;
            }
        }
        
        private void Moving()
        {
            if (!_navAgent.enabled || IsDashing())
                return;
            var normalizedDirection = _joystick.Direction.normalized;
#if UNITY_EDITOR
            normalizedDirection += new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
#endif
            var offset = new Vector3(normalizedDirection.x, 0, normalizedDirection.y);
            _navAgent.SetDestination(transform.position + offset);
            _animations.SetRunSpeed(_navAgent.velocity.magnitude);
        }
    }
}
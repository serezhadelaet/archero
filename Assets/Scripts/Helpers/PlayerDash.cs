using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.AI;

namespace Helpers
{
    public class PlayerDash : MonoBehaviour
    {
        [SerializeField] private AnimationCurve dashCurve;
        [SerializeField] private float dashSpeed = 20;
        [SerializeField] private float dashTime = 0.5f;
        [SerializeField] private ParticleSystem[] dashTrail;
        [SerializeField] private int dashMaxAmount = 3;
        [SerializeField] private EnemyTakeDamageEvent takeDamageEvent;
        [SerializeField] private DashPerformedEvent dashEvent;
        
        public bool IsDashing() => _dashTime > 0;
        
        private Vector2 _dashDirection;
        private float _dashTime;
        private bool _canDash = true;
        private IEnumerator _dashCoroutine;
        private float _initialSpeed;
        private Dictionary<ParticleSystem, float> _defaultDashTrailEmissionRate = new Dictionary<ParticleSystem, float>();
        private int _dashCounter;
        
        private NavMeshAgent _navAgent;
        private Joystick _joystick;
        private GameOverlay _gameOverlay;
        private RectTransform _joystickRect;
        
        private int DashCounter
        {
            get => _dashCounter;
            set
            {
                _dashCounter = value;
                _gameOverlay.UpdateDashCounter(_dashCounter, dashMaxAmount);
            }
        }
        
        public void Init(NavMeshAgent navAgent, Joystick joystick, GameOverlay gameOverlay)
        {
            _navAgent = navAgent;
            _joystick = joystick;
            _gameOverlay = gameOverlay;
            _initialSpeed = _navAgent.speed;
            _joystickRect = joystick.GetComponent<RectTransform>();
            
            foreach (var effect in dashTrail)
                _defaultDashTrailEmissionRate[effect] = effect.emission.rateOverDistanceMultiplier;
            
            EnableTrail(false);

            takeDamageEvent.Event += ResetDashCounter;
            dashEvent.Event += TryDash;
        }

        private void OnDestroy()
        {
            takeDamageEvent.Event -= ResetDashCounter;
        }

        private void ResetDashCounter()
        {
            DashCounter = 0;
        }
        
        private void TryDash()
        {
            if (!CanPerformDash())
                return;

            if (_joystick.Direction == Vector2.zero)
            {
                DashCounter++;
                _dashDirection = (Input.mousePosition - _joystickRect.position).normalized;
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
                var offset = new Vector3(_dashDirection.x, 0, _dashDirection.y);
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

        private bool CanPerformDash() => DashCounter < dashMaxAmount && _canDash;
    }
}
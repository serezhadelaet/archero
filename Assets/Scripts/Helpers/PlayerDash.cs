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
        [SerializeField] private float dashMaxDistanceFromJoystick = 10f;
        [SerializeField] private float joystickRange = 10f;
        [SerializeField] private float dashSpeed = 20;
        [SerializeField] private float dashTime = 0.5f;
        [SerializeField] private float dashTimeToRelease = 0.1f;
        [SerializeField] private float dashClicksMaxDelay = 0.1f;
        [SerializeField] private ParticleSystem[] dashTrail;
        [SerializeField] private int dashMaxAmount = 3;
        [SerializeField] private EnemyTakeDamageEvent takeDamageEvent;
        
        public bool IsDashing() => _dashTime > 0;
        
        private Vector2 _dashDirection;
        private float _dashTime;
        private Vector2 _lastJoystickDir;
        private float _dashCheckTime;
        private bool _canDash = true;
        private IEnumerator _dashCoroutine;
        private float _initialSpeed;
        private Dictionary<ParticleSystem, float> _defaultDashTrailEmissionRate = new Dictionary<ParticleSystem, float>();
        private int _dashCounter;
        private int _clicksCounter;
        private float _lastClickTime;
        
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
        }

        private void OnDestroy()
        {
            takeDamageEvent.Event -= ResetDashCounter;
        }

        private void Update()
        {
            _dashCheckTime += Time.deltaTime;
            
            TryDash();
            
            if (_dashCheckTime > dashTimeToRelease)
            {
                _lastJoystickDir = _joystick.Direction;
                _dashCheckTime = 0;
            }
        }

        private void ResetDashCounter()
        {
            DashCounter = 0;
        }
        
        private void TryDash()
        {
            if (!CanPerformDash())
                return;

            if (!Input.GetMouseButtonDown(0))
                return;
            
            var inputPos = Input.mousePosition;
            var distance = Vector3.Distance(_joystickRect.position, inputPos);
            if (_joystick.Direction == Vector2.zero && distance > joystickRange && distance < dashMaxDistanceFromJoystick)
            {
                if (_clicksCounter == 1 && Time.time - _lastClickTime > dashClicksMaxDelay)
                    _clicksCounter = 0;
                _clicksCounter++;
                _lastClickTime = Time.time;   
                if (_clicksCounter < 2)
                    return;
                
                _clicksCounter = 0;
                DashCounter++;
                _dashDirection = (inputPos - _joystickRect.position).normalized;
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
using UnityEngine;
using Zenject;

namespace Entities
{
    public class Player : BaseCharacter
    {
        [SerializeField] private float attackingRotationSpeed = 1440;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private float attackRange = 10;
        
        private const float _attackCooldown = 0.1f;
        
        private int _currentLevel;
        private Joystick _joystick;
        private Collider[] _collBuff = new Collider[30];
        private Transform _currentTarget;
        private Vector3 _lastTargetPos;
        private float _lastAttackTime;

        [Inject]
        private void Construct(Joystick joystick)
        {
            _joystick = joystick;
            animations.OnAttacked += Attack;
        }

        private void OnLevelUp()
        {
            _currentLevel++;
            weapon.SetLevel(_currentLevel);
        }

        private void Update()
        {
            var joystickOffset = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);

            Moving(joystickOffset);
            InstantiateAttack(joystickOffset.magnitude);

            if (joystickOffset.magnitude > 0)
            {
                animations.Attack(false);
            }
            
            if (!Input.GetMouseButton(0) && animations.IsAttacking()
                && _currentTarget)
            {
                _lastTargetPos = _currentTarget.position;
                var targetDir = (_lastTargetPos - transform.position).normalized;
                var targetRot = Quaternion.LookRotation(targetDir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot,
                    Time.deltaTime * attackingRotationSpeed);
            }
        }

        private void Moving(Vector3 offset)
        {
            navAgent.SetDestination(transform.position + offset);
            animations.SetRunSpeed(offset.magnitude);
        }

        private void InstantiateAttack(float magnitude)
        {
            if (!CanAttack() || magnitude >= 0.1f || Time.time < _lastAttackTime + _attackCooldown)
                return;
            _lastAttackTime = Time.time;
            _currentTarget = GetNearestEnemy();
            if (_currentTarget)
            {
                animations.Attack(true);
            }
        }

        private void Attack()
        {
            if (animations.IsAttacking() && !Input.GetMouseButton(0))
            {
                transform.LookAt(_lastTargetPos);
                weapon.Attack(_lastTargetPos);
            }
        }

        private Transform GetNearestEnemy()
        {
            var count = Physics.OverlapSphereNonAlloc(transform.position, attackRange, _collBuff, LayerMask.GetMask("Enemy"));
            var minDistance = float.MaxValue;
            Transform nearestEntity = null;
            for (int i = 0; i < count; i++)
            {
                var coll = _collBuff[i];
                var combatEntity = coll.GetComponentInParent<BaseCombatEntity>();
                if (combatEntity.IsDead())
                    continue;

                if (Physics.Linecast(transform.position, combatEntity.transform.position, obstacleLayer))
                    continue;
                
                var distance = Vector3.Distance(transform.position, coll.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEntity = combatEntity.transform;
                }
            }

            return nearestEntity;
        }

        private bool CanAttack() => !Input.GetMouseButton(0) && !animations.IsAttacking();
    }
}
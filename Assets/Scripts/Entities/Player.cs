using UnityEngine;
using Zenject;

namespace Entities
{
    public class Player : BaseCharacter
    {
        [SerializeField] private float attackingRotationSpeed = 1440;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private float attackRange = 10;
        
        private const float AttackCooldown = 0.1f;

        private int _currentLevel;
        private Joystick _joystick;
        private Collider[] _collBuff = new Collider[30];
        private BaseCombatEntity _currentTarget;
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
            Moving();
            InstantiateAttack();
            TryToCancelAttack();
            FollowTarget();
        }

        private void Moving()
        {
            var offset = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
            navAgent.SetDestination(transform.position + offset);
            animations.SetRunSpeed(offset.magnitude);
        }

        private void InstantiateAttack()
        {
            var nearestEnemy = GetNearestEnemy();
            if (nearestEnemy != null)
                _currentTarget = nearestEnemy;
            if (!CanAttack())
                return;
            
            _lastAttackTime = Time.time;
            if (_currentTarget != null)
            {
                _lastTargetPos = _currentTarget.transform.position;
            }
            animations.Attack(_currentTarget != null);
        }

        private void FollowTarget()
        {
            if (!ShouldFollowTarget())
                return;
            
            _lastTargetPos = _currentTarget.transform.position;
            var targetDir = (_lastTargetPos - transform.position).normalized;
            var targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot,
                Time.deltaTime * attackingRotationSpeed);
        }

        private void TryToCancelAttack()
        {
            if (IsMoving() || !CanSee(_lastTargetPos) || (_currentTarget != null && _currentTarget.IsDead()))
                animations.Attack(false);
        }
        
        private void Attack()
        {
            if (IsMoving())
                return;
            
            transform.LookAt(_lastTargetPos);
            weapon.Attack(_lastTargetPos);
        }

        private BaseCombatEntity GetNearestEnemy()
        {
            var count = Physics.OverlapSphereNonAlloc(transform.position, attackRange, _collBuff, targetLayer);
            var minDistance = float.MaxValue;
            BaseCombatEntity nearestEntity = null;
            for (int i = 0; i < count; i++)
            {
                var coll = _collBuff[i];
                var combatEntity = coll.GetComponentInParent<BaseCombatEntity>();
                if (combatEntity.IsDead())
                    continue;

                if (!CanSee(coll.transform.position))
                    continue;

                var distance = Vector3.Distance(transform.position, coll.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEntity = combatEntity;
                }
            }

            return nearestEntity;
        }

        private bool ShouldFollowTarget() => !IsMoving() && animations.IsAttacking() && _currentTarget;
        private bool IsMoving() => Input.GetMouseButton(0) || _joystick.Horizontal > 0 || _joystick.Vertical > 0;

        private bool CanAttack() => !animations.IsAttacking() && !IsMoving() &&
                                    Time.time > _lastAttackTime + AttackCooldown;

        private bool CanSee(Vector3 pos) => !Physics.Linecast(transform.position, pos, obstacleLayer);
    }
}
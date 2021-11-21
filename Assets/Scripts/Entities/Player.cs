using UnityEngine;
using Zenject;

namespace Entities
{
    public class Player : BaseCharacter
    {
        private int _currentLevel;
        private Joystick _joystick;
        private Collider[] _collBuff = new Collider[30];
        private Transform _currentTarget;
        private Vector3 _lastTargetPos;

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

            if (animations.IsAttacking())
            {
                
            }
        }

        private void Moving()
        {
            var offset = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
            navAgent.SetDestination(transform.position + offset);
            animations.SetRunSpeed(_joystick.Horizontal + _joystick.Vertical);
        }
        
        private void InstantiateAttack()
        {
            if (!CanAttack())
                return;
            _currentTarget = GetNearestEnemy();
            if (_currentTarget)
            {
                animations.Attack();
            }
        }
        
        private void Attack()
        {
            transform.LookAt(_lastTargetPos);
            weapon.Attack(_lastTargetPos);
        }
        
        private Transform GetNearestEnemy()
        {
            var count = Physics.OverlapSphereNonAlloc(transform.position, 5, _collBuff, LayerMask.GetMask("Enemy"));
            var minDistance = float.MaxValue;
            Transform nearestEntity = null;
            for (int i = 0; i < count; i++)
            {
                var coll = _collBuff[i];
                var combatEntity = coll.GetComponentInParent<BaseCombatEntity>();
                if (combatEntity.IsDead())
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
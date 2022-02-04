using System;
using Entities;
using Extensions;
using Interfaces;
using UnityEngine;

namespace Helpers
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private float attackRange = 10;
        
        private const float AttackCooldown = 0.1f;
        
        private Collider[] _collBuff = new Collider[30];
        private float _lastAttackTime;
        
        private CharacterAnimations _animations;
        private Player _player;
        private LayerMask _targetLayer;
        private PlayerProgressionFollower _playerProgression;
        private PlayerDash _dash;
        
        public void Init(CharacterAnimations animations, Player player, LayerMask targetLayer,
            PlayerProgressionFollower playerProgressionFollower, PlayerDash dash)
        {
            _animations = animations;
            _player = player;
            _targetLayer = targetLayer;
            _playerProgression = playerProgressionFollower;
            _dash = dash;
            
            animations.OnAttacked += Attack;
        }

        private void OnDestroy()
        {
            _animations.OnAttacked -= Attack;
        }

        private void Update()
        {
            if (_player.IsDead)
                return;
            
            InstantiateAttack();
            TryToCancelAttack();
        }
        
        private void InstantiateAttack()
        {
            var nearestEnemy = GetNearestEnemy();
            if (nearestEnemy != null)
                _player.CurrentTarget = nearestEnemy;
            if (!CanAttack())
                return;
            
            _lastAttackTime = Time.time;
            if (_player.CurrentTarget != null)
            { 
                _player.LastTargetPos = _player.CurrentTarget.transform.position;
            }
            
            _animations.Attack(_player.CurrentTarget != null);
        }

        private void TryToCancelAttack()
        {
            if (IsMoving() 
                || !CanSee(_player.LastTargetPos) 
                || (_player.CurrentTarget != null && _player.CurrentTarget.IsDead))
                _animations.Attack(false);
        }
        
        private void Attack()
        {
            if (IsMoving())
                return;
            
            transform.LookAt(_player.LastTargetPos);
            var weapon = _player.GetWeapon();
            weapon.SetLevel(_playerProgression.GetLevel());
            weapon.Attack(_player.LastTargetPos);
        }
        
        private IDamageable GetNearestEnemy()
        {
            var count = Physics.OverlapSphereNonAlloc(transform.position, attackRange, _collBuff, _targetLayer);
            var minDistance = float.MaxValue;
            IDamageable nearestDamageable = null;
            for (int i = 0; i < count; i++)
            {
                var coll = _collBuff[i];
                var combatEntity = coll.GetDamageable();
                if (combatEntity == null || combatEntity.IsDead)
                    continue;

                if (!CanSee(coll.transform.position))
                    continue;

                var distance = Vector3.Distance(transform.position, coll.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestDamageable = combatEntity;
                }
            }

            return nearestDamageable;
        }
        
        private bool IsMoving() => Input.GetMouseButton(0) || _dash.IsDashing();
        private bool CanAttack() => !_animations.IsAttacking() && !IsMoving() && Time.time > _lastAttackTime + AttackCooldown;
        private bool CanSee(Vector3 pos) => !Physics.Linecast(transform.position, pos, obstacleLayer);
    }
}
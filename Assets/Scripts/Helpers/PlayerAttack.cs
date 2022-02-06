using System.Collections.Generic;
using Combat;
using Combat.Projectiles;
using Combat.Weapons;
using Entities;
using Extensions;
using Interfaces;
using UnityEngine;

namespace Helpers
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private Transform rangeTr;
        [SerializeField] private Transform meleeTr;
        [SerializeField] private Transform rangeDisarmedTr;
        [SerializeField] private Transform meleeDisarmedTr;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private float attackRange = 10;
        [SerializeField] private float meleeRange = 1;
        [SerializeField] private MeleeWeapon _meleeWeapon;
        [SerializeField] protected WeaponSettings _meleeWeaponSettings;
        
        private const float AttackCooldown = 0.1f;
        
        private Collider[] _collBuff = new Collider[30];
        private float _lastAttackTime;
        
        private CharacterAnimations _animations;
        private Player _player;
        private LayerMask _targetLayer;
        private PlayerProgressionFollower _playerProgression;
        private PlayerDash _dash;
        private AttackType _currentAttackType;

        private enum AttackType
        {
            Melee,
            Range
        }
        
        public void Init(CharacterAnimations animations, Player player, LayerMask targetLayer,
            PlayerProgressionFollower playerProgressionFollower, PlayerDash dash)
        {
            _animations = animations;
            _player = player;
            _targetLayer = targetLayer;
            _playerProgression = playerProgressionFollower;
            _dash = dash;
            _meleeWeapon.Init(_player, _targetLayer, _meleeWeaponSettings);
            
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
            
            SetMeleeOrRangedAttackType();
            InstantiateAttack();
            TryToCancelAttack();
        }

        private void SetMeleeOrRangedAttackType()
        {
            if (_currentAttackType == AttackType.Melee && _animations.IsAttackingMelee())
                return;
            if (_currentAttackType == AttackType.Range && _animations.IsAttacking())
                return;
            
            _currentAttackType = _player.CurrentTarget != null &&
                                 _player.CurrentTarget.transform.position.XZDistance(transform.position) <= meleeRange
                                ? AttackType.Melee : AttackType.Range;
            rangeTr.gameObject.SetActive(_currentAttackType == AttackType.Range);
            meleeTr.gameObject.SetActive(_currentAttackType == AttackType.Melee);
            rangeDisarmedTr.gameObject.SetActive(_currentAttackType == AttackType.Melee);
            meleeDisarmedTr.gameObject.SetActive(_currentAttackType == AttackType.Range);
        }
        
        private void InstantiateAttack()
        {
            var nearestEnemy = GetNearestEnemy();
            if (nearestEnemy == null)
            {
                _player.CurrentTarget = null;
                return;
            }
            
            _player.CurrentTarget = nearestEnemy;
            
            if (!CanAttack())
                return;
            
            _lastAttackTime = Time.time;
            _player.LastTargetPos = _player.CurrentTarget.transform.position;

            if (_currentAttackType == AttackType.Melee)
                _animations.AttackMelee(true);
            else
                _animations.Attack(true);
        }

        private void TryToCancelAttack()
        {
            if (_player.CurrentTarget == null
                || (IsMoving())
                || !CanSee(_player.LastTargetPos)
                || (_player.CurrentTarget != null && _player.CurrentTarget.IsDead))
            {
                _animations.Attack(false);
                _animations.AttackMelee(false);
            }
        }
        
        private void Attack()
        {
            if (IsMoving())
                return;
            
            transform.LookAt(_player.LastTargetPos);

            var weapon = _currentAttackType == AttackType.Melee ? _meleeWeapon : _player.GetWeapon();
            weapon.SetLevel(_playerProgression.GetLevel());
            weapon.Attack(_player.LastTargetPos);
        }

        private IDamageable GetNearestEnemy()
        {
            var list = new List<IDamageable>();
            var count = Physics.OverlapSphereNonAlloc(transform.position, attackRange, _collBuff, _targetLayer);
            var minDistance = float.MaxValue;
            IDamageable nearestDamageable = null;
            for (int i = 0; i < count; i++)
            {
                var coll = _collBuff[i];
                var combatEntity = coll.GetDamageable();
                if (combatEntity == null || combatEntity.IsDead || list.Contains(combatEntity))
                    continue;

                if (_currentAttackType == AttackType.Range && !CanSee(coll.transform.position))
                    continue;
                
                list.Add(combatEntity);
                var distance = transform.position.XZDistance(coll.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestDamageable = combatEntity;
                }
            }

            return nearestDamageable;
        }
        
        private bool IsMoving() => Input.GetMouseButton(0) || _dash.IsDashing();
        private bool CanAttack() => !_animations.IsAttacking() && !_animations.IsAttackingMelee() 
                                                               && !IsMoving() && Time.time > _lastAttackTime + AttackCooldown;
        private bool CanSee(Vector3 pos) => !Physics.Linecast(transform.position, pos, obstacleLayer);
    }
}
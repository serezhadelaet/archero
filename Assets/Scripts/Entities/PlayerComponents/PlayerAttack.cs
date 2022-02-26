using System.Collections.Generic;
using Combat.Weapons;
using Entities.PlayerComponents;
using Entities.PlayerComponents.AttackStates;
using UnityEngine;

namespace Helpers
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private Transform rangeTr;
        [SerializeField] private Transform meleeTr;
        [SerializeField] private Transform rangeDisarmedTr;
        [SerializeField] private Transform meleeDisarmedTr;
        [SerializeField] private float attackRange = 10;
        [SerializeField] private float meleeRange = 1;
        [SerializeField] private MeleeWeapon _meleeWeapon;
        [SerializeField] protected WeaponSettings _meleeWeaponSettings;
        [SerializeField] protected CombatTargetingSo _combatTargeting;
        
        private const float AttackCooldown = 0.1f;
        
        private Collider[] _collBuff = new Collider[30];
        private float _lastAttackTime;
        
        private CharacterAnimations _animations;
        private Player _player;
        private LayerMask _targetLayer;
        private PlayerProgressionFollower _playerProgression;
        private AttackStateMachine _attackStateMachine;
        private PlayerDash _dash;

        public void Init(CharacterAnimations animations, Player player, LayerMask targetLayer,
            PlayerProgressionFollower playerProgressionFollower, PlayerDash dash)
        {
            _animations = animations;
            _player = player;
            _targetLayer = targetLayer;
            _playerProgression = playerProgressionFollower;
            _dash = dash;
            _meleeWeapon.Init(_player, _targetLayer, _meleeWeaponSettings);

            InitStateMachine(animations, player, dash);

            animations.OnAttacked += AttackRange;
            animations.OnAttackedMelee += AttackMelee;
        }

        private void InitStateMachine(CharacterAnimations animations, Player player, PlayerDash dash)
        {
            var dataRange = new Data()
            {
                Animations = animations,
                CombatTargeting = _combatTargeting,
                Dash = dash,
                Player = player,
                RadiusMelee = meleeRange,
                RadiusRange = attackRange,
                WeaponModel = rangeTr.gameObject,
                WeaponModelDisarmed = rangeDisarmedTr.gameObject
            };
            
            var dataMelee = new Data()
            {
                Animations = animations,
                CombatTargeting = _combatTargeting,
                Dash = dash,
                Player = player,
                RadiusMelee = meleeRange,
                RadiusRange = attackRange,
                WeaponModel = meleeTr.gameObject,
                WeaponModelDisarmed = meleeDisarmedTr.gameObject
            };

            var rangeAttackState = new RangeAttackState(dataRange);
            var meleeAttackState = new MeleeAttackState(dataMelee);

            _attackStateMachine = new AttackStateMachine(rangeAttackState, meleeAttackState);
            _attackStateMachine.SetState(_attackStateMachine.RangeAttackState);
        }

        private void OnDestroy()
        {
            _animations.OnAttacked -= AttackRange;
            _animations.OnAttackedMelee -= AttackMelee;
            _attackStateMachine?.Exit();
        }

        private void Update()
        {
            _attackStateMachine?.Update();
        }

        private void AttackMelee()
        {
            if (IsMoving())
                return;
            
            transform.LookAt(_player.LastTargetPos);
            _meleeWeapon.SetLevel(_playerProgression.GetLevel());
            _meleeWeapon.Attack(_player.LastTargetPos);
        }
        
        private void AttackRange()
        {
            if (IsMoving())
                return;
            
            transform.LookAt(_player.LastTargetPos);
            _player.GetWeapon().SetLevel(_playerProgression.GetLevel());
            _player.GetWeapon().Attack(_player.LastTargetPos);
        }

        private bool IsMoving() => _dash.IsDashing() || Input.GetMouseButton(0);
    }
}
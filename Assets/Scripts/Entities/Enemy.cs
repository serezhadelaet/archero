using Combat;
using Helpers;
using NaughtyAttributes;
using UnityEngine;

namespace Entities
{
    public class Enemy : BaseCharacter
    {
        [SerializeField] private PlayerProgressionFollower playerProgressionFollower;
        [MinMaxSlider(0, 100)]
        [SerializeField] private Vector2 attackRange;
        [SerializeField] private float followRange = 10;
        [SerializeField] private float stopDistance = 2;
        [SerializeField] private EnemyTakeDamageEvent takeDamageEvent;

        private float _attackRange;
        private float _stopDistance;
        
        public void Init(Player player, CombatEntitySettings combatEntitySettings)
        {
            combatSettings = combatEntitySettings;
            CurrentTarget = player;
            navAgent.stoppingDistance = stopDistance;
            _attackRange = Random.Range(attackRange.x, attackRange.y);
            _stopDistance = _attackRange - 1;
            animations.OnAttacked += Shoot;
            
            SetWeapon();
            SetHealth();
            SetLevel();
            weapon.SetLevel(Level);
        }

        protected override void Update()
        {
            base.Update();
            if (IsDead)
                return;

            var distanceToPlayer = Vector3.Distance(CurrentTarget.transform.position, transform.position);
            MoveToPlayer(distanceToPlayer);
            TryToAttackPlayer(distanceToPlayer);
        }

        private void MoveToPlayer(float distance)
        {
            animations.SetRunSpeed(navAgent.velocity.magnitude);
            
            if (ShouldStopMoving(distance))
            {
                navAgent.isStopped = true;
                return;
            }
            
            navAgent.isStopped = false;    
            navAgent.SetDestination(CurrentTarget.transform.position);
        }

        public override void TakeDamage(HitInfo hitInfo)
        {
            base.TakeDamage(hitInfo);
            takeDamageEvent.Invoke();
        }

        private void TryToAttackPlayer(float distance)
        {
            animations.Attack(CanAttackPlayer(distance));
        }

        protected override bool ShouldFollowTarget() => animations.IsAttacking();

        private void Shoot()
        {
            weapon.Attack(CurrentTarget.transform.position);
        }

        protected override void OnDead()
        {
            base.OnDead();
            playerProgressionFollower.Progress();
        }

        private bool ShouldStopMoving(float distance) => distance > followRange || distance < _stopDistance;
        private bool CanAttackPlayer(float distance) => distance < _attackRange && navAgent.velocity.magnitude < 0.1f;
        protected override bool CanDamageMyself() => true;
    }
}
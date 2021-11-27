using Helpers;
using UnityEngine;

namespace Entities
{
    public class Enemy : BaseCharacter
    {
        [SerializeField] private PlayerProgressionFollower playerProgressionFollower;
        [SerializeField] private float attackRange = 5;
        [SerializeField] private float followRange = 10;
        
        public void Init(Player player, CombatEntitySettings combatEntitySettings)
        {
            combatSettings = combatEntitySettings;
            CurrentTarget = player;
            navAgent.stoppingDistance = followRange - attackRange;
            animations.OnAttacked += Shoot;
            
            SetWeapon();
            SetHealth();
        }

        protected override void Update()
        {
            base.Update();
            if (IsDead())
                return;

            var distanceToPlayer = Vector3.Distance(CurrentTarget.transform.position, transform.position);
            MoveToPlayer(distanceToPlayer);
            TryToAttackPlayer(distanceToPlayer);
        }

        private void MoveToPlayer(float distance)
        {
            if (distance < followRange)
                navAgent.SetDestination(CurrentTarget.transform.position);
            animations.SetRunSpeed(navAgent.velocity.magnitude);
        }

        private void TryToAttackPlayer(float distance)
        {
            animations.Attack(distance < attackRange);
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
    }
}
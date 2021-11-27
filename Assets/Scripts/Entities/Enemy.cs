using Helpers;
using UnityEngine;

namespace Entities
{
    public class Enemy : BaseCharacter
    {
        [SerializeField] private PlayerProgressionFollower playerProgressionFollower;
        [SerializeField] private float attackRange = 5;
        [SerializeField] private float followRange = 10;
        [SerializeField] private float stopDistance = 2;
        
        public void Init(Player player, CombatEntitySettings combatEntitySettings)
        {
            combatSettings = combatEntitySettings;
            CurrentTarget = player;
            navAgent.stoppingDistance = followRange - attackRange;
            animations.OnAttacked += Shoot;
            
            SetWeapon();
            SetHealth();
            SetLevel();
            weapon.SetLevel(_level);
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

        public override void Heal(float hp)
        {
            Debug.Log(name + " was heal on " + hp);
            base.Heal(hp);
        }

        private void MoveToPlayer(float distance)
        {
            animations.SetRunSpeed(navAgent.velocity.magnitude);
            
            if (distance > followRange || distance < stopDistance)
            {
                navAgent.isStopped = true;
                return;
            }
            navAgent.isStopped = false;    
            navAgent.SetDestination(CurrentTarget.transform.position);
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

        protected override bool CanDamageMyself() => true;
    }
}
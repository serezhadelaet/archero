using UnityEngine;

namespace Entities
{
    public class Enemy : BaseCharacter
    {
        private const float Range = 10;
        private const float AttackRange = 5;
        private const float AttackCooldown = 1;
        
        private Player _player;
        private float _lastAttackTime;

        public void Init(Player player)
        {
            _player = player;
            
            navAgent.stoppingDistance = Range - AttackRange;
        }
        
        private void Update()
        {
            if (IsDead())
                return;
            
            var distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            MoveToPlayer(distanceToPlayer);
            AttackPlayer(distanceToPlayer);
        }

        private void MoveToPlayer(float distance)
        {
            if (distance < Range)
                navAgent.SetDestination(_player.transform.position);
            animations.SetRunSpeed(navAgent.velocity.magnitude);
        }

        private void AttackPlayer(float distance)
        {
            if (distance < AttackRange && Time.time > _lastAttackTime + AttackCooldown)
            {
                _lastAttackTime = Time.time;
            }
        }
    }
}
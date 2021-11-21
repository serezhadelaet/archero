using UnityEngine;

namespace Entities
{
    public class Enemy : BaseCharacter
    {
        private const float Range = 10;
        private const float AttackRange = 5;
        private const float AttackCooldown = 1;
        
        private float _difficulty;
        private Player _player;
        private float _lastAttackTime;
        
        protected override float GetArmor() => _difficulty;

        public void Init(Player player, float difficulty)
        {
            _player = player;
            _difficulty = difficulty;
            
            navAgent.stoppingDistance = Range - AttackRange;
        }
        
        private void Update()
        {
            var distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            MoveToPlayer(distanceToPlayer);
            AttackPlayer(distanceToPlayer);
        }

        private void MoveToPlayer(float distance)
        {
            if (distance < Range)
                navAgent.SetDestination(_player.transform.position);
        }

        private void AttackPlayer(float distance)
        {
            if (distance < AttackRange && Time.time > _lastAttackTime + AttackCooldown)
            {
                _lastAttackTime = Time.time;
                Debug.Log(name + " Attack");
            }
        }
    }
}
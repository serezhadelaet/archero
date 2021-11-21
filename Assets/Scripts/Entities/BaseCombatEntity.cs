using System;
using Interfaces;
using UnityEngine;
using Combat;

namespace Entities
{
    public class BaseCombatEntity : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100;
        
        public event Action OnDeath;
        public event Action OnTakeDamage;
        public bool IsDead() => _health <= 0;

        private float _health;
        
        protected virtual void Awake()
        {
            _health = maxHealth;
        }
        
        public void TakeDamage(HitInfo hitInfo)
        {
            if (hitInfo.Damage > 0)
            {
                _health -= hitInfo.Damage;
                OnTakeDamage?.Invoke();
                
                if (_health <= 0)
                    OnDeath?.Invoke();

                hitInfo.Projectile.OnHit(this, hitInfo.Damage);
            }
        }

        public void Heal(float hp)
        {
            _health += hp;
        }
    }
}
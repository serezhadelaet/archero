using System;
using Interfaces;
using UnityEngine;
using Combat;

namespace Entities
{
    public abstract class BaseCombatEntity : MonoBehaviour, IDamageable
    {
        [SerializeField] protected CombatEntitySettings combatSettings;
        
        public event Action OnDeath;
        public event Action<float> OnHealthChanged;
        public bool IsDead() => _health <= 0;
        protected float Health
        {
            get => _health;
            private set
            {
                _health = value;
                OnHealthChanged?.Invoke(_health);
            }
        }

        private float _maxHealth;
        private float _health;
        
        protected virtual void SetHealth()
        {
            _maxHealth = combatSettings.health;
            Health = _maxHealth;
        }

        public virtual void TakeDamage(HitInfo hitInfo)
        {
            if (IsDead() || (hitInfo.Initiator == this && !CanDamageMyself()))
                return;
            
            if (hitInfo.Damage > 0)
            {
                Health = Mathf.Max(_health - hitInfo.Damage, 0);

                if (Health <= 0)
                {
                    OnDeath?.Invoke();
                    OnDead();
                }
                
                hitInfo.Projectile.OnHit(this, hitInfo.Damage);
            }
        }

        protected virtual void OnDead() { }
        
        public virtual void Heal(float hp)
        {
            if (!IsDead())
                Health = Mathf.Min(_health + hp, _maxHealth);
        }

        protected virtual bool CanDamageMyself() => false;
    }
}
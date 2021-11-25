using System;
using Interfaces;
using UnityEngine;
using Combat;

namespace Entities
{
    public abstract class BaseCombatEntity : MonoBehaviour, IDamageable
    {
        [SerializeField] private CombatEntitySettings combatSettings;
        
        public event Action OnDeath;
        public event Action OnTakeDamage;
        public bool IsDead() => _health <= 0;

        private float _health;
        
        protected virtual void Awake()
        {
            _health = combatSettings.health;
        }
        
        public virtual void TakeDamage(HitInfo hitInfo)
        {
            if (hitInfo.Damage > 0)
            {
                _health -= hitInfo.Damage;
                OnTakeDamage?.Invoke();

                if (_health <= 0)
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
            Debug.Log("On heal " + name + " " + hp);
            _health += hp;
        }
    }
}
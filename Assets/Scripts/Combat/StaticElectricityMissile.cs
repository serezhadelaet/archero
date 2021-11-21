using System;
using Combat.Projectiles;
using Entities;

namespace Combat
{
    public class StaticElectricityMissile : BaseProjectile
    {
        public event Action OnTargetDead;
        public event Action<BaseCombatEntity> OnArrived;

        private BaseCombatEntity _target;
        
        public void SetNextTarget(BaseCombatEntity target)
        {
            if (target.IsDead())
                OnTargetDead.Invoke();
        }

        public void FixedUpdate()
        {
            
        }

        private void OnLerpEnd()
        {
            OnArrived.Invoke(_target);
        }

        public void Kill()
        {
            Destroy(gameObject);
        }
    }
}
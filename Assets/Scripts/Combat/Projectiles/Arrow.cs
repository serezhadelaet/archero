using Interfaces;
using UnityEngine;

namespace Combat.Projectiles
{
    public class Arrow : BaseProjectile
    {
        [SerializeField] private StraightMovingDamager straightMovingDamager;

        private Vector3 _direction;
        
        public override void Shoot(Vector3 direction)
        {
            base.Shoot(direction);
            _direction = direction;
            straightMovingDamager.Init(direction, TargetLayerMask, DoHit);
        }

        private void DoHit(IDamageable damageable, Collider coll)
        {
            damageable.TakeDamage(new HitInfo(this, Damage, Owner, coll, _direction));
            Destroy(gameObject);
        }
    }
}
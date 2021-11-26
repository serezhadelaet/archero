using Interfaces;
using UnityEngine;

namespace Combat.Projectiles
{
    public class StaticElectricityArrow : StaticElectricityMissileProjectile
    {
        [SerializeField] private StraightDirectionMoving straightDirectionMoving;

        private Vector3 _direction;
        
        public override void Shoot(Vector3 direction)
        {
            base.Shoot(direction);
            _direction = direction;
            straightDirectionMoving.Init(direction, TargetLayerMask, DoHit);
        }
        
        private void DoHit(IDamageable damageable, Collider coll)
        {
            damageable.TakeDamage(new HitInfo(this, Damage, Owner, coll, _direction));
            straightDirectionMoving.IsStopped = true;
        }
    }
}
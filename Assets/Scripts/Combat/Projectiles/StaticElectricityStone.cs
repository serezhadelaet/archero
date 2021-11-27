using Combat.Projectiles.MovingDamagers;
using Interfaces;
using UnityEngine;

namespace Combat.Projectiles
{
    public class StaticElectricityStone : StaticElectricityMissileProjectile
    {
        [SerializeField] private MovingDamager movingDamager;

        private Vector3 _direction;
        
        public override void Shoot(Vector3 direction)
        {
            base.Shoot(direction);
            _direction = direction;
            movingDamager.Init(direction, TargetLayerMask, DoHit, Owner);
        }
        
        private void DoHit(IDamageable damageable, Collider coll)
        {
            damageable?.TakeDamage(new HitInfo(this, Damage, Owner, coll, _direction));
            movingDamager.IsStopped = true;
        }
    }
}
using Combat.Projectiles.MovingDamagers;
using Interfaces;
using UnityEngine;

namespace Combat.Projectiles
{
    public class StoneProjectile : BaseProjectile
    {
        [SerializeField] private MovingDamager movingDamager;
        [SerializeField] private GameObject hitEffect;
        
        public override void Shoot(Vector3 pos)
        {
            movingDamager.Init(pos, TargetLayerMask, DoHit);
        }
        
        private void DoHit(IDamageable damageable, Collider coll)
        {
            damageable?.TakeDamage(new HitInfo(this, Damage, Owner, coll, Vector3.one));
            Instantiate(hitEffect, transform.position, default);
            Destroy(gameObject);
        }
    }
}
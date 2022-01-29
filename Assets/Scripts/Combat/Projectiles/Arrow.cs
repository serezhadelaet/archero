using Interfaces;
using UnityEngine;

namespace Combat.Projectiles
{
    public class Arrow : BaseProjectile
    {
        protected override void DoHit(IDamageable damageable, Collider coll)
        {
            base.DoHit(damageable, coll);
            ReturnToPool();
        }
    }
}
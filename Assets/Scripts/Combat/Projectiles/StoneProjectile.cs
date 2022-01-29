using Interfaces;
using UnityEngine;

namespace Combat.Projectiles
{
    public class StoneProjectile : BaseProjectile
    {
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private Transform[] trails;
        
        protected override void DoHit(IDamageable damageable, Collider coll)
        {
            base.DoHit(damageable, coll);
            Instantiate(hitEffect, transform.position, default);
            ReturnToPool();
        }

        private void FreeTrails()
        {
            foreach (var trail in trails)
            {
                trail.SetParent(null);
                Destroy(trail.gameObject, 0.5f);
            }
        }
    }
}
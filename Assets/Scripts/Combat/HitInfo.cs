using Combat.Projectiles;
using Entities;
using UnityEngine;

namespace Combat
{
    public struct HitInfo
    {
        public BaseProjectile Projectile;
        public float Damage;
        public BaseCombatEntity Initiator;
        public Collider HitCollider;
        public Vector3 ForceDir;

        public HitInfo(BaseProjectile projectile, float damage, BaseCombatEntity initiator, Collider hitCollider = null,
            Vector3 forceDir = default)
        {
            Projectile = projectile;
            Damage = damage;
            Initiator = initiator;
            HitCollider = hitCollider;
            ForceDir = forceDir;
        }
    }
}
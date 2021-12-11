using Combat.Projectiles;
using Entities;
using UnityEngine;

namespace Combat
{
    public readonly struct HitInfo
    {
        public readonly BaseProjectile Projectile;
        public readonly float Damage;
        public readonly BaseCombatEntity Initiator;
        public readonly Collider HitCollider;
        public readonly Vector3 ForceDir;

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
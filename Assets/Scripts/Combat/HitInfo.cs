using Combat.Projectiles;
using Entities;

namespace Combat
{
    public struct HitInfo
    {
        public BaseProjectile Projectile;
        public float Damage;
        public BaseCombatEntity Initiator;

        public HitInfo(BaseProjectile projectile, float damage, BaseCombatEntity initiator)
        {
            Projectile = projectile;
            Damage = damage;
            Initiator = initiator;
        }
    }
}
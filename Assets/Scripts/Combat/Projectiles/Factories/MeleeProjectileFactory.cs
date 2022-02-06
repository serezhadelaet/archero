using Entities;
using UnityEngine;

namespace Combat.Projectiles.Factories
{
    public class MeleeProjectileFactory : BaseProjectileFactory
    {
        public override BaseProjectile GetProjectile(int level, BaseCharacter owner, float damage, LayerMask targetLayerMask)
        {
            var projectile = GetDefaultProjectile(owner, damage, targetLayerMask);
            return projectile;
        }
    }
}
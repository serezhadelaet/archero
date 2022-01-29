using Entities;
using UnityEngine;

namespace Combat.Projectiles.Factories
{
    public abstract class BaseProjectileFactory : MonoBehaviour
    {
        [SerializeField] private ProjectilePool projectilePool;
        
        public abstract BaseProjectile GetProjectile(int level, 
            BaseCharacter owner, float damage, LayerMask targetLayerMask);

        protected BaseProjectile GetDefaultProjectile(BaseCharacter owner, float damage, LayerMask targetLayerMask)
        {
            var projectile = projectilePool.Get();
            projectile.Init(owner, damage, targetLayerMask);
            return projectile;
        }
    }
}
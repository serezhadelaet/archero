using Entities;
using UnityEngine;

namespace Combat.Projectiles.Factories
{
    public abstract class BaseProjectileFactory : MonoBehaviour
    {
        
        public abstract BaseProjectile GetProjectile(BaseProjectile prefab, int level,
            BaseCharacter owner, float damage, LayerMask targetLayerMask);

        protected BaseProjectile GetDefaultProjectile(BaseProjectile prefab,
            BaseCharacter owner, float damage, LayerMask targetLayerMask)
        {
            var projectile = Instantiate(prefab);
            projectile.Init(owner, damage, targetLayerMask);
            return projectile;
        }
    }
}
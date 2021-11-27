using Combat.Projectiles;
using UnityEngine;

namespace Combat
{
    public class ArrowProjectileFactory : BaseProjectileFactory
    {
        [SerializeField] private StaticElectricityArrow staticElectricityArrow;

        public override BaseProjectile GetProjectile(BaseProjectile prefab, int level)
        {
            BaseProjectile projectile;
            switch (level)
            {
                case 2:
                    projectile = Instantiate(staticElectricityArrow);
                    return projectile;
                case 3:
                    projectile = Instantiate(staticElectricityArrow);
                    projectile.Mods.Add(new HealingProjectileModificator());
                    return projectile;
            }
            
            return Instantiate(prefab, transform.position, default);
        }
    }
}
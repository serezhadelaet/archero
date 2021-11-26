using Combat.Projectiles;
using UnityEngine;

namespace Combat
{
    public class ProjectileFactory : MonoBehaviour
    {
        [SerializeField] private StaticElectricityArrow staticElectricityArrow;

        public BaseProjectile GetProjectile(BaseProjectile prefab, int level)
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
                    break;
            }
            
            return Instantiate(prefab, transform.position, default);
        }
    }
}
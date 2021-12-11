using Combat.Projectiles.Modificators;
using UnityEngine;

namespace Combat.Projectiles.Factories
{
    public class StoneProjectileFactory : BaseProjectileFactory
    {
        [SerializeField] private StaticElectricityProjectile staticElectricityStonePrefab;
        
        public override BaseProjectile GetProjectile(BaseProjectile prefab, int level)
        {
            switch (level)
            {
                case 2:
                case 3:
                    var projectile = Instantiate(prefab, transform.position, default);
                    projectile.Mods.Add(new HealingProjectileModificator());
                    return projectile;
            }
            
            return Instantiate(prefab, transform.position, default);
        }
    }
}
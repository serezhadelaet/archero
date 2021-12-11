using Combat.Projectiles.Modificators;
using Entities;
using UnityEngine;

namespace Combat.Projectiles.Factories
{
    public class ArrowProjectileFactory : BaseProjectileFactory
    {
        [SerializeField] private StaticElectricityProjectileModificator staticElectricityProjectileModificator;
        [SerializeField] private HealingProjectileModificator healingProjectileModificator;
        
        public override BaseProjectile GetProjectile(BaseProjectile prefab, int level,
            BaseCharacter owner, float damage, LayerMask targetLayerMask)
        {
            var projectile = GetDefaultProjectile(prefab, owner, damage, targetLayerMask);;
            
            switch (level)
            {
                case 2:
                    AddStaticElectricityMod(projectile);
                    
                    return projectile;
                case 3:
                    AddStaticElectricityMod(projectile);
                    AddHealingMod(projectile);
                    
                    return projectile;
            }
            
            return projectile;
        }

        private void AddStaticElectricityMod(BaseProjectile projectile)
        {
            var mod = Instantiate(staticElectricityProjectileModificator, transform.position, default);
            mod.transform.SetParent(projectile.transform);
            mod.Init(projectile.Owner, projectile.Damage, projectile.TargetLayerMask);
            projectile.Mods.Add(mod);
        }

        private void AddHealingMod(BaseProjectile projectile)
        {
            projectile.Mods.Add(healingProjectileModificator);
        }
    }
}
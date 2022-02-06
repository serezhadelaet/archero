using Combat.Projectiles.Modificators;
using Entities;
using UnityEngine;

namespace Combat.Projectiles.Factories
{
    public class ArrowProjectileFactory : BaseProjectileFactory
    {
        [SerializeField] private StaticElectricityProjectilePool staticElectricityProjectilePool;
        [SerializeField] private HealingProjectileModificator healingProjectileModificator;
        
        public override BaseProjectile GetProjectile(int level, 
            BaseCharacter owner, float damage, LayerMask targetLayerMask)
        {
            var projectile = GetDefaultProjectile(owner, damage, targetLayerMask);
            
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
            var mod = staticElectricityProjectilePool.Get();
            mod.transform.SetParent(projectile.transform);
            mod.transform.localPosition = default;
            mod.transform.localRotation = default;
            mod.Init(projectile.Owner, projectile.Damage, projectile.TargetLayerMask);
            projectile.Mods.Add(mod as IProjectileModificator);
        }

        private void AddHealingMod(BaseProjectile projectile)
        {
            projectile.Mods.Add(healingProjectileModificator);
        }
    }
}
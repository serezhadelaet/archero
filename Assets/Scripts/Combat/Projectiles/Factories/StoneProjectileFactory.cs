using Combat.Projectiles.Modificators;
using Entities;
using UnityEngine;

namespace Combat.Projectiles.Factories
{
    public class StoneProjectileFactory : BaseProjectileFactory
    {
        [SerializeField] private HealingProjectileModificator healingProjectileModificator;
        
        public override BaseProjectile GetProjectile(BaseProjectile prefab, int level,
            BaseCharacter owner, float damage, LayerMask targetLayerMask)
        {
            var projectile = GetDefaultProjectile(prefab, owner, damage, targetLayerMask);;
            
            switch (level)
            {
                case 2:
                case 3:
                    AddHealingMod(projectile);
                    
                    return projectile;
            }
            
            return projectile;
        }

        private void AddHealingMod(BaseProjectile projectile)
        {
            projectile.Mods.Add(healingProjectileModificator);
        }
    }
}
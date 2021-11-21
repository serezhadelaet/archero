using System.Collections.Generic;
using Combat.Projectiles;
using UnityEngine;

namespace Combat
{
    public class Bow : BaseWeapon
    {
        [SerializeField] private Arrow arrowPrefab;
        [SerializeField] private StaticElectricityMissile staticElectricityMissilePrefab;
        
        public override void Attack()
        {
            var arrow = Instantiate(arrowPrefab, transform.position, default);
            arrow.Init(_owner, weaponSettings.damage, targetLayerMask, GetMods(arrow));
            arrow.SetDirection(_owner.transform.forward);
        }
        
        private List<IProjectileModificator> GetMods(BaseProjectile projectile)
        {
            switch (Level)
            {
                case 1:
                    return new List<IProjectileModificator> { GetStaticElectricityProjectileModificator(projectile) };
                case 2:
                    return new List<IProjectileModificator> { 
                        GetStaticElectricityProjectileModificator(projectile),
                        new HealingProjectileModificator() 
                    };
            }

            return null;
        }

        private StaticElectricityProjectileModificator GetStaticElectricityProjectileModificator(BaseProjectile projectile)
        {
            var staticElectricityMissile = Instantiate(staticElectricityMissilePrefab);
            staticElectricityMissile.gameObject.SetActive(false);
            staticElectricityMissile.Init(_owner, weaponSettings.damage, targetLayerMask, projectile.Mods);
            return new StaticElectricityProjectileModificator(staticElectricityMissile);
        }
    }
}
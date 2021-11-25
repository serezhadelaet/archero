using System.Collections.Generic;
using Combat.Projectiles;
using Entities;
using UnityEngine;

namespace Combat
{
    public class ProjectileModificatorsApplier : MonoBehaviour
    {
        [SerializeField] private StaticElectricityMissile staticElectricityMissilePrefab;

        public List<IProjectileModificator> GetMods(BaseCharacter owner, int level, BaseProjectile projectile,
            WeaponSettings weaponSettings, LayerMask targetLayerMask)
        {
            switch (level)
            {
                case 2:
                    return new List<IProjectileModificator>
                    {
                        GetStaticElectricityProjectileModificator(owner, projectile, weaponSettings, targetLayerMask)
                    };
                case 3:
                    return new List<IProjectileModificator>
                    {
                        GetStaticElectricityProjectileModificator(owner, projectile, weaponSettings, targetLayerMask),
                        new HealingProjectileModificator()
                    };
            }

            return null;
        }

        private StaticElectricityProjectileModificator GetStaticElectricityProjectileModificator(BaseCharacter owner,
            BaseProjectile projectile, WeaponSettings weaponSettings, LayerMask targetLayerMask)
        {
            var staticElectricityMissile = Instantiate(staticElectricityMissilePrefab);
            staticElectricityMissile.gameObject.SetActive(false);
            staticElectricityMissile.Init(owner, weaponSettings.damage, targetLayerMask, projectile.Mods);
            return new StaticElectricityProjectileModificator(staticElectricityMissile);
        }
    }
}
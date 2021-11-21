using System.Collections.Generic;
using Combat.Projectiles;
using NaughtyAttributes;
using UnityEngine;

namespace Combat
{
    public class Bow : BaseWeapon
    {
        [SerializeField] private Arrow arrowPrefab;
        [SerializeField] private StaticElectricityMissile staticElectricityMissilePrefab;
        
        [Button()]
        public override void Attack(Vector3 pos)
        {
            var arrow = Instantiate(arrowPrefab, transform.position, default);
            arrow.Init(_owner, weaponSettings.damage, targetLayerMask, GetMods(arrow));
            var dir = (new Vector3(pos.x, transform.position.y, pos.z) - transform.position).normalized;
            arrow.SetDirection(dir);
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
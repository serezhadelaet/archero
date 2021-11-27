using Combat.Projectiles;
using UnityEngine;

namespace Combat
{
    public class StonesThrower : BaseWeapon
    {
        [SerializeField] private StoneProjectile stoneProjectile;
        [SerializeField] private BaseProjectileFactory projectileFactory;
        
        public override void Attack(Vector3 pos)
        {
            var projectile = projectileFactory.GetProjectile(stoneProjectile, Level);
            projectile.transform.position = transform.position;
            projectile.Init(_owner, _weaponSettings.damage, _targetLayerMask);
            projectile.Shoot(pos);
        }
    }
}
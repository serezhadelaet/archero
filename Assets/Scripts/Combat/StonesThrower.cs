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
            projectile.Init(Owner, WeaponSettings.damage, TargetLayerMask);
            var dir = (new Vector3(pos.x, transform.position.y, pos.z) - transform.position).normalized;
            projectile.Shoot(dir, pos);
        }
    }
}
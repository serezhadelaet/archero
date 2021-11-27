using Combat.Projectiles;
using UnityEngine;

namespace Combat
{
    public class StonesThrower : BaseWeapon
    {
        [SerializeField] private StoneProjectile stoneProjectile;
        
        public override void Attack(Vector3 pos)
        {
            var projectile = Instantiate(stoneProjectile);
            projectile.transform.position = transform.position;
            projectile.Init(_owner, _weaponSettings.damage, _targetLayerMask);
            projectile.Shoot(pos);
        }
    }
}
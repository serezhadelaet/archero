using Combat.Projectiles;
using UnityEngine;

namespace Combat
{
    public class Bow : BaseWeapon
    {
        [SerializeField] private Arrow arrowPrefab;
        [SerializeField] private GameObject onAttackEffect;
        [SerializeField] private ProjectileFactory projectileFactory;
        
        public override void Attack(Vector3 pos)
        {
            var projectile = projectileFactory.GetProjectile(arrowPrefab, Level);
            projectile.transform.position = transform.position;
            projectile.Init(_owner, _weaponSettings.damage, _targetLayerMask);
            var dir = (new Vector3(pos.x, transform.position.y, pos.z) - transform.position).normalized;
            projectile.Shoot(dir);
            SpawnOnAttackEffect(dir);
        }
        
        private void SpawnOnAttackEffect(Vector3 targetDir)
        {
            Instantiate(onAttackEffect, transform.position, Quaternion.LookRotation(targetDir));
        }
    }
}
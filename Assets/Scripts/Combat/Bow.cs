using Combat.Projectiles;
using UnityEngine;

namespace Combat
{
    public class Bow : BaseWeapon
    {
        [SerializeField] private Arrow arrowPrefab;
        [SerializeField] private GameObject onAttackEffect;
        [SerializeField] private ArrowProjectileFactory projectileFactory;
        
        public override void Attack(Vector3 pos)
        {
            var projectile = projectileFactory.GetProjectile(arrowPrefab, Level);
            projectile.transform.position = transform.position;
            projectile.Init(Owner, WeaponSettings.damage, TargetLayerMask);
            var dir = (new Vector3(pos.x, transform.position.y, pos.z) - transform.position).normalized;
            projectile.Shoot(dir, pos);
            SpawnOnAttackEffect(dir);
        }
        
        private void SpawnOnAttackEffect(Vector3 dir)
        {
            Instantiate(onAttackEffect, transform.position, Quaternion.LookRotation(dir));
        }
    }
}
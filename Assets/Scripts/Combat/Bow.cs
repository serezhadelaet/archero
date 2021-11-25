using Combat.Projectiles;
using UnityEngine;

namespace Combat
{
    public class Bow : BaseWeapon
    {
        [SerializeField] private Arrow arrowPrefab;
        [SerializeField] private GameObject onAttackEffect;
        [SerializeField] private ProjectileModificatorsApplier projectileModificatorsApplier;
        
        public override void Attack(Vector3 pos)
        {
            var arrow = Instantiate(arrowPrefab, transform.position, default);
            var mods = projectileModificatorsApplier.GetMods(_owner, Level, arrow, weaponSettings, _targetLayerMask);
            arrow.Init(_owner, weaponSettings.damage, _targetLayerMask, mods);
            var dir = (new Vector3(pos.x, transform.position.y, pos.z) - transform.position).normalized;
            arrow.SetDirection(dir);
            SpawnOnAttackEffect(dir);
        }
        
        private void SpawnOnAttackEffect(Vector3 targetDir)
        {
            Instantiate(onAttackEffect, transform.position, Quaternion.LookRotation(targetDir));
        }
    }
}
using Combat.Projectiles;
using Combat.Projectiles.Factories;
using Entities;
using UnityEngine;

namespace Combat.Weapons
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        [SerializeField] private BaseProjectile defaultProjectile;
        [SerializeField] private BaseProjectileFactory projectileFactory;
        
        private WeaponSettings _weaponSettings;
        private BaseCharacter _owner;
        private int _level;
        private LayerMask _targetLayerMask;
        protected Vector3 Direction;

        public void SetLevel(int level)
        {
            _level = level;
        }

        public void Init(BaseCharacter owner, LayerMask targetLayer, WeaponSettings weaponSettings)
        {
            _owner = owner;
            _targetLayerMask = targetLayer;
            _weaponSettings = weaponSettings;
        }

        public virtual void Attack(Vector3 pos)
        {
            var projectile = projectileFactory.GetProjectile(defaultProjectile, _level);
            projectile.transform.position = transform.position;
            projectile.Init(_owner, _weaponSettings.damage, _targetLayerMask);
            Direction = (new Vector3(pos.x, transform.position.y, pos.z) - transform.position).normalized;
            projectile.Shoot(Direction, pos);
        }
    }
}
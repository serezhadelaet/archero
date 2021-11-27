using Entities;
using UnityEngine;

namespace Combat
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        protected WeaponSettings _weaponSettings;
        protected BaseCharacter _owner;
        protected int Level;
        protected LayerMask _targetLayerMask;

        public void SetLevel(int level)
        {
            Level = level;
        }

        public void Init(BaseCharacter owner, LayerMask targetLayer, WeaponSettings weaponSettings)
        {
            _owner = owner;
            _targetLayerMask = targetLayer;
            _weaponSettings = weaponSettings;
        }

        public abstract void Attack(Vector3 pos);
    }
}
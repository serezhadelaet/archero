using Entities;
using UnityEngine;

namespace Combat
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        protected WeaponSettings WeaponSettings;
        protected BaseCharacter Owner;
        protected int Level;
        protected LayerMask TargetLayerMask;

        public void SetLevel(int level)
        {
            Level = level;
        }

        public void Init(BaseCharacter owner, LayerMask targetLayer, WeaponSettings weaponSettings)
        {
            Owner = owner;
            TargetLayerMask = targetLayer;
            WeaponSettings = weaponSettings;
        }

        public abstract void Attack(Vector3 pos);
    }
}
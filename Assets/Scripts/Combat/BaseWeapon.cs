using Entities;
using UnityEngine;

namespace Combat
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        [SerializeField] protected WeaponSettings weaponSettings;
        
        protected BaseCharacter _owner;
        protected int Level;
        protected LayerMask _targetLayerMask;

        public void SetLevel(int level)
        {
            Level = level;
        }

        public void Init(BaseCharacter owner, LayerMask targetLayer)
        {
            _owner = owner;
            _targetLayerMask = targetLayer;
        }

        public abstract void Attack(Vector3 pos);
    }
}
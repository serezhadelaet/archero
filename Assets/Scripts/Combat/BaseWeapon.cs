using Entities;
using UnityEngine;

namespace Combat
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        [SerializeField] protected LayerMask targetLayerMask;
        [SerializeField] protected WeaponSettings weaponSettings;
        
        protected BaseCharacter _owner;
        protected int Level;
        
        public void SetLevel(int level)
        {
            Level = level;
        }

        public void Init(BaseCharacter owner, int level)
        {
            _owner = owner;
            Level = level;
        }

        public abstract void Attack();
    }
}
using Combat;
using UnityEngine;

namespace Entities
{
    public class BaseCharacter : BaseCombatEntity
    {
        [SerializeField] protected BaseWeapon weapon;

        protected override void Awake()
        {
            base.Awake();
            weapon.Init(this, 0);
        }
    }
}
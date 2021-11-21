using Combat;
using UnityEngine;
using UnityEngine.AI;

namespace Entities
{
    public class BaseCharacter : BaseCombatEntity
    {
        [SerializeField] protected NavMeshAgent navAgent;
        [SerializeField] protected BaseWeapon weapon;

        protected override void Awake()
        {
            base.Awake();
            weapon.Init(this, 0);
        }
    }
}
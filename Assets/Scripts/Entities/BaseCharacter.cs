using Combat;
using Helpers;
using UnityEngine;
using UnityEngine.AI;

namespace Entities
{
    public class BaseCharacter : BaseCombatEntity
    {
        [SerializeField] protected NavMeshAgent navAgent;
        [SerializeField] protected BaseWeapon weapon;
        [SerializeField] private CharacterAnimations characterAnimations;
        [SerializeField] private RagDoll ragDoll;
        
        protected override void Awake()
        {
            base.Awake();
            weapon.Init(this, 0);
        }

        protected override void OnDead()
        {
            base.OnDead();
            
        }
    }
}
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
        [SerializeField] protected CharacterAnimations animations;
        [SerializeField] protected RagDoll ragDoll;
        [SerializeField] protected LayerMask targetLayer;
        
        private HitInfo _lastHit;
        
        protected override void Awake()
        {
            base.Awake();
            weapon.Init(this, targetLayer);
        }

        public override void TakeDamage(HitInfo hitInfo)
        {
            base.TakeDamage(hitInfo);
            animations.OnHit();
            _lastHit = hitInfo;
        }

        protected override void OnDead()
        {
            base.OnDead();
            navAgent.enabled = false;
            ragDoll.SetAsRagDoll(true, _lastHit);
        }
    }
}
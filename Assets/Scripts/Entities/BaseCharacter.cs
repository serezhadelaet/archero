using Combat;
using Helpers;
using UI;
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
        [SerializeField] private float attackingRotationSpeed = 1440;
        [SerializeField] private Renderer[] renderers;
        [SerializeField] private HealthBar healthBar;
        
        private HitInfo _lastHit;
        protected Vector3 LastTargetPos;
        protected BaseCombatEntity CurrentTarget;
        protected int _level;
        
        private void Awake()
        {
            PickRandomSkin();
        }

        protected override void SetHealth()
        {
            base.SetHealth();
            SetHealthBar();
        }
        
        protected virtual void SetLevel()
        {
            _level = combatSettings.level;
        }

        protected virtual void SetWeapon()
        {
            weapon.Init(this, targetLayer, combatSettings.weaponSettings);
        }

        public override void TakeDamage(HitInfo hitInfo)
        {
            base.TakeDamage(hitInfo);
            
            if (IsDead())
                return;
            
            animations.OnHit();
            _lastHit = hitInfo;
        }

        protected override void OnDead()
        {
            base.OnDead();
            navAgent.enabled = false;
            ragDoll.SetAsRagDoll(true, _lastHit);
        }

        protected virtual void Update()
        {
            if (IsDead())
                return;
            
            FollowTarget();
        }

        private void FollowTarget()
        {
            if (!ShouldFollowTarget())
                return;
            
            LastTargetPos = CurrentTarget.transform.position;
            var targetDir = (LastTargetPos - transform.position).normalized;
            var targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot,
                Time.deltaTime * attackingRotationSpeed);
        }
        
        private void PickRandomSkin()
        {
            var randomSkin = renderers[Random.Range(0, renderers.Length)];
            foreach (var r in renderers)
            {
                r.gameObject.SetActive(r == randomSkin);
            }
        }
        
        private void SetHealthBar()
        {
            if (!healthBar)
                return;
            healthBar.SetMaxHealth(Health);
            OnHealthChanged += healthBar.SetHealth;
        }

        protected virtual bool ShouldFollowTarget() => false;
    }
}
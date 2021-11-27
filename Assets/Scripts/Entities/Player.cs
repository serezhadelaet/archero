using Helpers;
using UI;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class Player : BaseCharacter
    {
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private float attackRange = 10;
        [SerializeField] private ParticleSystem healingEffect;
        [SerializeField] private PlayerProgressionFollower playerProgression;
        [SerializeField] private PlayerMovement movement;
        
        private const float AttackCooldown = 0.1f;
        
        private Collider[] _collBuff = new Collider[30];
        private float _lastAttackTime;
        private WinLoseOverlay _winLoseOverlay;

        [Inject]
        private void Construct(Joystick joystick, GameOverlay gameOverlay, WinLoseOverlay winLoseOverlay)
        {
            _winLoseOverlay = winLoseOverlay;
            animations.OnAttacked += Attack;
            movement.Init(joystick, navAgent, animations);
            SetupOverlay(gameOverlay);
            SetHealth();
            SetWeapon();
            SetLevel();
        }
 
        private void SetupOverlay(GameOverlay gameOverlay)
        {
            gameOverlay.UpdateHealth((int)Health);
            gameOverlay.UpdateExp(playerProgression.GetExp());
            gameOverlay.UpdateLevel(playerProgression.GetLevel());
            
            OnHealthChanged += (health) => gameOverlay.UpdateHealth((int) health);
            playerProgression.OnProgress += () =>
            {
                gameOverlay.UpdateLevel(playerProgression.GetLevel());
                gameOverlay.UpdateExp(playerProgression.GetExp());
            };
        }

        public override void Heal(float hp)
        {
            base.Heal(hp);
            healingEffect.Play();
        }

        protected override void OnDead()
        {
            base.OnDead();
            _winLoseOverlay.Show();
        }

        protected override void Update()
        {
            base.Update();
            if (IsDead())
                return;
            
            InstantiateAttack();
            TryToCancelAttack();
        }
        
        private void InstantiateAttack()
        {
            var nearestEnemy = GetNearestEnemy();
            if (nearestEnemy != null)
                CurrentTarget = nearestEnemy;
            if (!CanAttack())
                return;
            
            _lastAttackTime = Time.time;
            if (CurrentTarget != null)
            {
                LastTargetPos = CurrentTarget.transform.position;
            }
            
            animations.Attack(CurrentTarget != null);
        }

        private void TryToCancelAttack()
        {
            if (IsMoving() || !CanSee(LastTargetPos) || (CurrentTarget != null && CurrentTarget.IsDead()))
                animations.Attack(false);
        }
        
        private void Attack()
        {
            if (IsMoving())
                return;
            
            transform.LookAt(LastTargetPos);
            weapon.SetLevel(playerProgression.GetLevel());
            weapon.Attack(LastTargetPos);
        }

        private BaseCombatEntity GetNearestEnemy()
        {
            var count = Physics.OverlapSphereNonAlloc(transform.position, attackRange, _collBuff, targetLayer);
            var minDistance = float.MaxValue;
            BaseCombatEntity nearestEntity = null;
            for (int i = 0; i < count; i++)
            {
                var coll = _collBuff[i];
                var combatEntity = coll.GetComponentInParent<BaseCombatEntity>();
                if (combatEntity.IsDead())
                    continue;

                if (!CanSee(coll.transform.position))
                    continue;

                var distance = Vector3.Distance(transform.position, coll.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEntity = combatEntity;
                }
            }

            return nearestEntity;
        }

        protected override bool ShouldFollowTarget() => !IsMoving() && animations.IsAttacking() && CurrentTarget;
        private bool IsMoving() => Input.GetMouseButton(0);

        private bool CanAttack() => !animations.IsAttacking() && !IsMoving() &&
                                    Time.time > _lastAttackTime + AttackCooldown;

        private bool CanSee(Vector3 pos) => !Physics.Linecast(transform.position, pos, obstacleLayer);
    }
}
using Helpers;
using UI;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class Player : BaseCharacter
    {
        [SerializeField] private float attackingRotationSpeed = 1440;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private float attackRange = 10;
        [SerializeField] private ParticleSystem healingEffect;
        [SerializeField] private PlayerProgressionFollower playerProgression;
        [SerializeField] private PlayerMovement movement;
        
        private const float AttackCooldown = 0.1f;
        
        private Collider[] _collBuff = new Collider[30];
        private BaseCombatEntity _currentTarget;
        private Vector3 _lastTargetPos;
        private float _lastAttackTime;

        [Inject]
        private void Construct(Joystick joystick, GameOverlay gameOverlay)
        {
            animations.OnAttacked += Attack;

            movement.Init(joystick, navAgent, animations);
            SetupOverlay(gameOverlay);
        }

        private void SetupOverlay(GameOverlay gameOverlay)
        {
            gameOverlay.UpdateHealth((int)Health);
            gameOverlay.UpdateLevel(playerProgression.GetLevel());
            OnHealthChanged += (health) => gameOverlay.UpdateHealth((int) health);
            playerProgression.OnProgress += () => gameOverlay.UpdateLevel(playerProgression.GetLevel());
        }

        public override void Heal(float hp)
        {
            base.Heal(hp);
            healingEffect.Play();
        }

        private void Update()
        {
            InstantiateAttack();
            TryToCancelAttack();
            FollowTarget();
        }
        
        private void InstantiateAttack()
        {
            var nearestEnemy = GetNearestEnemy();
            if (nearestEnemy != null)
                _currentTarget = nearestEnemy;
            if (!CanAttack())
                return;
            
            _lastAttackTime = Time.time;
            if (_currentTarget != null)
            {
                _lastTargetPos = _currentTarget.transform.position;
            }
            
            animations.Attack(_currentTarget != null);
        }

        private void FollowTarget()
        {
            if (!ShouldFollowTarget())
                return;
            
            _lastTargetPos = _currentTarget.transform.position;
            var targetDir = (_lastTargetPos - transform.position).normalized;
            var targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot,
                Time.deltaTime * attackingRotationSpeed);
        }

        private void TryToCancelAttack()
        {
            if (IsMoving() || !CanSee(_lastTargetPos) || (_currentTarget != null && _currentTarget.IsDead()))
                animations.Attack(false);
        }
        
        private void Attack()
        {
            if (IsMoving())
                return;
            
            transform.LookAt(_lastTargetPos);
            weapon.SetLevel(playerProgression.GetLevel());
            weapon.Attack(_lastTargetPos);
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

        private bool ShouldFollowTarget() => !IsMoving() && animations.IsAttacking() && _currentTarget;
        private bool IsMoving() => Input.GetMouseButton(0);

        private bool CanAttack() => !animations.IsAttacking() && !IsMoving() &&
                                    Time.time > _lastAttackTime + AttackCooldown;

        private bool CanSee(Vector3 pos) => !Physics.Linecast(transform.position, pos, obstacleLayer);
    }
}
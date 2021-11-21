using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Helpers
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimations : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        private static readonly int RunSpeed = Animator.StringToHash("RunSpeed");
        private static readonly int OnHit1 = Animator.StringToHash("OnHit1");
        private static readonly int OnHit2 = Animator.StringToHash("OnHit2");

        public event Action OnAttacked;
        
        private void OnValidate()
        {
            animator = GetComponent<Animator>();
        }
        
        public void Attack()
        {
            animator.SetTrigger(AttackTrigger);
        }

        public void SetRunSpeed(float speed)
        {
            animator.SetFloat(RunSpeed, speed);
        }

        public void OnHit()
        {
            var randomTrigger = Random.Range(0, 2) == 0 ? OnHit1 : OnHit2;
            animator.SetTrigger(randomTrigger);
        }

        public void OnAttackTriggered()
        {
            OnAttacked?.Invoke();
        }

        public bool IsAttacking() => animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }
}
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Helpers
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimations : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float attackSpeed = 1;
        [SerializeField] private float moveSpeedMod = 1;
        
        private static readonly int AttackBool = Animator.StringToHash("IsAttacking");
        private static readonly int AttackMeleeBool = Animator.StringToHash("IsAttackingMelee");
        private static readonly int RunSpeed = Animator.StringToHash("RunSpeed");
        private static readonly int OnHit1 = Animator.StringToHash("OnHit1");
        private static readonly int OnHit2 = Animator.StringToHash("OnHit2");
        private static readonly int AttackSpeed = Animator.StringToHash("AttackSpeed");
        private static readonly int MoveSpeedMod = Animator.StringToHash("MoveSpeedMod");
        
        public event Action OnAttacked;
        public event Action OnAttackedMelee;
        
        private void OnEnable()
        {
            animator.SetFloat(AttackSpeed, attackSpeed);
            animator.SetFloat(MoveSpeedMod, moveSpeedMod);
        }

        private void OnValidate()
        {
            animator = GetComponent<Animator>();
        }

        public void AttackRange(bool f)
        {
            animator.SetBool(AttackBool, f);
        }
        
        public void AttackMelee(bool f)
        {
            animator.SetBool(AttackMeleeBool, f);
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
        
        public void OnAttackMeleeTriggered()
        {
            OnAttackedMelee?.Invoke();
        }

        public bool IsAttacking() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
            || animator.GetBool(AttackBool);
        
        public bool IsAttackingMelee() => 
            animator.GetCurrentAnimatorStateInfo(0).IsName("AttackMelee")
            || animator.GetBool(AttackMeleeBool);
    }
}
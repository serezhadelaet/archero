using UnityEngine;

namespace Combat.Weapons
{
    public class Bow : BaseWeapon
    {
        [SerializeField] private GameObject onAttackEffect;
        
        public override void Attack(Vector3 pos)
        {
            base.Attack(pos);
            SpawnOnAttackEffect();
        }
        
        private void SpawnOnAttackEffect()
        {
            Instantiate(onAttackEffect, transform.position, Quaternion.LookRotation(Direction));
        }
    }
}
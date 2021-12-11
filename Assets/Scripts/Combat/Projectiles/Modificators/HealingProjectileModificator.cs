using Entities;
using UnityEngine;

namespace Combat.Projectiles.Modificators
{
    [CreateAssetMenu]
    public class HealingProjectileModificator : ScriptableObject, IProjectileModificator
    {
        [SerializeField] private float healingMod = 0.23f;

        public void ApplyMod(BaseProjectile projectile, BaseCombatEntity combatEntity, float damage)
        {
            projectile.Owner.Heal(healingMod * damage);
        }
    }
}
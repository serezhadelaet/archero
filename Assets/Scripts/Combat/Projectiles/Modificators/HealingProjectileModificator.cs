using Entities;

namespace Combat.Projectiles.Modificators
{
    public class HealingProjectileModificator : IProjectileModificator
    {
        private const float HealingMod = 0.23f;

        public void ApplyMod(BaseProjectile projectile, BaseCombatEntity combatEntity, float damage)
        {
            projectile.Owner.Heal(HealingMod * damage);
        }
    }
}
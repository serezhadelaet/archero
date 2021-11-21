using Entities;

namespace Combat.Projectiles
{
    public interface IProjectileModificator
    {
        void ApplyMod(BaseProjectile projectile, BaseCombatEntity combatEntity, float damage);
    }
}
using Entities;

namespace Combat.Projectiles.Modificators
{
    public interface IProjectileModificator
    {
        void ApplyMod(BaseProjectile projectile, BaseCombatEntity combatEntity, float damage);
    }
}
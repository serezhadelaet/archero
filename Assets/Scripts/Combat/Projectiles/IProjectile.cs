using Entities;

namespace Combat.Projectiles
{
    public interface IProjectile
    {
        void OnHit(BaseCombatEntity combatEntity, float damage);
    }
}
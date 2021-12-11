using UnityEngine;

namespace Combat.Projectiles.Factories
{
    public abstract class BaseProjectileFactory : MonoBehaviour
    {
        public abstract BaseProjectile GetProjectile(BaseProjectile prefab, int level);
    }
}
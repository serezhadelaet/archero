using Combat.Projectiles;
using UnityEngine;

namespace Combat
{
    public abstract class BaseProjectileFactory : MonoBehaviour
    {
        public abstract BaseProjectile GetProjectile(BaseProjectile prefab, int level);
    }
}
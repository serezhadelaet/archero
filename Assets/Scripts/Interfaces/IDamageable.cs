using Combat;
using UnityEngine;

namespace Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(HitInfo hitInfo);
        bool IsDead { get; }
        Transform transform { get; }
    }
}
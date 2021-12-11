using Entities;
using Interfaces;
using UnityEngine;

namespace Extensions
{
    public static class ComponentExtensions
    {
        public static IDamageable GetDamageable(this Component component)
        {
            var damageable = component.GetComponent<IDamageable>();
            if (damageable != null)
                return damageable;
            damageable = component.GetComponentInChildren<IDamageable>();
            if (damageable != null)
                return damageable;
            damageable = component.GetComponentInParent<IDamageable>();
            return damageable;
        }
    }
}
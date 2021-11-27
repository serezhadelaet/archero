using Combat;
using UnityEngine;

namespace Entities
{
    [CreateAssetMenu]
    public class CombatEntitySettings : ScriptableObject
    {
        public WeaponSettings weaponSettings;
        public int health;
    }
}
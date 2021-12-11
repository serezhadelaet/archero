using Combat;
using Combat.Weapons;
using UnityEngine;

namespace Entities
{
    [CreateAssetMenu]
    public class CombatEntitySettings : ScriptableObject
    {
        public WeaponSettings weaponSettings;
        public int level;
        public int health;
    }
}
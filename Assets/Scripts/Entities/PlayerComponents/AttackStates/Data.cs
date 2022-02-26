using Helpers;
using UnityEngine;

namespace Entities.PlayerComponents.AttackStates
{
    public class Data
    {
        public GameObject WeaponModel;
        public GameObject WeaponModelDisarmed;
        public Player Player;
        public CombatTargetingSo CombatTargeting;
        public float RadiusRange;
        public float RadiusMelee;
        public CharacterAnimations Animations;
        public PlayerDash Dash;
    }
}
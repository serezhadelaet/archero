using UnityEngine;
using Zenject;

namespace Entities
{
    public class Player : BaseCharacter
    {
        private int _currentLevel;
        private Joystick _joystick;

        [Inject]
        private void Construct(Joystick joystick)
        {
            _joystick = joystick;
        }
        
        private void OnLevelUp()
        {
            _currentLevel++;
            weapon.SetLevel(_currentLevel);
        }

        private void Update()
        {
            Moving();
        }

        private void InstantiateAttack()
        {
            
        }
        
        private void Moving()
        {
            var offset = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
            navAgent.SetDestination(transform.position + offset);
        }
    }
}
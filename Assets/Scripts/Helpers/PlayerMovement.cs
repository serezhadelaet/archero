using UnityEngine;
using UnityEngine.AI;

namespace Helpers
{
    public class PlayerMovement : MonoBehaviour
    {
        private Joystick _joystick;
        private NavMeshAgent _navAgent;
        private CharacterAnimations _animations;
        
        public void Init(Joystick joystick, NavMeshAgent navAgent, CharacterAnimations animations)
        {
            _joystick = joystick;
            _navAgent = navAgent;
            _animations = animations;
        }
        
        private void Moving()
        {
            if (!_navAgent.enabled)
                return;
            
            var offset = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
            _navAgent.SetDestination(transform.position + offset);
            _animations.SetRunSpeed(offset.magnitude);
        }
        
        private void Update()
        {
            Moving();
        }
    }
}
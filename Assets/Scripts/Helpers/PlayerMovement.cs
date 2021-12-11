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
            
            var normalizedDirection = _joystick.Direction.normalized;
            var offset = new Vector3(normalizedDirection.x, 0, normalizedDirection.y);
            _navAgent.SetDestination(transform.position + offset);
            _animations.SetRunSpeed(_navAgent.velocity.magnitude);
        }
        
        private void Update()
        {
            Moving();
        }
    }
}
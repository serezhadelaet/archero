using Entitas.Unity;
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
            
            Contexts.sharedInstance.input.CreateEntity().AddJoystick(_joystick);
            Contexts.sharedInstance.game.CreateEntity().AddPlayerNavmeshAgent(_navAgent);
        }
        
        private void Moving()
        {
            if (!_navAgent.enabled)
                return;
            _animations.SetRunSpeed(_navAgent.velocity.magnitude);
        }
        
        private void Update()
        {
            Moving();
        }
    }
}
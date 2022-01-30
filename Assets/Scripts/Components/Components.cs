using Entitas;
using UnityEngine;
using UnityEngine.AI;

[Game] public struct PlayerNavmeshAgentComponent : IComponent { public NavMeshAgent Value; }
[Input] public struct InputUpdateComponent : IComponent { public Vector2 Input; }
[Input] public struct JoystickComponent : IComponent { public Joystick Joystick; }
using Entitas;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Entitas.Systems _systems;

    private void Start()
    {
        // get a reference to the contexts
        var contexts = Contexts.sharedInstance;
        
        // create the systems by creating individual features
        _systems = new Feature("Systems")
            .Add(new InputUpdateSystem(contexts))
            .Add(new PlayerNavMeshUpdateSystem(contexts));
        
        // call Initialize() on all of the IInitializeSystems
        _systems.Initialize();
    }

    private void Update()
    {
        // call Execute() on all the IExecuteSystems and 
        // ReactiveSystems that were triggered last frame
        _systems.Execute();
        // call cleanup() on all the ICleanupSystems
        _systems.Cleanup();
    }
}
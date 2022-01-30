using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class PlayerNavMeshUpdateSystem : ReactiveSystem<InputEntity>
{
    private readonly IGroup<GameEntity> _groupNavmesh;
        
    public PlayerNavMeshUpdateSystem (Contexts contexts) : base(contexts.input)
    {
        _groupNavmesh = contexts.game.GetGroup(GameMatcher.PlayerNavmeshAgent);
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    {
        return context.CreateCollector(InputMatcher.InputUpdate);
    }

    protected override bool Filter(InputEntity entity)
    {
        return entity.hasInputUpdate;
    }

    protected override void Execute(List<InputEntity> entities)
    {
        foreach (var navmesh in _groupNavmesh.GetEntities())
        foreach (var input in entities)
        {
            var nav = navmesh.playerNavmeshAgent.Value;
                    
            var normalizedDirection = input.inputUpdate.Input;
#if UNITY_EDITOR
            normalizedDirection += new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
#endif
            var offset = new Vector3(normalizedDirection.x, 0, normalizedDirection.y);

            nav.SetDestination(nav.transform.position + offset);
        }
    }
}
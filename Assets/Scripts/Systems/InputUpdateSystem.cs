using System.Collections.Generic;
using Entitas;

public class InputUpdateSystem : IExecuteSystem, ICleanupSystem
{
    private readonly InputContext _context;
    private readonly IGroup<InputEntity> _inputUpdateGroup;
    
    public InputUpdateSystem(Contexts contexts)
    {
        _context = contexts.input;
        _inputUpdateGroup = contexts.input.GetGroup(InputMatcher.InputUpdate);
    }
    
    public void Execute()
    {
        foreach (var entity in _context.GetEntities(InputMatcher.Joystick))
        {
            Contexts.sharedInstance.input.CreateEntity().AddInputUpdate(entity.joystick.Joystick.Direction.normalized);
        }
    }
    
    public void Cleanup()
    {
        foreach (var e in _inputUpdateGroup.GetEntities())
            e.Destroy();
    }
}
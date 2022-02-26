namespace Entities.PlayerComponents.AttackStates
{
    public interface IState
    {
        void Init(AttackStateMachine stateMachine);
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}
namespace Entities.PlayerComponents.AttackStates
{
    public class AttackStateMachine
    {
        public IState RangeAttackState;
        public IState MeleeAttackState;
        
        private IState _currentState;

        public AttackStateMachine(IState rangeAttackState, IState meleeAttackState)
        {
            RangeAttackState = rangeAttackState;
            MeleeAttackState = meleeAttackState;
            
            RangeAttackState.Init(this);
            MeleeAttackState.Init(this);
        }
        
        public void SetState(IState state)
        {
            _currentState?.OnExit();
            _currentState = state;
            _currentState.OnEnter();
        }

        public void Update()
        {
            _currentState?.OnUpdate();
        }

        public void Exit()
        {
            _currentState?.OnExit();
        }
    }
}
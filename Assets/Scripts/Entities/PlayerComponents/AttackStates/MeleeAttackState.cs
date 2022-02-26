namespace Entities.PlayerComponents.AttackStates
{
    public class MeleeAttackState : BaseAttackState
    {
        public MeleeAttackState(Data data) : base(data) { }
        
        public override void OnUpdate()
        {
            if (_data.Player.IsDead)
            {
                _stateMachine.Exit();
                return;
            }
                
            var target = GetNearestTarget();
            if (target)
            {
                _data.Player.LastTargetPos = target.transform.position;
                _data.Player.CurrentTarget = target;
            }
            
            _data.Animations.AttackMelee(target && !IsMoving());
            
            if (!target)
                _stateMachine.SetState(_stateMachine.RangeAttackState);
        }
        
        private BaseCombatEntity GetNearestTarget()
        {
            return _data.CombatTargeting.GetTarget(_data.Player.transform.position, 
                _data.RadiusMelee, _data.RadiusMelee, out _);
        }
        
        public override void OnExit()
        {
            base.OnExit();
            _data.Animations.AttackMelee(false);
        }
    }
}
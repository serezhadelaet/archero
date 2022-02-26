namespace Entities.PlayerComponents.AttackStates
{
    public class RangeAttackState : BaseAttackState
    {
        public RangeAttackState(Data data) : base(data) { }
        
        public override void OnUpdate()
        {
            if (_data.Player.IsDead)
            {
                _stateMachine.Exit();
                return;
            }

            var (target, meleeFound) = GetNearestTarget();
            if (meleeFound)
            {
                _stateMachine.SetState(_stateMachine.MeleeAttackState);
                return;
            }

            if (target)
            {
                _data.Player.LastTargetPos = target.transform.position;
                _data.Player.CurrentTarget = target;
            }
            _data.Animations.AttackRange(target && !IsMoving());
        }

        private (BaseCombatEntity target, bool meleeFound) GetNearestTarget()
        {
            return (_data.CombatTargeting.GetTarget(_data.Player.transform.position, _data.RadiusRange,
                _data.RadiusMelee, out var meleeFound), meleeFound);
        }
        
        public override void OnExit()
        {
            base.OnExit();
            _data.Animations.AttackRange(false);
        }
    }
}
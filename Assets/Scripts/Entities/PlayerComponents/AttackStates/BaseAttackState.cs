using UnityEngine;

namespace Entities.PlayerComponents.AttackStates
{
    public abstract class BaseAttackState : IState
    {
        protected Data _data;
        protected AttackStateMachine _stateMachine;

        protected BaseAttackState(Data data)
        {
            _data = data;
        }
        
        public virtual void Init(AttackStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual void OnEnter()
        {
            _data.WeaponModel.gameObject.SetActive(true);
            _data.WeaponModelDisarmed.gameObject.SetActive(false);
        }

        public abstract void OnUpdate();

        public virtual void OnExit()
        {
            _data.WeaponModel.gameObject.SetActive(false);
            _data.WeaponModelDisarmed.gameObject.SetActive(true);
        }

        protected bool IsMoving() => Input.GetMouseButton(0) || _data.Dash.IsDashing();
    }
}
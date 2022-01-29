using System;
using Entities;
using Interfaces;
using UnityEngine;

namespace Combat.Projectiles.MovingDamagers
{
    public class MovingDamager : MonoBehaviour
    {
        public bool IsStopped { get; set; }

        private Vector3 _lastPos;
        private LayerMask _targetLayerMask;
        private Action<IDamageable, Collider> _onHit;
        protected Vector3 TargetPos;
        protected Vector3 Direction;
        
        private IDamageable _owner;

        public virtual void Init(Vector3 direction, Vector3 targetPos, LayerMask layerMask, Action<IDamageable, Collider> onHit,
            IDamageable owner)
        {
            IsStopped = false;
            Direction = direction;
            TargetPos = targetPos;
            _targetLayerMask = layerMask;
            _onHit = onHit;
            _owner = owner;
            _lastPos = default;
        }

        private void DoHit(Collider coll)
        {
            var damageable = coll.gameObject.GetComponentInParent<IDamageable>();
            if (damageable != _owner)
                _onHit?.Invoke(damageable, coll);
        }

        private void LateUpdate()
        {
            if (!IsStopped && _lastPos != default
                           && Physics.Linecast(_lastPos, transform.position, out var hit, _targetLayerMask))
            {
                if (hit.collider)
                {
                    DoHit(hit.collider);
                }
            }
        }

        protected virtual void Update()
        {
            if (!IsStopped)
                _lastPos = transform.position;
        }
    }
}
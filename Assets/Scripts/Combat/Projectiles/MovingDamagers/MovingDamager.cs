using System;
using Interfaces;
using UnityEngine;

namespace Combat.Projectiles.MovingDamagers
{
    public class MovingDamager : MonoBehaviour
    {
        protected Vector3 _lastPos;
        protected LayerMask _targetLayerMask;
        protected Action<IDamageable, Collider> _onHit;
        protected Vector3 _targetPos;
        public bool IsStopped { get; set; }
        
        public virtual void Init(Vector3 targetPos, LayerMask layerMask, Action<IDamageable, Collider> onHit)
        {
            _targetPos = targetPos;
            _targetLayerMask = layerMask;
            _onHit = onHit;
        }
        
        private void DoHit(Collider coll)
        {
            var damageable = coll.gameObject.GetComponentInParent<IDamageable>();
            _onHit?.Invoke(damageable, coll);
        }
        
        private void LateUpdate()
        {
            if (!IsStopped && _lastPos != default &&
                Physics.Linecast(_lastPos, transform.position, out var hit, _targetLayerMask))
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
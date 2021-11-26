using System;
using Interfaces;
using UnityEngine;

namespace Combat.Projectiles
{
    public class StraightDirectionMoving : MonoBehaviour
    {
        [SerializeField] private float speed = 40;

        private Vector3 _dir;
        private Vector3 _lastPos;
        private LayerMask _targetLayerMask;
        private Action<IDamageable, Collider> _onHit;
        
        public void Init(Vector3 direction, LayerMask layerMask, Action<IDamageable, Collider> onHit)
        {
            _dir = direction;
            transform.forward = direction;
            _targetLayerMask = layerMask;
            _onHit = onHit;
        }

        private void DoHit(Collider coll)
        {
            var damageable = coll.gameObject.GetComponentInParent<IDamageable>();
            if (damageable != null)
                _onHit?.Invoke(damageable, coll);
        }
        
        private void Update()
        {
            _lastPos = transform.position;
            transform.position += _dir * (Time.deltaTime * speed);
        }

        private void LateUpdate()
        {
            if (_lastPos != default && Physics.Linecast(_lastPos, transform.position, out var hit, _targetLayerMask))
            {
                if (hit.collider)
                {
                    DoHit(hit.collider);
                }
            }
        }
    }
}
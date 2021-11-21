using Entities;
using Interfaces;
using UnityEngine;

namespace Combat.Projectiles
{
    public class Arrow : BaseProjectile
    {
        [SerializeField] private float speed = 40;
        
        private Vector3 _dir;
        private Vector3 _lastPos;
        
        public void SetDirection(Vector3 direction)
        {
            _dir = direction;
            transform.forward = direction;
        }

        private void DoHit(Collider coll)
        {
            var damageable = coll.gameObject.GetComponentInParent<IDamageable>();
            damageable?.TakeDamage(new HitInfo(this, Damage, Owner, coll, _dir));
            Destroy(gameObject);
        }
        
        private void Update()
        {
            _lastPos = transform.position;
            transform.position += _dir * (Time.deltaTime * speed);
        }

        private void LateUpdate()
        {
            if (_lastPos != default && Physics.Linecast(_lastPos, transform.position, out var hit, TargetLayerMask))
            {
                if (hit.collider)
                {
                    DoHit(hit.collider);
                }
            }
        }
    }
}
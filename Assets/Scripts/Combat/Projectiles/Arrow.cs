using Interfaces;
using UnityEngine;

namespace Combat.Projectiles
{
    public class Arrow : BaseProjectile
    {
        private Vector3 _dir;
        public void SetDirection(Vector3 direction)
        {
            _dir = direction;
            transform.forward = direction;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (TargetLayerMask == (TargetLayerMask | (1 << other.gameObject.layer)))
                DoHit(other);
        }

        private void DoHit(Collider other)
        {
            var damageable = other.gameObject.GetComponentInParent<IDamageable>();
            damageable?.TakeDamage(new HitInfo(this, Damage, Owner));
        }

        private void FixedUpdate()
        {
            transform.position += _dir;
        }
    }
}
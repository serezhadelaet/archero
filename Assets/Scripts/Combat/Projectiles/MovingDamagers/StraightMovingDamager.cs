using System;
using Interfaces;
using UnityEngine;

namespace Combat.Projectiles.MovingDamagers
{
    public class StraightMovingDamager : MovingDamager
    {
        [SerializeField] private float speed = 40;
        
        public override void Init(Vector3 direction, LayerMask layerMask, Action<IDamageable, Collider> onHit)
        {
            base.Init(direction, layerMask, onHit);
            transform.forward = direction;
        }

        protected override void Update()
        {
            base.Update();
            
            if (!IsStopped)
                transform.position += _targetPos * (Time.deltaTime * speed);
        }
    }
}
using System;
using Interfaces;
using UnityEngine;

namespace Combat.Projectiles.MovingDamagers
{
    public class StraightMovingDamager : MovingDamager
    {
        [SerializeField] private float speed = 40;
        
        public override void Init(Vector3 direction, Vector3 targetPos, LayerMask layerMask, Action<IDamageable, Collider> onHit,
            IDamageable owner)
        {
            base.Init(direction, targetPos, layerMask, onHit, owner);
            transform.forward = direction;
        }

        protected override void Update()
        {
            base.Update();
            
            if (!IsStopped)
                transform.position += Direction * (Time.deltaTime * speed);
        }
    }
}
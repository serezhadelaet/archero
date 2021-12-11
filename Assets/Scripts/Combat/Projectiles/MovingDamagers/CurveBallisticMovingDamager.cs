using System;
using Extensions;
using Interfaces;
using UnityEngine;

namespace Combat.Projectiles.MovingDamagers
{
    public class CurveBallisticMovingDamager : MovingDamager
    {
        [SerializeField] private AnimationCurve yCurve;
        [SerializeField] private float flySpeed = 1;
        [SerializeField] private float targetYOffset = 0.5f;

        private float _lerpTime;
        private Vector3 _startPos;
        
        public override void Init(Vector3 direction, Vector3 targetPos, LayerMask layerMask, Action<IDamageable, Collider> onHit,
            IDamageable owner)
        {
            base.Init(direction, targetPos, layerMask, onHit, owner);
            
            _startPos = transform.position;
            _lerpTime = 0;
            SetAnimationKeys();
        }

        private void SetAnimationKeys()
        {
            var keys = yCurve.keys;
            keys[0].value = _startPos.y;
            keys[yCurve.length - 2].value = TargetPos.y + targetYOffset;
            yCurve.keys = keys;
        }
        
        protected override void Update()
        {
            base.Update();
            if (IsStopped)
                return;

            _lerpTime += Time.deltaTime * flySpeed;
            var newPos = Vector3.Lerp(_startPos, TargetPos, _lerpTime);
            newPos.y = yCurve.Evaluate(_lerpTime);
            transform.position = newPos;
            var dir = (newPos - TargetPos).normalized;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
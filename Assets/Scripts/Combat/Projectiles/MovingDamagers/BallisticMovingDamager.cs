using System;
using Interfaces;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Combat.Projectiles.MovingDamagers
{
    public class BallisticMovingDamager : MovingDamager
    {
        [SerializeField] private float flySpeed = 5;
        [SerializeField] private Vector2 randomizeRange = Vector2.one;
        [SerializeField] private bool randomizeLaunchSpeed = true;
        [SerializeField] private float yOffset = .5f;
        [SerializeField] private float ballisticStrength = .5f;
        [SerializeField] private float rangeOffset = 1;
        
        private float _launchSpeed;
        private Vector3 _launchPoint;
        private Vector3 _launchVelocity;
        private float _lerpTime;
        private float _gravityY;

        public override void Init(Vector3 targetPos, LayerMask layerMask, Action<IDamageable, Collider> onHit)
        {
            base.Init(targetPos, layerMask, onHit);
            
            _launchPoint = transform.position;

            var distance = Vector3.Distance(targetPos, _launchPoint);
            
            var x0 = distance + rangeOffset;
            var y0 = -ballisticStrength;
            _launchSpeed = Mathf.Sqrt(Mathf.Abs(Physics.gravity.y) * (y0 + Mathf.Sqrt(x0 * x0 + y0 * y0)));
            
            // Some ballistic math
            Vector2 dir;
            dir.x = _targetPos.x - _launchPoint.x;
            dir.y = _targetPos.z - _launchPoint.z;
            var dirMagnitude = dir.magnitude;
            var y = -_launchPoint.y;
            dir /= dirMagnitude;
            _gravityY = Mathf.Abs(Physics.gravity.y);
            var s = _launchSpeed + (randomizeLaunchSpeed ? Random.Range(randomizeRange.x, randomizeRange.y) : 0);
            var s2 = s * s;

            var r = s2 * s2 - _gravityY * (_gravityY * dirMagnitude * dirMagnitude + 2 * y * s2);
            var tanTheta = (s2 + Mathf.Sqrt(r)) / (_gravityY * dirMagnitude);
            var cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
            var sinTheta = cosTheta * tanTheta;

            _launchVelocity = new Vector3(s * cosTheta * dir.x, s * sinTheta, s * cosTheta * dir.y);
            if (float.IsNaN(_launchVelocity.x) || float.IsNaN(_launchVelocity.y) || float.IsNaN(_launchVelocity.z))
                _launchVelocity = targetPos;
        }

        protected override void Update()
        {
            base.Update();
            if (IsStopped)
                return;
            
            _lerpTime += Time.deltaTime * flySpeed;
            var pos = _launchPoint + _launchVelocity * _lerpTime;
            pos.y -= yOffset * _gravityY * _lerpTime * _lerpTime;
            transform.position = pos;
            var dir = _launchVelocity;
            dir.y -= _gravityY * _lerpTime;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
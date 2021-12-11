using System;
using Combat.Projectiles.Modificators;
using Entities;
using Interfaces;
using UnityEngine;

namespace Combat.Projectiles
{
    public class StaticElectricityProjectile : BaseProjectile
    {
        public event Action OnTargetDead;
        public event Action<IDamageable> OnArrived;

        [SerializeField] private float speed = 100;
        
        private IDamageable _target;
        private float _lerpTime;
        private bool _shouldLerp;

        public override void Init(BaseCharacter owner, float damage, LayerMask layerMask)
        {
            Mods.Add(new StaticElectricityProjectileModificator(this));
            base.Init(owner, damage, layerMask);
        }

        public void SetNextTarget(IDamageable target)
        {
            _target = target;
            _shouldLerp = true;
            _lerpTime = 0;
            if (target.IsDead)
                OnTargetDead.Invoke();
        }

        public void Update()
        {
            if (!_shouldLerp)
                return;

            _lerpTime += Time.deltaTime * speed;

            if (_lerpTime < 1)
                transform.position = Vector3.Lerp(transform.position, _target.transform.position, _lerpTime);
            else
                OnLerpEnd();
        }

        private void OnLerpEnd()
        {
            _shouldLerp = false;
            OnArrived.Invoke(_target);
        }

        public void Kill()
        {
            Destroy(gameObject);
        }

        protected override void DoHit(IDamageable damageable, Collider coll)
        {
            base.DoHit(damageable, coll);
            movingDamager.IsStopped = true;
        }
    }
}
using System.Collections.Generic;
using Entities;
using Extensions;
using Interfaces;
using UnityEngine;

namespace Combat.Projectiles.Modificators
{
    public class StaticElectricityProjectileModificator : BaseProjectile, IProjectileModificator
    {
        [SerializeField] private float speed = 100;
        [SerializeField] private int chargesAmount = 3;
        [SerializeField] private float damageDecreaseMod = 35;
        [SerializeField] private float radiusToFindNextTarget = 10;
        
        private IDamageable _target;
        private float _lerpTime;
        private bool _shouldLerp;
        private Collider[] _collBuf = new Collider[150];
        private float _nextDamage;
        private bool _hasApplied;
        private int _currentCharges;
        private List<IDamageable> _affected = new List<IDamageable>();

        private void Awake()
        {
            transform.localPosition = Vector3.zero;
        }

        public void ApplyMod(BaseProjectile projectile, BaseCombatEntity entity, float damage)
        {
            if (_hasApplied)
                return;

            Mods = projectile.CopyMods();
            Mods.Remove(this);
            
            transform.SetParent(null);
            _hasApplied = true;
            _currentCharges++;
            _affected.Add(entity);
            
            _nextDamage = damage - ((damage / 100f) * damageDecreaseMod);
            
            FlyToNextEntity();
        }

        private void FlyToNextEntity()
        {
            var nextNearest = GetNearestCharacter();
            if (nextNearest != null)
            {
                _affected.Add(nextNearest);
                SetNextTarget(nextNearest);
            }
        }

        private void SetNextTarget(IDamageable target)
        {
            _target = target;
            _shouldLerp = true;
            _lerpTime = 0;
            if (target.IsDead)
                ElectricityMissileOnTargetDead();
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
            ElectricityMissileOnArrived(_target);
        }

        private void Kill()
        {
            Destroy(gameObject);
        }
        
        private void ElectricityMissileOnTargetDead()
        {
            FlyToNextEntity();
        }
        
        private void ElectricityMissileOnArrived(IDamageable target)
        {
            target.TakeDamage(new HitInfo(this, _nextDamage, Owner));
            _currentCharges++;
            if (chargesAmount >= _currentCharges)
            {
                _nextDamage = _nextDamage - ((_nextDamage / 100f) * damageDecreaseMod);
                FlyToNextEntity();
            }
            else
            {
                Kill();
            }
        }

        private IDamageable GetNearestCharacter()
        {
            var lastPos = _affected[_affected.Count - 1].transform.position;
            var count = Physics.OverlapSphereNonAlloc(lastPos, radiusToFindNextTarget, _collBuf, TargetLayerMask);
            var minDistance = float.MaxValue;
            IDamageable nearestDamageable = null;
            for (int i = 0; i < count; i++)
            {
                var coll = _collBuf[i];
                var damageable = coll.GetDamageable();
                if (_affected.Contains(damageable))
                    continue;
                var distance = Vector3.Distance(lastPos, coll.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestDamageable = damageable;
                }
            }

            if (nearestDamageable == null)
            {
                Kill();
            }
            return nearestDamageable;
        }
    }
}
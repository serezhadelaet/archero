using System.Collections.Generic;
using Entities;
using Extensions;
using Interfaces;
using UnityEngine;

namespace Combat.Projectiles.Modificators
{
    public class StaticElectricityProjectileModificator : IProjectileModificator
    {
        private const int ChargesAmount = 3;
        private const float DamageMod = 35;
        private const float Radius = 10;

        private Collider[] _collBuf = new Collider[150];
        private StaticElectricityProjectile _electricityMissile;
        
        private float _nextDamage;
        private bool _hasApplied;
        private int _currentCharges;
        private List<IDamageable> _affected = new List<IDamageable>();

        public StaticElectricityProjectileModificator(StaticElectricityProjectile electricityMissile)
        {
            _electricityMissile = electricityMissile;
            _electricityMissile.OnArrived += ElectricityMissileOnArrived;
            _electricityMissile.OnTargetDead += ElectricityMissileOnTargetDead;
        }

        public void ApplyMod(BaseProjectile projectile, BaseCombatEntity entity, float damage)
        {
            if (_hasApplied)
                return;
            _hasApplied = true;
            _electricityMissile.gameObject.SetActive(true);
            _electricityMissile.transform.position = projectile.transform.position;
            _currentCharges++;
            _affected.Add(entity);
            
            _nextDamage = damage - ((damage / 100f) * DamageMod);
            
            FlyToNextEntity();
        }

        private void FlyToNextEntity()
        {
            var nextNearest = GetNearestCharacter();
            if (nextNearest != null)
            {
                _affected.Add(nextNearest);
                _electricityMissile.SetNextTarget(nextNearest);
            }
        }

        private void ElectricityMissileOnTargetDead()
        {
            FlyToNextEntity();
        }
        
        private void ElectricityMissileOnArrived(IDamageable target)
        {
            target.TakeDamage(new HitInfo(_electricityMissile, _nextDamage, _electricityMissile.Owner));
            _currentCharges++;
            if (ChargesAmount >= _currentCharges)
            {
                _nextDamage = _nextDamage - ((_nextDamage / 100f) * DamageMod);
                FlyToNextEntity();
            }
            else
            {
                _electricityMissile.Kill();
            }
        }

        private IDamageable GetNearestCharacter()
        {
            var lastPos = _affected[_affected.Count - 1].transform.position;
            var count = Physics.OverlapSphereNonAlloc(lastPos, Radius, _collBuf, _electricityMissile.TargetLayerMask);
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
                _electricityMissile.Kill();
            }
            return nearestDamageable;
        }
    }
}
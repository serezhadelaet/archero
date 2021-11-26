using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Combat.Projectiles
{
    public class StaticElectricityProjectileModificator : IProjectileModificator
    {
        private const int ChargesAmount = 3;
        private const float DamageMod = 35;
        private const float Radius = 10;

        private Collider[] _collBuf = new Collider[150];
        private StaticElectricityMissileProjectile _electricityMissile;
        
        private float _nextDamage;
        private bool _hasApplied;
        private int _currentCharges;
        private List<BaseCombatEntity> _affected = new List<BaseCombatEntity>();

        public StaticElectricityProjectileModificator(StaticElectricityMissileProjectile electricityMissile)
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
        
        private void ElectricityMissileOnArrived(BaseCombatEntity target)
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

        private BaseCombatEntity GetNearestCharacter()
        {
            var lastPos = _affected[_affected.Count - 1].transform.position;
            var count = Physics.OverlapSphereNonAlloc(lastPos, Radius, _collBuf, _electricityMissile.TargetLayerMask);
            var minDistance = float.MaxValue;
            BaseCombatEntity nearestEntity = null;
            for (int i = 0; i < count; i++)
            {
                var coll = _collBuf[i];
                var combatEntity = coll.GetComponentInParent<BaseCombatEntity>();
                if (_affected.Contains(combatEntity))
                    continue;
                var distance = Vector3.Distance(lastPos, coll.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEntity = combatEntity;
                }
            }

            if (nearestEntity == null)
            {
                _electricityMissile.Kill();
            }
            return nearestEntity;
        }
    }
}
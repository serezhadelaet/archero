using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using Entities;
using UnityEngine;

namespace Combat.Projectiles
{
    public class StaticElectricityProjectileModificator : IProjectileModificator
    {
        private const int ChargesAmount = 3;
        private const float DamageMod = 35;
        private const float Radius = 10;

        private Collider[] _collBuf = new Collider[30];
        private StaticElectricityMissile _electricityMissile;
        
        private float _nextDamage;
        private int _currentCharges;
        private List<BaseCombatEntity> _affected = new List<BaseCombatEntity>();

        public StaticElectricityProjectileModificator(StaticElectricityMissile electricityMissile)
        {
            _electricityMissile = electricityMissile;
            _electricityMissile.OnArrived += ElectricityMissileOnArrived;
            _electricityMissile.OnTargetDead += ElectricityMissileOnTargetDead;
            _electricityMissile.Mods.Remove(this);
        }

        public void ApplyMod(BaseProjectile projectile, BaseCombatEntity entity, float damage)
        {
            _currentCharges++;
            _affected.Add(entity);
            
            _nextDamage = damage - ((damage / 100f) * DamageMod);
            FlyToNextEntity();
        }

        private void FlyToNextEntity()
        {
            var nextNearest = GetNearestCharacter();
            if (nextNearest != null)
                _electricityMissile.SetNextTarget(nextNearest);
        }

        private void ElectricityMissileOnTargetDead()
        {
            FlyToNextEntity();
        }
        
        private void ElectricityMissileOnArrived(BaseCombatEntity target)
        {
            target.TakeDamage(new HitInfo(_electricityMissile, _nextDamage, _electricityMissile.Owner));
            _currentCharges++;
            if (_currentCharges < ChargesAmount)
            {
                _nextDamage = _nextDamage - ((_nextDamage / 100f) * DamageMod);
                FlyToNextEntity();
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
                var combatEntity = coll.GetComponent<BaseCombatEntity>();
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
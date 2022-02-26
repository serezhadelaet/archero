using System.Collections.Generic;
using Entities;
using Extensions;
using UnityEngine;

namespace Helpers
{
    [CreateAssetMenu]
    public class CombatTargetingSo : ScriptableObject
    {
        [SerializeField] private LayerMask _blockersLayerMask;

        private readonly List<BaseCombatEntity> _targets = new List<BaseCombatEntity>();

        public void AddTarget(BaseCombatEntity target)
        {
            if (!_targets.Contains(target))
                _targets.Add(target);
        }

        public void RemoveTarget(BaseCombatEntity target)
        {
            _targets.Remove(target);
        }

        public BaseCombatEntity GetTarget(Vector3 pos, float radius, float meleeRadius, out bool meleeFound)
        {
            var nearestDistance = float.MaxValue;
            BaseCombatEntity nearestTarget = default;
            
            for (int i = 0; i < _targets.Count; i++)
            {
                var target = _targets[i];
                if (ShouldSkip(target))
                    continue;
                
                if (Blocking(pos, target.transform.position))
                    continue;
                
                var distance = pos.PlanarDistance(target.transform.position);
                if (distance <= radius && distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTarget = target;
                }
            }

            meleeFound = meleeRadius >= nearestDistance;
            
            return nearestTarget ? nearestTarget : null;

            bool ShouldSkip(BaseCombatEntity target)
            {
                return !target || target.IsDead;
            }

            bool Blocking(Vector3 start, Vector3 end)
            {
                return Physics.Linecast(start, end, _blockersLayerMask);
            }
        }
    }
}
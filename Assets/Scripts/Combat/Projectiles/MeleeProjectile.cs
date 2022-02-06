using System.Collections.Generic;
using Extensions;
using Interfaces;
using UnityEngine;

namespace Combat.Projectiles
{
    public class MeleeProjectile : BaseProjectile
    {
        [SerializeField] private float _splashRange;
        
        private Collider[] _collBuff = new Collider[50];
        
        public override void Shoot(Vector3 dir, Vector3 targetPos)
        {
            var list = new List<IDamageable>();
            var count = Physics.OverlapSphereNonAlloc(transform.position, _splashRange, _collBuff, TargetLayerMask);
            for (int i = 0; i < count; i++)
            {
                var coll = _collBuff[i];
                var combatEntity = coll.GetDamageable();
                if (combatEntity == null || combatEntity.IsDead || list.Contains(combatEntity))
                    continue;
                
                list.Add(combatEntity);
                DoHit(combatEntity, coll);
            }
            
            ReturnToPool();
        }
    }
}
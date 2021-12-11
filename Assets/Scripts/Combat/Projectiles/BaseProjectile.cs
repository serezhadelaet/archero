using System.Collections.Generic;
using Combat.Projectiles.Modificators;
using Combat.Projectiles.MovingDamagers;
using Entities;
using Interfaces;
using UnityEngine;

namespace Combat.Projectiles
{
    public abstract class BaseProjectile : MonoBehaviour
    {
        public LayerMask TargetLayerMask { get; private set; }
        public BaseCharacter Owner { get; private set; }
        
        [SerializeField] protected MovingDamager movingDamager;
        [SerializeField] private float destroyIn = 2;
        
        public List<IProjectileModificator> Mods = new List<IProjectileModificator>();
        protected float Damage;
        private Vector3 _direction;
        
        public virtual void Init(BaseCharacter owner, float damage, LayerMask layerMask)
        {
            Owner = owner;
            Damage = damage;
            TargetLayerMask = layerMask;
            
            Destroy(gameObject, destroyIn);
        }

        public virtual void Shoot(Vector3 dir, Vector3 targetPos)
        {
            _direction = dir;
            movingDamager.Init(dir, targetPos, TargetLayerMask, DoHit, Owner);
        }

        protected virtual void DoHit(IDamageable damageable, Collider coll)
        {
            damageable?.TakeDamage(new HitInfo(this, Damage, Owner, coll, _direction));
        }
        
        public void OnAfterHit(BaseCombatEntity entity, float damage)
        {
            ModsAffect(entity, damage);
        }

        private void ModsAffect(BaseCombatEntity entity, float damage)
        {
            if (Mods == null)
                return;
            foreach (var mod in Mods)
                mod.ApplyMod(this, entity, damage);
        }
    }
}
using System;
using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Combat.Projectiles
{
    public abstract class BaseProjectile : MonoBehaviour, IProjectile
    {
        public LayerMask TargetLayerMask { get; private set; }
        public BaseCharacter Owner { get; private set; }
        
        [SerializeField] private float destroyIn = 2;
        
        public List<IProjectileModificator> Mods = new List<IProjectileModificator>();
        protected float Damage;
        
        public virtual void Init(BaseCharacter owner, float damage, LayerMask layerMask)
        {
            Owner = owner;
            Damage = damage;
            TargetLayerMask = layerMask;
            
            Destroy(gameObject, destroyIn);
        }

        public virtual void Shoot(Vector3 dir) { }
        
        public virtual void OnHit(BaseCombatEntity entity, float damage)
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
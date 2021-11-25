﻿using System;
using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Combat.Projectiles
{
    public abstract class BaseProjectile : MonoBehaviour, IProjectile
    {
        [NonSerialized] public LayerMask TargetLayerMask;
        [NonSerialized] public BaseCharacter Owner;
        [SerializeField] private float destroyIn = 2;
        
        public List<IProjectileModificator> Mods = new List<IProjectileModificator>();
        protected float Damage;
        
        public void Init(BaseCharacter owner, float damage, LayerMask layerMask, List<IProjectileModificator> mods)
        {
            Owner = owner;
            Damage = damage;
            TargetLayerMask = layerMask;
            Mods = mods;
            
            Destroy(gameObject, destroyIn);
        }

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
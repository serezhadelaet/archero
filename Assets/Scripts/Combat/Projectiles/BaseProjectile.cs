using System.Collections.Generic;
using System.Linq;
using Combat.Projectiles.Modificators;
using Combat.Projectiles.MovingDamagers;
using Entities;
using Interfaces;
using UnityEngine;

namespace Combat.Projectiles
{
    public abstract class BaseProjectile : MonoBehaviour, IPooled<BaseProjectile>
    {
        public LayerMask TargetLayerMask { get; private set; }
        public BaseCharacter Owner { get; private set; }
        
        [SerializeField] protected MovingDamager movingDamager;
        [SerializeField] private float destroyIn = 2;
        
        public List<IProjectileModificator> Mods = new List<IProjectileModificator>();
        public float Damage { get; private set; }
        private Vector3 _direction;
        private IPool<BaseProjectile> _pool;
        
        public void Init(BaseCharacter owner, float damage, LayerMask layerMask)
        {
            Owner = owner;
            Damage = damage;
            TargetLayerMask = layerMask;
            
            Invoke(nameof(ReturnToPool), destroyIn);
        }

        protected void ReturnToPool()
        {
            _pool.Return(this);
        }

        public virtual void Shoot(Vector3 dir, Vector3 targetPos)
        {
            _direction = dir;
            movingDamager?.Init(dir, targetPos, TargetLayerMask, DoHit, Owner);
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

        public List<IProjectileModificator> CopyMods()
        {
            var tempModsArray = new IProjectileModificator[Mods.Count];
            Mods.CopyTo(tempModsArray);
            return tempModsArray.ToList();
        }

        public void SetPool(IPool<BaseProjectile> pool)
        {
            _pool = pool;
        }

        public void OnReturnToPool()
        {
            CancelInvoke(nameof(ReturnToPool));
        }
    }
}
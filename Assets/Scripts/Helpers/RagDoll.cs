using Combat;
using UnityEngine;

namespace Helpers
{
    public class RagDoll : MonoBehaviour
    {
        [SerializeField] private Rigidbody[] rigidbodies = null;
        [SerializeField] private Animator animator = null;
        [SerializeField] private float forceMod = 2500;
        private bool _isRagDoll = true;
        
        private void Awake()
        {
            SetAsRagDoll(false);
        }

        public void SetAsRagDoll(bool f, HitInfo lastHit)
        {
            if (_isRagDoll == f)
                return;

            _isRagDoll = f;
            SetKinematicRigidBodies(!f);
            animator.enabled = !f;
            
            lastHit.HitCollider.GetComponent<Rigidbody>().AddForce(lastHit.ForceDir * forceMod);
        }

        private void SetAsRagDoll(bool f)
        {
            if (_isRagDoll == f)
                return;

            _isRagDoll = f;
            SetKinematicRigidBodies(!f);
            animator.enabled = !f;
        }
        
        private void SetKinematicRigidBodies(bool f)
        {
            foreach (var rigidBody in rigidbodies)
            {
                rigidBody.isKinematic = f;
                rigidBody.interpolation = f ? RigidbodyInterpolation.None : RigidbodyInterpolation.Extrapolate;
            }
        }
    }
}
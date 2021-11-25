using System;
using UnityEngine;

namespace Helpers
{
    public class DelayDestroyer : MonoBehaviour
    {
        [SerializeField] private float destroyIn = 1;

        private void Awake()
        {
            Destroy(gameObject, destroyIn);
        }
    }
}
using System;
using UnityEngine;

namespace Helpers
{
    [CreateAssetMenu]
    public class PlayerProgressionFollower : ScriptableObject
    {
        [SerializeField] private int maxLevel = 3;
        
        public event Action OnProgress;
        
        private int _expPoints;
        
        public void Progress()
        {
            _expPoints++;
            OnProgress?.Invoke();
        }

        private void OnEnable()
        {
            _expPoints = 0;
        }

        public int GetExp() => _expPoints;
        public int GetLevel() => Mathf.Min(maxLevel, Mathf.FloorToInt(_expPoints / 2f) + 1);
    }
}
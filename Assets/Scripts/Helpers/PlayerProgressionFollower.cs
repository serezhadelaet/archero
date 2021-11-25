using UnityEngine;

namespace Helpers
{
    [CreateAssetMenu]
    public class PlayerProgressionFollower : ScriptableObject
    {
        private int _expPoints;
        
        public void OnProgress()
        {
            _expPoints++;
        }

        public int GetLevel() => (_expPoints / 2) + 1;
    }
}
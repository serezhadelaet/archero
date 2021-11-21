using Entities;
using UnityEngine;

namespace Levels
{
    public class CharacterFactory : MonoBehaviour
    {
        [SerializeField] private Player playerPrefab;
        [SerializeField] private Enemy enemyPrefab;
        
        public Player SpawnPlayer(Vector3 pos, Quaternion rot, Transform parent)
        {
            return Instantiate(playerPrefab, pos, rot, parent);
        }
        
        public Enemy SpawnEnemy(Vector3 pos, Quaternion rot, Transform parent)
        {
            return Instantiate(enemyPrefab, pos, rot, parent);
        }
    }
}
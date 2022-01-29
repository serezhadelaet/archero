using UnityEngine;
using Zenject;

namespace Levels
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private Level[] levels;
        
        private PlaceholderFactory<Level, Level> _levelsFactory;
        
        [Inject]
        private void Construct(PlaceholderFactory<Level, Level> levelsFactory)
        {
            _levelsFactory = levelsFactory;
        }

        private void Start()
        {
            _levelsFactory.Create(levels[Random.Range(0, levels.Length)]);
        }
    }
}
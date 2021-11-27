using UnityEngine;
using Zenject;

namespace Levels
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private Level[] levels;
        
        private LevelsFactory _levelsFactory;
        
        [Inject]
        private void Construct(LevelsFactory levelsFactory)
        {
            _levelsFactory = levelsFactory;
        }

        private void Start()
        {
            _levelsFactory.Create(levels[Random.Range(0, levels.Length)]);
        }
    }
}
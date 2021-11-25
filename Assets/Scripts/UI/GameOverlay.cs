using TMPro;
using UnityEngine;

namespace UI
{
    public class GameOverlay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI lvlText;

        public void UpdateLevel(int level)
        {
            lvlText.text = "Player level: " + level;
        }
        
        public void UpdateHealth(int health)
        {
            healthText.text = "Player HP: " + health;
        }
    }
}
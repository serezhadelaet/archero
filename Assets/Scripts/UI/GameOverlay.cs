using TMPro;
using UnityEngine;

namespace UI
{
    public class GameOverlay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI expText;
        [SerializeField] private TextMeshProUGUI lvlText;

        public void UpdateExp(int exp)
        {
            expText.text = "Player experience: " + exp;
        }
        
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
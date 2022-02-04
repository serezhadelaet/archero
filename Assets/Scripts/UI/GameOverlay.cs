using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameOverlay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI expText;
        [SerializeField] private TextMeshProUGUI lvlText;
        [SerializeField] private Image dashImage;
        [SerializeField] private float dashFillDuration = 0.15f;
        private Tween _dashTween;
        
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
        
        public void UpdateDashCounter(int amount, int max)
        {
            _dashTween?.Kill();
            _dashTween = dashImage.DOFillAmount(1 - (amount * 1f) / max, dashFillDuration)
                .SetEase(Ease.InOutQuad);
        }
    }
}
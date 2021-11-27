using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image fillBar;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private float _maxHealth;

        public void SetMaxHealth(float maxHealth)
        {
            _maxHealth = maxHealth;
        }

        public void SetHealth(float health)
        {
            fillBar.fillAmount = health / _maxHealth;
            if (fillBar.fillAmount == 0)
                gameObject.SetActive(false);
        }

        private void Update()
        {
            transform.LookAt(transform.position + _camera.transform.rotation * Vector3.back,
                _camera.transform.rotation * Vector3.up);
        }
    }
}
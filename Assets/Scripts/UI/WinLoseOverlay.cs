using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class WinLoseOverlay : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void OnRestartClick()
        {
            SceneManager.LoadScene(0);
        }
    }
}
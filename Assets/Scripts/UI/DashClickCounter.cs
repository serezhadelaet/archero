using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class DashClickCounter : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private float dashClicksMaxDelay = 0.1f;
        [SerializeField] private DashPerformedEvent dashEvent;
        
        private int _clicksCounter;
        private float _lastClickTime;

        private void OnClick()
        {
            if (_clicksCounter == 1 && Time.time - _lastClickTime > dashClicksMaxDelay)
                _clicksCounter = 0;
            
            _clicksCounter++;
            _lastClickTime = Time.time;
            
            if (_clicksCounter < 2)
                return;
                
            _clicksCounter = 0;
            dashEvent.Invoke();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_clicksCounter == 1 && Time.time - _lastClickTime > dashClicksMaxDelay)
                _clicksCounter = 0;
            
            _clicksCounter++;
            _lastClickTime = Time.time;
            
            if (_clicksCounter < 2)
                return;
                
            _clicksCounter = 0;
            dashEvent.Invoke();
        }
    }
}
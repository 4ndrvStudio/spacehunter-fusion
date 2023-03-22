using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SH.Multiplayer
{
    public class UIButtonCustom : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
    {
        
        public bool IsPressed = default;

        [SerializeField] private float _startTimeDelay = default;
        [SerializeField] private float _timeDelay = default;
        [SerializeField] private bool _canPress = true;

        public void Awake() {
            _canPress = true;
            _timeDelay = _startTimeDelay;
        }

        public void Update() {
            if(_canPress == false) {

                _timeDelay -= Time.deltaTime;

            }
            if(_timeDelay <= 0) {
                _timeDelay = _startTimeDelay;
                _canPress = true;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(_canPress) {
                IsPressed = true;
                _canPress = false;
            }
          
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
        }

        IEnumerator PressButtonState() {

            yield return new WaitForSeconds(_timeDelay);
            IsPressed = true;
        }

    }

}

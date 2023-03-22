using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SH.Multiplayer
{
    public class JoystickCustom : Joystick
    {
        private Vector3 StartPos;


        protected override void Start()
        {
            base.Start();
            StartPos = background.anchoredPosition;
        }


        public override void OnPointerDown(PointerEventData eventData)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            background.anchoredPosition = StartPos;
            base.OnPointerUp(eventData);
        }

        public JoystickCustom GetJoyStick()
        {
            return this;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SH.Multiplayer
{
    public class UILookTouchZone : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
    {
        [SerializeField] private Vector2 StartPosition;
        [SerializeField] private Vector2 EndPosition;

      


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void OnPointerDown(PointerEventData eventData)
        {
          
            StartPosition = eventData.position;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            
        }

    }

}

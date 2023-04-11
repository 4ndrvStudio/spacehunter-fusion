using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SH.Multiplayer
{
    public class UITouchPanel : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public static UITouchPanel Instance;
        private Vector2 _playerTouchVectorOutput;
        private bool _isPlayerTouchingPanel;
        private Touch _myTouch;
        private int _touchID;

        public bool IsDragging;

        private Vector2 _startPos;
        PointerEventData eventDatas;

        void Start()
        {

            if (Instance == null) Instance = this;

        }

        void Update()
        {
        }

        // private void FixedUpdate()
        // {
        //     if (Input.touchCount > 0)
        //     {
        //         for (int i = 0; i < Input.touchCount; i++)
        //         {
        //             _myTouch = Input.GetTouch(i);
        //             if (_isPlayerTouchingPanel)
        //             {
        //                 if (_myTouch.fingerId == _touchID)
        //                 {

        //                     if (_myTouch.phase != TouchPhase.Moved) {
        //                         IsDragging = true;

        //                 } else  {
        //                                                         IsDragging = false;

        //                 }
        //                         OutputVectorValue(Vector2.zero);
        //                 }
        //             }
        //         }
        //     }
        // }

        // private void OutputVectorValue(Vector2 outputValue)
        // {
        //     _playerTouchVectorOutput = outputValue;
        // }

        public Vector2 PlayerJoystickOutputVector()
        {
            return _playerTouchVectorOutput;
        }

        // public void OnPointerUp(PointerEventData _onPointerUpData)
        // {
        //     OutputVectorValue(Vector2.zero);
        //     _isPlayerTouchingPanel = false;
        // }

        // public void OnPointerDown(PointerEventData _onPointerDownData)
        // {
        //     OnDrag(_onPointerDownData);
        //     _touchID = _myTouch.fingerId;
        //     _isPlayerTouchingPanel = true;
        // }

        // public void OnDrag(PointerEventData _onDragData)
        // {

        //     OutputVectorValue(new Vector2(_onDragData.delta.normalized.x, _onDragData.delta.normalized.y));

        // }
        public UITouchPanel GetUITouchPanel() => Instance;

        public void OnBeginDrag(PointerEventData _onDragData)
        {
            _startPos = _onDragData.delta;
        }

        public void OnEndDrag(PointerEventData _onDragData)
        {
            _startPos = Vector2.zero;

            _playerTouchVectorOutput = Vector2.zero;

            Debug.Log("Endrag");


        }
        public void OnDrag(PointerEventData _onDragData)
        {
            eventDatas = _onDragData;

            IsDragging = _onDragData.delta != Vector2.zero;
            Vector2 delta = _onDragData.delta / 35.0f;

   
            if (delta.magnitude > 0.1f)
            {
                _playerTouchVectorOutput = _onDragData.delta.normalized;
            }
            else
            {
                _playerTouchVectorOutput = Vector2.zero;
            }



        }

        // public void OnPointerInside(PointerEventData eventData)
        // {
        //     Debug.Log("helsd");
        //     IsDragging =eventData.IsPointerMoving();
        // }
    }

}

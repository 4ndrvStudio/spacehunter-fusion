using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Multiplayer.Player
{
    using FishNet;
    using FishNet.Object;
    using FishNet.Object.Prediction;
    using FishNet.Transporting;
    using FishNet.Component.Prediction;
    
    public class Player : NetworkBehaviour
    {
        public static Player Instance;

        [Header("Component")]
        public PredictedObject PredictedObject;
        public PlayerMovement PlayerMovement;


        [Header("Body Parts")]
        [SerializeField] private Transform _body;
        [SerializeField] private Transform _lookPoint;


        public override void OnStartClient()
        {
            base.OnStartClient();

            if (IsOwner == false) return;

            if (Instance == null)
                Instance = this;

            var targetLookPoint = Resources.Load<GameObject>("Camera/LookPoint");
            GameObject lookPoint = Instantiate(targetLookPoint);
            lookPoint.GetComponent<TargetLookPoint>().TargetFollow = _body.transform;

            CameraManager.Instance.SetAimTarget(_body, lookPoint.transform);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;





        }





    }

}

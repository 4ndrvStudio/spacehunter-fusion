using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_UIFaceToCamera : MonoBehaviour
    {
        void Update()
        {
            if (Application.isBatchMode == false)
            {
                transform.LookAt(CameraManager.Instance.GetTransform());
            }

        }
    }

}

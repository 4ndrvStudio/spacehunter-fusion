using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_UIFaceToCamera : MonoBehaviour
    {
       void Update() {
            transform.LookAt(Network_CameraManager.Instance.GetTransform());
       }
    }

}

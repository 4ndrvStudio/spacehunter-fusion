using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using SH.Define;
using SH.Networking.Station;
using SH.Multiplayer;
using UnityEngine.Events;

namespace SH
{
    public class Portal : MonoBehaviour
    {

        [SerializeField] private PortalClass _portalClass;
        [SerializeField] private TextMeshPro _textMesh;
        [SerializeField] private GameObject _videoChangeScene;

        private bool _isEnterPortal = false;

        private void Awake()
        {
            _textMesh.text = _portalClass.NamePortal.ToString();
            _textMesh.fontSize = _portalClass.NameSize;
            _isEnterPortal = false;
        }

        private void LateUpdate()
        {
            NameFaceToPlayer();
        }

        

        private void OnTriggerEnter(Collider other)
        {   

            if (other.gameObject.tag == "Player")
            {
                if (_portalClass.Name == PortalName.Planet_1)
                {
                    if (!_isEnterPortal)
                    {
                        Debug.Log($"Enter {_portalClass.Name}");
                        Network_ClientManager.MoveToRoom(SceneDefs.scene_miningFusion);
                        _isEnterPortal = true;
                    }
                }
                else
                {
                    UIManager.Instance.ShowAlert("Feature is coming soon!", AlertType.Normal);
                }
            }
        }


        private void NameFaceToPlayer()
        {
            _textMesh.transform.LookAt(Network_CameraManager.Instance.GetTransform());
            _textMesh.transform.Rotate(0, 180f, 0);
        }






    }
}

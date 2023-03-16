using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using SH.Define;
using SH.Networking.Station;

namespace SH
{
    public class Portal : MonoBehaviour
    {

        [SerializeField] private PortalClass _portalClass;
        [SerializeField] private TextMeshPro _textMesh;
        [SerializeField] private GameObject _videoChangeScene;

        private bool _isEnterPortal = false;
        private bool _canEnterPortal = true;

        private void Awake()
        {
            _textMesh.text = _portalClass.NamePortal.ToString();
            _textMesh.fontSize = _portalClass.NameSize;
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
                    if (other.gameObject.transform.parent.GetComponent<RoomStationNetworkEntityView>()?.IsMine == true)
                    {
                        Debug.Log($"Enter {_portalClass.Name}");
                        RoomStationManager.Instance.LeaveRoom(() =>
                        {
                             UIManager.Instance.LoadScene(SceneName.SceneMining);
                        });
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
            _textMesh.transform.LookAt(Camera.main.transform);
            _textMesh.transform.Rotate(0, 180f, 0);
        }






    }
}

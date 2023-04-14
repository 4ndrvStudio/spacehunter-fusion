using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SH.Multiplayer;

namespace SH.NPC
{
    public class NPC_Interaction : MonoBehaviour
    {
        [SerializeField] private LayerMask _npcMask;
        [SerializeField] private bool _isInteraction;
        [SerializeField] private float _disToPlayer;
        [SerializeField] private float _rotateSpeed;

        // Update is called once per frame
        void Update()
        {
            if(Network_Player.Local != null)
                _disToPlayer = Vector3.Distance(this.transform.position, Network_Player.Local.transform.position);
            
            // Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Network_CameraManager.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);

                Debug.DrawRay(ray.origin, ray.direction, Color.blue, 100f);
                if (Physics.Raycast(ray, out hit, 1000, _npcMask))
                {
                    if (hit.transform == this.transform)
                        _isInteraction = true;

                }
            }

            if (_isInteraction)
            {
                LookToPlayer();
            }
            

            if(_disToPlayer > 4f && _isInteraction == true)
                _isInteraction = false;

        }

        private void LookToPlayer()
        {
            transform.LookAt(Network_Player.Local.transform);

            Vector3 eulerAngles = transform.rotation.eulerAngles;
            eulerAngles.x = 0;
            eulerAngles.z = 0;

            transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(eulerAngles),_rotateSpeed * Time.deltaTime);
        
        }






    }

}



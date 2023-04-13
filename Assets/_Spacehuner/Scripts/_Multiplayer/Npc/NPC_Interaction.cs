using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SH.Multiplayer;

namespace SH.NPC
{
    public class NPC_Interaction : MonoBehaviour
    {
        [SerializeField] private LayerMask _npcMask;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("rayy");

                Ray ray = new Ray(
                    Network_CameraManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition),
                    Network_CameraManager.Instance.MainCamera.transform.forward);

                RaycastHit hit;
                
                Debug.DrawRay(ray.origin,ray.direction *1000,Color.red, 100f);
                Debug.Log(ray.origin);

                if (Physics.Raycast(ray, out hit, 1000000f, _npcMask))
                {
                    Debug.Log("hitted");
                }
            }
        }
        
        void OnMouseDown() {
                    Debug.Log("Click!");

        }
    }

}

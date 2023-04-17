using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SH.Multiplayer;
using SH.Dialogue;
namespace SH.NPC
{
    public class NPC_Interaction : MonoBehaviour
    {

        [SerializeField] private float _distToInteract;
        [SerializeField] private float _disToPlayer;
        [SerializeField] private float _rotateSpeed;

        [SerializeField] private Vector3 _rootEuler;

        [SerializeField] public TextAsset DialogueContent;
        
        void Awake() {
            _rootEuler = this.transform.rotation.eulerAngles;
         
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            CheckPlayerNear();
        }
        private void CheckPlayerNear() {

            if(Network_Player.Local == null) return;

            _disToPlayer = Vector3.Distance(Network_Player.Local.transform.position, this.transform.position);
           
            if(_disToPlayer <= _distToInteract )  {

                LookToPosition(Network_Player.Local.transform.position);

            }  else {

                transform.rotation =Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(_rootEuler),_rotateSpeed * Time.deltaTime);
  
            }
        }

        private void LookToPosition(Vector3 targetRot)
        {
            var lookPos = targetRot - transform.position;

            lookPos.y = 0;

            var rotation = Quaternion.LookRotation(lookPos);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotateSpeed/10 * Time.deltaTime);
            
        }






    }

}



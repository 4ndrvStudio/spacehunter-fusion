using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Fusion;
using SH.NPC;
using SH.Dialogue;

namespace SH.Multiplayer
{
    


    public class Network_PlayerSensitivityEnvironment : NetworkBehaviour
    {

        [Header("NPC Checker")]

        [SerializeField] private Network_PlayerState _playerState;

        [SerializeField] private LayerMask _npcMask;

        [SerializeField] private float _overlapseRadius  = 10f;

        [SerializeField] private float _rangeToInteract = 5f;


        // Update is called once per frame
        void FixedUpdate()
        {
            if(Object.HasInputAuthority == false) return;

            CheckNPCNear();

        }

        private void CheckNPCNear() {
            
            Collider[] npcCollider = Physics.OverlapSphere(transform.position, _overlapseRadius, _npcMask);
                       
            for(int i = 0 ; i<= npcCollider.Length -1; i++) {

                float dist = Vector3.Distance(this.transform.position,npcCollider[i].transform.position);
                
                string npcName = npcCollider[i].gameObject.GetComponent<NPC_Brain>().Name;
                
                if(npcCollider[i].gameObject.GetComponent<NPC_Brain>().NpcState != NPCState.Receptionist) return;

                if(DialogueManager.DialogueIsPlaying == true) return;
                
                if(dist <= _rangeToInteract && _playerState.L_IsAction != true) {
                    TextAsset dialogueContent = npcCollider[i].gameObject.GetComponent<NPC_Interaction>().DialogueContent;
  
                    Dictionary<string, object> customProperties = new Dictionary<string, object>() {
                        {InteractButtonCustomProperties.Name.ToString(), npcName },
                        {InteractButtonCustomProperties.ChatContent.ToString(), dialogueContent}
                    };

                    UIControllerManager.Instance.AddInteractButton(npcCollider[i].GetInstanceID(),InteractButtonType.Chat, customProperties);

                } else {
                    UIControllerManager.Instance.RemoveInteractionButton(npcCollider[i].GetInstanceID());
                }
            
             }


        }   


    }



}

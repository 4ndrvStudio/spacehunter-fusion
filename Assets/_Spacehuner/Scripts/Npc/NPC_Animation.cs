using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.NPC
{
    public class NPC_Animation : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        [SerializeField] private NPC_Brain _npcBrain;
        

        // Update is called once per frame
        void Update()
        {
            switch (_npcBrain.NpcState) {
                case NPCState.Idle : 
                    anim.SetBool("isWalking", false);
                    break;
                case NPCState.Walking : 
                    anim.SetBool("isWalking", true);
                    break;
                case NPCState.Sitting : 
                    anim.SetBool("isSitting",true);
                    break;
            }
        }

        public void PlayDance(string danceName) {
            anim.CrossFade(danceName,0.3f,1);
        }
    
    }

}

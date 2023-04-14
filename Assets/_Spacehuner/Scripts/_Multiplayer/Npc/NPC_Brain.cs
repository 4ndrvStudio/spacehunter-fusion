using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.NPC
{
    public class NPC_Brain : MonoBehaviour
    {
        public NPCState NpcState;


    }

    public enum NPCState {
        Idle,
        Walking,
        Sitting,
        Dancing,
        Receptionist
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Multiplayer
{
    public class NPC_Brain : MonoBehaviour
    {
        public NPCState NpcState;


    }

    public enum NPCState {
        Idle,
        Walking,
        Sitting
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Brain : MonoBehaviour
{
    [SerializeField] private NPCState _npcState;
    

    // Start is called before the first frame update
    void Start()
    {
        _npcState = NPCState.Walking;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum NPCState {
    Idle, 
    Walking,
}

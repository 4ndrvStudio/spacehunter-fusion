using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SH.Dialogue;

namespace SH
{
    public class NPC_Test : MonoBehaviour
    {
        [SerializeField] private TextAsset inkJSON;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        
        void OnMouseDown() {
            DialogueManager.Instance.EnterDialogueMode(inkJSON);
        }


    }

}

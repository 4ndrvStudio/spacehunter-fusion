using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.NPC
{
    public class NPC_Dancing : MonoBehaviour
    {
        [SerializeField] private NPC_Brain _npcBrain;
        [SerializeField] private NPC_Animation _npcAnim;

        [SerializeField] private List<string> _dancingName = new List<string>();

        [SerializeField] private float _timeChangeState = 20;
        [SerializeField] private float _timeFactor = 20;
        [SerializeField] private bool _changeStateExcute;
    


        // Start is called before the first frame update
        void Start()
        {
            if(_npcBrain.NpcState != NPCState.Dancing) 
                return;

            if(_dancingName.Count == 1) 
                _npcAnim.PlayDance(_dancingName[0]);
            else if ( _dancingName.Count> 1 ) 
                _changeStateExcute = true;
            
        }

        // Update is called once per frame
        void Update()
        {

            //Random Dance
            if(_changeStateExcute == true) {
                _timeChangeState = Random.Range(_timeFactor/2, _timeFactor);
                _npcAnim.PlayDance(_dancingName[Random.Range(0,_dancingName.Count)]);
                _changeStateExcute = false;

            } else {
                _timeChangeState -= Time.deltaTime;
                if(_timeChangeState<= 0) {
                    _changeStateExcute = true;
                }

            }

        }
    }

}

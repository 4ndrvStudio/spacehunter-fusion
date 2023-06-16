using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SH
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
       
        [SerializeField] private AudioClip _uiClickAudio;
        [SerializeField] private AudioSource _audioSource;
        // Start is called before the first frame update
        void Start()
        {
            if(Instance ==null) 
                Instance = this;
        }

        // Update is called once per frame
        void Update()
        {   
        


        }

        public void PlayUIClick()
        {
            _audioSource.clip = _uiClickAudio;
            _audioSource.Play();
        }
    }

}

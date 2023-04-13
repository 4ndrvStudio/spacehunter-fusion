using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Multiplayer
{
    public class LightBrain : MonoBehaviour
    {
        [SerializeField] private MusicManager _musicManager;   
        [SerializeField] private Light _light;
        [SerializeField] private float _intensityFactor;

        // Update is called once per frame
        void Update()
        {
            _light.intensity = _musicManager.OutDbFactor * _intensityFactor;
        }
    }

}

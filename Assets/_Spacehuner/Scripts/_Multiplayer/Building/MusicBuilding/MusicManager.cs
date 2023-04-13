using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Multiplayer
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private AudioClip _musicTest;
        [SerializeField] private AudioSource _speaker;


        private const int SAMPLE_SIZE = 1024;
        [SerializeField] private float _rmsValue;
        [SerializeField] private float _dbValue;
        [SerializeField] private float _pitchValue;

        [SerializeField]  private float[] _samples;
        [SerializeField] private float[] _spectrum;
        [SerializeField] private float _samplesRate;
        [SerializeField] private float _smoothSpeed = 10f;

        [SerializeField] private float _emiIntensity;
        [SerializeField] private Color _minColor;
        [SerializeField] private Color _maxColor;

        [SerializeField] private float _lerpFactor;

        public float OutDbFactor;

        private void Start() {
            _speaker.clip = _musicTest;
            _speaker.Play();
            _samples = new float[SAMPLE_SIZE];
            _spectrum = new float[SAMPLE_SIZE];
            _samplesRate = AudioSettings.outputSampleRate;
        }

        private void Update() {
            AnalyzeSound();
            UpdateLight();
        }

        private void AnalyzeSound() {
            _speaker.GetOutputData(_samples,0);

            //get the rms
            int i = 0;
            float sum = 0;
            for(; i<SAMPLE_SIZE; i++) {
                sum = _samples[i] * _samples[i];
            }
            _rmsValue = Mathf.Sqrt(sum/SAMPLE_SIZE);
            
            //get db value
            _dbValue = 20* Mathf.Log10(_rmsValue/ 0.1f);

            //get the sound spectrum

            _speaker.GetSpectrumData(_spectrum,0,FFTWindow.BlackmanHarris);

        }

        private void UpdateLight() {

            _emiIntensity -= Time.deltaTime * _lerpFactor;
             if(_emiIntensity < _dbValue/40) 
                _emiIntensity = _dbValue/80;
            OutDbFactor =1-  (-_emiIntensity);
        }
        
    }

}

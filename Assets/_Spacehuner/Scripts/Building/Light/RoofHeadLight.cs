using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VLB;
using DG.Tweening;
namespace SH.Multiplayer
{
    public class RoofHeadLight : MonoBehaviour
    {
        [SerializeField] private MusicManager _musicManager;
        [SerializeField] private VolumetricLightBeam _lightSource;

        private Tweener _normalRotateTween;
        private Tweener _normalRandomColorTween;
        private Tweener _onBeatsRotateTween;
        private Tweener _onBeatsRandomColorTween;

        [SerializeField] private List<Color> _colorList = new List<Color>();
        private int _indexColor = 0;

        [SerializeField] private float _normalRotateSpeed;
        [SerializeField] private float _xFactor;
        [SerializeField] private float _zFactor;
        [SerializeField] private float _timeTest = 1f;

    
        void Awake()
        {
            _indexColor = 0;
            RotateRandom();
            RandomColor();
        }

        // Update is called once per frame
        void Update()
        {
            // _lightSource.spotAngle =_musicManager.OutDbFactor *80;
            _lightSource.intensityGlobal = _musicManager.OutDbFactor * 5;
            _timeTest -=Time.deltaTime;
            _lightSource.enabled = Random.Range(0,3) > 1f && _musicManager.OutDbFactor > 0.5f;

            // if(_timeTest <=0) {
            //     _lightSource.enabled = !_lightSource.enabled;
            //     _timeTest = 0.2f;

            // } 

        }

        void RotateRandom()
        {
            float targetRotateZ = Random.Range(-180f + _xFactor, -180f - _xFactor);

            float targetRotateY = Random.Range(-_zFactor, _zFactor);

            Vector3 targetRotate = new Vector3(0, targetRotateY, targetRotateZ);

            _normalRotateTween = transform.DORotate(targetRotate, _normalRotateSpeed)
                .SetSpeedBased()
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    RotateRandom();
                });

        }

        void RandomColor()
        {
            _normalRandomColorTween = DOVirtual.Color(_colorList[_indexColor], _colorList[_indexColor + 1], 1f, (value) =>
            {
                _lightSource.color = value;
              
            }).OnComplete(()=> {
                  _indexColor++;
                  if(_indexColor>= _colorList.Count-1) _indexColor = 0;
                  RandomColor();

            });
        }

      



    }

}

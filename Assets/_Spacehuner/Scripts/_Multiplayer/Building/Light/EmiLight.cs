using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace SH.Multiplayer
{
    public class EmiLight : MonoBehaviour
    {
         public enum LightType
        {
            Color,
            RandomColor,
            MusicBeat
        }

        [SerializeField] private MusicManager _musicManager;
        [SerializeField] private MeshRenderer _bodyMesh;
        [SerializeField] private int _matPosition;
        private Material _selectedMat;

        [SerializeField] private float _timeChangeState = 5;

        [SerializeField] private LightType _lightType;

        [SerializeField] private float _intensityFactor = 1f ;
        [SerializeField] private List<Color> _colorList = new List<Color>();
        private int _currentColorIndex;

        private bool _isMusicBeat;

        // Start is called before the first frame update
        void Start()
        {
            Material matClone = new Material(_bodyMesh.materials[_matPosition]);
            _bodyMesh.materials[_matPosition] = matClone;
            _selectedMat = _bodyMesh.materials[_matPosition];

            //call one time;
            switch (_lightType)
            {
                case LightType.Color:
                    _currentColorIndex = 0;
                    ColorLight(_colorList[_currentColorIndex]);
                    break;
            }
        }



        // Update is called once per frame
        void Update()
        {
            //call every frame
            if(_lightType == LightType.MusicBeat && _musicManager != null) {
                _selectedMat.SetColor("_EmissionColor",_colorList[0] * _musicManager.OutDbFactor * _intensityFactor);
            }
        }

        private void ColorLight(Color color)
        {
                   _selectedMat.DOColor(color * _intensityFactor, "_EmissionColor", _timeChangeState)
                .SetEase(Ease.Linear)
                .SetSpeedBased()
                .OnComplete(async () =>
                    {
                       _currentColorIndex++;
                       if(_currentColorIndex >= _colorList.Count) _currentColorIndex = 0;
                        await Task.Delay(200);
                        ColorLight(_colorList[_currentColorIndex]); 
                    });
            

         
        }
    }

}



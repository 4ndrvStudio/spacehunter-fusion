using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace SH.Multiplayer
{
    public class EmiTexAnimation : MonoBehaviour
    {
        public enum AnimationType
        {
            TwoColor,
            RandomColor,
            Insentity
        }

        [SerializeField] private MeshRenderer _bodyMesh;
        [SerializeField] private int _matPosition;
        private Material _selectedMat;

        [SerializeField] private float _timeChangeState = 5;

        [SerializeField] private AnimationType _animationType;

        [SerializeField] private float _intensity = 1f ;
        [SerializeField] private List<Color> _colorList = new List<Color>();
        private int _currentColorIndex;

        // Start is called before the first frame update
         void Start()
        {
            Material matClone = new Material(_bodyMesh.materials[_matPosition]);
            _bodyMesh.materials[_matPosition] = matClone;
            _selectedMat = _bodyMesh.materials[_matPosition];


            switch (_animationType)
            {
                case AnimationType.TwoColor:
                    _currentColorIndex = 0;
                    ColorAnimation(_colorList[_currentColorIndex]);
                    break;
            }

        }


        private void ColorAnimation(Color color)
        {
            
            _selectedMat.DOColor(color * _intensity, "_EmissionColor", _timeChangeState)
                .SetEase(Ease.Linear)
                .SetSpeedBased()
                .OnComplete(async () =>
                    {
                       _currentColorIndex++;
                       if(_currentColorIndex >= _colorList.Count) _currentColorIndex = 0;
                        await Task.Delay(200);
                        ColorAnimation(_colorList[_currentColorIndex]); 
                    });
        }


    }

}


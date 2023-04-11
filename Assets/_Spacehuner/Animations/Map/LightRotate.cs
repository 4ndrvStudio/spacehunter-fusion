using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
namespace SH.Multiplayer
{
    public class LightRotate : MonoBehaviour
    {
        [SerializeField] private Transform _lightRight;
        [SerializeField] private Transform _lightLeft;

        private bool _leftDone;
        private bool _rightDone;

        [SerializeField] private float _limitZ = 40;
        [SerializeField] private float _limitY = 180;


        // Start is called before the first frame update
        void Start()
        {
            ExcuteRotate();
        }

        void LateUpdate() {
            if(_leftDone && _rightDone) {
                ExcuteRotate();
            }
        }

        private void ExcuteRotate()
        {
            _leftDone = false;
            _rightDone = false;

            float targetRotateZ = Random.Range(-_limitZ, _limitZ);

            float targetRotateY = Random.Range(-_limitY, _limitY);

            Vector3 targetRotate = new Vector3(0, targetRotateY,targetRotateZ );
            _lightRight.transform.DORotate(targetRotate,80f)
                .SetSpeedBased()
                .SetEase(Ease.Linear)
                .OnComplete(async()=> {
                    //await Task.Delay(2000);
                    _rightDone = true;
                });
             _lightLeft.transform.DORotate(targetRotate,80f)
                .SetSpeedBased()
                .SetEase(Ease.Linear)
                .OnComplete(async()=> {
                    //await Task.Delay();
                    _leftDone = true;
                });
        }



    }

}

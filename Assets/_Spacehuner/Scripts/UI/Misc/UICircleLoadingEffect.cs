using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SH.UI
{
    public class UICircleLoadingEffect : MonoBehaviour
    {
        private Transform transformToDoRotate;
        private Tweener _circleTweener;
        // Start is called before the first frame update
        void OnEnable()
        {
            transformToDoRotate = GetComponent<Transform>();
            _circleTweener =  transformToDoRotate.DORotate(new Vector3(0, 0, -360), 1.1f, RotateMode.FastBeyond360)
                                                    .SetLoops(-1)
                                                    .SetEase(Ease.Linear);
        }
        void OnDisable() {
            _circleTweener.Kill();
        }


    }

}

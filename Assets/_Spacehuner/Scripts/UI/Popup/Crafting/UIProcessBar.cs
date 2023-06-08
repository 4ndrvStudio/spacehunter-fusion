using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SH.UI
{
    public class UIProcessBar : MonoBehaviour
    {
        [SerializeField] private Image _statusImage;

        public void ProcessPercent(float percent) {
            _statusImage.fillAmount = percent;
        }
    }
}


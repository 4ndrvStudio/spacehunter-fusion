using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.UI
{
    public class UIMenuPanel_Setting : UIMenuPanel
    {
        [SerializeField] private GameObject _loaderIcon;
        [SerializeField] private GameObject Content;

        public override void Display()
        {
            Content.SetActive(true);

        }
        public override void Hide()
        {
            Content.SetActive(false);

        }
    }

}

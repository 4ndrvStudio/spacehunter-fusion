using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.UI
{
    using UnityEngine.UI;

    public class UICheckbox : MonoBehaviour
    {
        private Button _checkboxBtn;
        private GameObject _activeImage;

        // Start is called before the first frame update
        void Start()
        {
            _activeImage  = transform.GetChild(0).gameObject;
            _checkboxBtn =gameObject.GetComponent<Button>();
            
            _checkboxBtn.onClick.AddListener(() => {
                _activeImage.gameObject.SetActive(!_activeImage.activeSelf);
            });

        }

    }

}

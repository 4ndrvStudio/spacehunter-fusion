using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace SH.UI
{
    public class UIHiddenField : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Image _hiddenImg;
        [SerializeField] private Sprite _sprHideSprite;
        [SerializeField] private Sprite _sprActiveSprite;
        [SerializeField] private Button _toggleButton;
        [SerializeField] private TextMeshProUGUI _text;

        private void Start()
        {
            _toggleButton.onClick.AddListener(() => Show());
            _text.enableWordWrapping = true;
        }

        public void Show()
        {
            var contentType = _inputField.contentType;

            if (contentType == TMP_InputField.ContentType.Standard)
            {
                _inputField.contentType = TMP_InputField.ContentType.Password;
                _hiddenImg.sprite = _sprActiveSprite;
                _text.enableWordWrapping = true;
                _inputField.ForceLabelUpdate();
            }
            else if (contentType == TMP_InputField.ContentType.Password)
            {
                _inputField.contentType = TMP_InputField.ContentType.Standard;
                _hiddenImg.sprite = _sprHideSprite;
                _text.enableWordWrapping = true;
                _inputField.ForceLabelUpdate();
            }
        }
    }

}

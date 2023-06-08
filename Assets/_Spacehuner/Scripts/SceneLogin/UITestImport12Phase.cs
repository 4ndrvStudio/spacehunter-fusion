using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using TMPro;

namespace SH.UI
{
    public class UITestImport12Phase : MonoBehaviour
    {
        [SerializeField] private List<TMP_InputField> _inputsMnenonics = new List<TMP_InputField>();
        // Start is called before the first frame update
        void Start()
        {
            // _inputsMnenonics.ForEach(Input =>
            // {
            //     Input.onValueChanged.AddListener(OnInputValueChanged);
            // });
        }

        void LateUpdate() {
             _inputsMnenonics.ForEach(Input =>
            {
                string[] mnenonics = Input.text.Trim().Split(" ");
                if(mnenonics.Length == 12) {
                    OnInputValueChanged(mnenonics);
                };
            });
        }

        private void OnInputValueChanged(string[] mnenonics)
        {
            if (mnenonics.Length == 12)
            {
                for (int i = 0; i < _inputsMnenonics.Count; i++)
                {
                   if(Regex.IsMatch(mnenonics[i], @"^[a-zA-Z]+$")) {
                     _inputsMnenonics[i].text = mnenonics[i];
                   } else {
                    UIManager.Instance.ShowAlert("Check Your 12 Pass, some word not only letters! ",AlertType.Warning);
                   }

                   
                }
            }
        }
    }
}


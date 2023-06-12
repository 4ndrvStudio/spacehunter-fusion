using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;

namespace SH.UI
{
    public class UI12InputPhases : MonoBehaviour
    {
        [SerializeField] private List<TMP_InputField> _inputList = new List<TMP_InputField>();
        [SerializeField] private Button _createButton;
        public string Mnenonics = string.Empty;

        void LateUpdate()
        {
            _inputList.ForEach(Input =>
            {
                string[] mnenonics = Input.text.Trim().Split(" ");
                if (mnenonics.Length == 12)
                {
                    OnInputValueChanged(mnenonics);
                };
            });
        }

        public string Get12Phases()
        {
            string mnenonics = "";
            _inputList.ForEach(input => {
                Debug.Log(input.text.Trim());
                mnenonics+= " "  +input.text.Trim();
            });
            Mnenonics = mnenonics;
            return mnenonics;
        }

        

        private void OnInputValueChanged(string[] mnenonics)
        {
            if (mnenonics.Length == 12)
            {
                for (int i = 0; i < _inputList.Count; i++)
                {
                    if (Regex.IsMatch(mnenonics[i], @"^[a-zA-Z]+$"))
                    {
                        _inputList[i].text = mnenonics[i];
                    }
                    else
                    {
                        UIManager.Instance.ShowAlert("Check Your 12 Pass, some word not only letters! ", AlertType.Warning);
                    }
                }
            }
        }
    }

}

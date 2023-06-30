using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
namespace SH
{
    public class UIEstimatedGasFees : UIPopup
    {
        [SerializeField] private Button _rejectBtn;
        [SerializeField] private Button _confirmBtn;

        [SerializeField] private TextMeshProUGUI _gasText;


        private UnityAction _callback;
        private UnityAction _cancelCallback;
        private string _currentTx;
        private bool IsMinted;

        public override void ShowWithCallback(object customProperties, UnityAction callback = null, UnityAction cancelCallback =null)
        {
            base.ShowWithCallback(customProperties, callback,cancelCallback);

            SuiEstimatedGasFeesModel suiEstimatedGasFeesModel = customProperties as SuiEstimatedGasFeesModel;

            if (suiEstimatedGasFeesModel.CanExcute == true)
            {
                 float gas = float.Parse(suiEstimatedGasFeesModel.EstimatedGasFees) / 1000000000;
                string gasString =gas.ToString("0.#########");
                _gasText.text = gasString.Contains("-") ? gasString : "-" + gasString;

                _callback = callback;
                _currentTx = suiEstimatedGasFeesModel.Tx;
                IsMinted = suiEstimatedGasFeesModel.CanExcute;
                _cancelCallback = cancelCallback;
                
            }
            else
            {
                _gasText.text = "Cannot Estimated Gas";
                _callback = callback;
            }

        }

        // Start is called before the first frame update
        void Start()
        {
            _confirmBtn.onClick.AddListener(() => ConfirmClick());
            _rejectBtn.onClick.AddListener(() => CancelClick());
        }
        void CancelClick() {

            if(_cancelCallback != null) {
                _cancelCallback?.Invoke();
            }
            Hide();
        }
        void ConfirmClick()
        {
            if (_callback != null)
                if (IsMinted) _callback?.Invoke();

            Hide();
        }

    
    }
        public class SuiEstimatedGasFeesModel
        {
            public bool CanExcute;
            public string EstimatedGasFees;
            public string Tx;
        }



}

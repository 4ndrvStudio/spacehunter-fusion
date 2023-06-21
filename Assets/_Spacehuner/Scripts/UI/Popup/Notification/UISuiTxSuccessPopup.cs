using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace SH
{
    public class UISuiTxSuccessPopup : UIPopup
    {
        [SerializeField] private Button _checkExplorerButton;
        [SerializeField] private Button _confirmButton;

        [SerializeField] private TextMeshProUGUI _message;

        private string _objectID;

        public override void Show(object customProperties)
        {
            base.ShowWithCallback(customProperties);

            SuiTxSuccessModel suiTxSuccessModel = customProperties as SuiTxSuccessModel;
            _objectID = suiTxSuccessModel.ObjectID;
            _message.text = suiTxSuccessModel.Message;
        }

        // Start is called before the first frame update
        void Start()
        {
            _confirmButton.onClick.AddListener(() => Hide());
            _checkExplorerButton.onClick.AddListener(() => {
                string objectURL = $"https://suiexplorer.com/txblock/{_objectID}?network=testnet";
                Application.OpenURL(objectURL);
            });
        }

    }
    public class SuiTxSuccessModel
    {
        public string Message;
        public string ObjectID;
    }

}



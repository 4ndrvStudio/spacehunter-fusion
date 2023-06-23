using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine.Events;

namespace SH
{
    public class UISuiNotification : UIPopup
    {
        [Header("Panel & Button")]
        [SerializeField] private GameObject _completePanel;
        [SerializeField] private GameObject _failPanel;
        [SerializeField] private Button _confirmBtn;
        [SerializeField] private Button _suiExplorerButton;

        [Header("Field")]
        //complete
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _completeDescriptionText;
        [SerializeField] private Image _nftImage;

        //fail
        [SerializeField] private TextMeshProUGUI _failReasonText;

        private string _objectID;
        private UnityAction _callback;
        private bool IsMinted;
        public override async void ShowWithCallback(object customProperties, UnityAction callback = null)
        {
            base.ShowWithCallback(customProperties, callback);

            SuiNotificationModel suiNotificationModel = customProperties as SuiNotificationModel;
 
           
            if (suiNotificationModel.IsSuccess == true)
            {
                _completePanel.SetActive(true);
                _failPanel.SetActive(false);
                _objectID = suiNotificationModel.ObjectId;
                _nameText.text = suiNotificationModel.Name;
                _completeDescriptionText.text = suiNotificationModel.Description;
                _callback = callback;
                _nftImage.gameObject.SetActive(false);
                IsMinted = true;
                await LoadNFTImage(suiNotificationModel.ImageURL);
            }
            else
            {
                _completePanel.SetActive(false);
                _failPanel.SetActive(true);

            }
        }

        // Start is called before the first frame update
        void Start()
        {
            _suiExplorerButton.onClick.AddListener(() => TakeToSuiExplorer());
            _confirmBtn.onClick.AddListener(() => ConfirmClick());
        }
        void ConfirmClick()
        {
            if(_callback != null) 
                if(IsMinted) _callback?.Invoke();
           
            Hide();
        }

        void TakeToSuiExplorer()
        {
            string objectURL = $"https://suiexplorer.com/object/{_objectID}?network=testnet";
            Application.OpenURL(objectURL);
        }

        private async Task LoadNFTImage(string url)
        {
            Debug.Log("img url: " + url); 
            using var req = new UnityWebRequest(url, "GET");

            req.downloadHandler = new DownloadHandlerBuffer();
            req.SendWebRequest();
            while (!req.isDone)
            {
                await Task.Yield();
            }
            var data = req.downloadHandler.data;

            var tex = new Texture2D(256, 256);
            tex.LoadImage(data);
            var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            _nftImage.sprite = sprite;
            _nftImage.gameObject.SetActive(true);
        }


    }

    public class SuiNotificationModel
    {
        public bool IsSuccess;
        public string Name;
        public string Description;
        public string ObjectId;
        public string ImageURL;
        public string ErrorDescription;
    }

}

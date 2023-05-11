using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SH
{
    public class UIUpdateNotification : UIPopup
    {

        [SerializeField] private List<Button> _closeBtn = new List<Button>(2);
        [SerializeField] private Button _confirmBtn;

        // Start is called before the first frame update
        void Start()
        {
            _closeBtn.ForEach(closeBtn => {
                closeBtn.onClick.AddListener(()=> Hide());
            });
            _confirmBtn.onClick.AddListener(() => TakeToUpdate());
            

        }
        void TakeToUpdate() {
            #if UNITY_EDITOR 
                Hide();
            #elif UNITY_ANDROID 
                Debug.Log("Open play store!");
            #elif UNITY_IOS
                Debug.Log("Open appstore !");
                Application.OpenURL("https://testflight.apple.com/join/qaf3PlR2");
            #endif

        }

    
    }

}

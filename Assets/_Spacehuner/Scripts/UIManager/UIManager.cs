using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using SH.Networking.Chat;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance = null;
    public static UIManager Instance => _instance;

    [SerializeField] private Camera _uiCamera = null;

    [SerializeField] private UILoading _uiLoading = null;

    [SerializeField] private UIWaiting _uiWaiting = null;

    [SerializeField] private UITextAlert _uiTextAlert = null;

    [SerializeField] private UIGameInfomation _uiGameInfo = null;

    [SerializeField] private List<UIPopup> _lstPopup = null;

    [SerializeField] private ChatGameManager _chatManager = default;

    public Camera UICamera => _uiCamera;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;

        if (_instance != this)
            Destroy(this);

        DontDestroyOnLoad(this);
    }

    #region Popup
    public void ShowPopup(PopupName name, object customProperties = null)
    {
        var popup = _lstPopup.FirstOrDefault((popup) => popup.PopupName == name);
        if(popup == null)
        {
            Debug.LogError("Invalid popup name");
            return;
        }

        popup.Show(customProperties);
    }


    public void HidePopup(PopupName name)
    {
        var popup = _lstPopup.FirstOrDefault((popup) => popup.PopupName == name);
        if (popup == null)
        {
            Debug.LogError("Invalid popup name");
            return;
        }
        popup.Hide();
    }
    #endregion

    #region Waiting
    public async void ShowWaiting(int hideTime = 15, bool hasBackground = false)
    {
        _uiWaiting.Show(hasBackground);
        await Task.Delay(hideTime * 1000);
        _uiWaiting.Hide();
    }

    public void HideWaiting()
    {
        _uiWaiting.Hide();
    }
    #endregion



    #region Loading
    public async void LoadScene(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        _uiLoading.Show(0);
        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f);
            _uiLoading.Show((int)(progress * 100));
            await Task.Yield();
        }
        _uiLoading.Hide();
    }

    private IEnumerator LoadSceneAwait(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        _uiLoading.Show(0);
        while (!async.isDone)
        { 
            float progress = Mathf.Clamp01(async.progress / 0.9f);
            _uiLoading.Show((int)(progress * 100));
            yield return null;
        }
        _uiLoading.Hide();
    }
    #endregion

    #region Text alert
    public void ShowAlert(string msg, AlertType type)
    {
        _uiTextAlert.Show(msg, type);
    }
    #endregion

    #region Chat Manager
    public void ShowChat()
    {
        _chatManager.gameObject.SetActive(true);
    }

    public void HideChat()
    {
       _chatManager.gameObject.SetActive(false);
    }
    #endregion

    #region Game Infomation
    public void SetPing(long ping) => _uiGameInfo.SetPing(ping);
    #endregion
}

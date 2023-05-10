using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
public class UILoadSceneTest : MonoBehaviour
{
     [SerializeField] private GameObject _objContent = null;
    [SerializeField] private Transform _iconWaiting = null;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Color _overlayColor;
    [SerializeField] private Sprite _backgroundSprite;
    [SerializeField] private GameObject _reconnectText;

    [SerializeField] private bool IsShowed;

    public async void Show(bool isReconnecting)
    {
       
        if (IsShowed) return;

        if (!_objContent.activeInHierarchy)
        {
            IsShowed = true;
            _objContent.SetActive(true);
            if(isReconnecting) _reconnectText.SetActive(true);
            _iconWaiting.DORotate(new Vector3(0, 0, -360), 0.7f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        }
        await Task.Delay(40000);
        Hide();

    }

    public void Hide()
    {
        IsShowed = false;
        _objContent.SetActive(false);
        _reconnectText.SetActive(false);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIWaiting : MonoBehaviour
{
    [SerializeField] private GameObject _objContent = null;
    [SerializeField] private Transform _iconWaiting = null;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Color _overlayColor;
    [SerializeField] private Sprite _backgroundSprite;

    [SerializeField] private bool IsShowed;

    public void Show(bool hasBackground)
    {
        if (hasBackground == true)
        {
            _backgroundImage.sprite = _backgroundSprite;
            _backgroundImage.color = Color.white;
        }


        if (IsShowed) return;
        if (!_objContent.activeInHierarchy)
        {
            IsShowed = true;
            _objContent.SetActive(true);
           // _iconWaiting.DORotate(new Vector3(0, 0, -360), 0.7f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        }
    }

    public void Hide()
    {
        _objContent.SetActive(false);
        IsShowed = false;
        _backgroundImage.sprite = null;
        _backgroundImage.color = _overlayColor;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIWaiting : MonoBehaviour
{
    [SerializeField] private GameObject _objContent = null;
    [SerializeField] private Transform _iconWaiting = null;
    [SerializeField] private bool IsShowed;
    private void Start()
    {
    }

    public void Show()
    {
        if(IsShowed) return;
        if (!_objContent.activeInHierarchy)
        {
            IsShowed = true;
            _objContent.SetActive(true);
            _iconWaiting.DORotate(new Vector3(0, 0, -360), 0.7f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        }
    }

    public void Hide()
    {
        _objContent.SetActive(false);
        IsShowed = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using UnityEngine.UI;

public class AnimatedUIManager : MonoBehaviour
{
    public enum EaseType
    {
        ScaleX,
        ScaleY,
        Linear,
        InSine,
        OutSine,
        InOutSine,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        InBack,
        OutBack,
        InOutBack,
        InElastic,
        OutElastic,
        InOutElastic,
        InBounce,
        OutBounce,
        InOutBounce
    }
    public float fadeTime = 1f;
    [Header("Appear animation")]
    [SerializeField] private bool haveButton;
    [SerializeField] private bool havePanel;

    [Header("PANEL ANIMATE PROPERTIES")]
    public RectTransform panelObject;
    public EaseType easeType;
    public CanvasGroup canvasGroup;
    [SerializeField] private float localPosX, localPosY;
    [SerializeField] private float xOffset, yOffset;

    [Header("BUTTON ANIMATE PROPERTIES")]
    public List<Button> buttonObject = new List<Button>();
    [SerializeField] private float scaleAmount;
    private void OnEnable()
    {
        if (haveButton)
        {
            ButtonAppearAnimate();
        }
        if (havePanel)
        {
            PanelAppearAnimate();
        }
    }
    private void Start()
    {
        foreach (Button button in buttonObject)
        {
            button.onClick.AddListener(() => ClickButton(button));
        }
    }
    private void ClickButton(Button clickedButton)
    {
        for (int i = 0; i < buttonObject.Count; i++)
        {
            Button button = buttonObject[i];
            bool isButtonClicked = button == clickedButton;
            if (isButtonClicked)
            {
                button.transform.DOScale(new Vector3(0.8f, 0.8f, 1f), 0.05f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    button.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InBack);
                });
            }
        }
    }
    private void PanelAppearAnimate()
    {
        switch (easeType)
        {
            case EaseType.ScaleX:
                Vector3 initialScale = panelObject.localScale;
                panelObject.localScale = new Vector3(0f, initialScale.y, initialScale.z);
                Sequence scaleSequence = DOTween.Sequence();
                scaleSequence.Append(panelObject.DOScaleX(initialScale.x, fadeTime));
                scaleSequence.Play();
                break;
            case EaseType.ScaleY:
                Vector3 initialScaleY = panelObject.localScale;
                panelObject.localScale = new Vector3(initialScaleY.x, 0f , initialScaleY.z);
                Sequence scaleSequenceY = DOTween.Sequence();
                scaleSequenceY.Append(panelObject.DOScaleY(initialScaleY.y, fadeTime));
                scaleSequenceY.Play();
                break;
            case EaseType.Linear:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.Linear);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InSine:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InSine);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.OutSine:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.OutSine);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InOutSine:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InOutSine);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InQuad:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InQuad);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.OutQuad:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.OutQuad);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InQuart:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InQuart);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.OutQuart:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.OutQuart);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InOutQuart:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InOutQuart);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InCubic:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InCubic);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.OutCubic:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.OutCubic);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InOutCubic:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InOutCubic);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InQuint:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InQuint);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.OutQuint:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.OutQuint);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InExpo:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InExpo);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.OutExpo:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.OutExpo);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InOutExpo:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InOutExpo);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InCirc:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InCirc);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.OutCirc:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.OutCirc);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InOutCirc:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InOutCirc);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InBack:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InBack);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.OutBack:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.OutBack);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InOutBack:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InOutBack);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InElastic:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InElastic);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.OutElastic:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.OutElastic);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InOutElastic:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InOutElastic);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InBounce:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InBounce);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.OutBounce:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.OutBounce);
                canvasGroup.DOFade(1, fadeTime);
                break;
            case EaseType.InOutBounce:
                canvasGroup.alpha = 0;
                panelObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
                panelObject.DOAnchorPos(new Vector2(localPosX, localPosY), fadeTime, false).SetEase(Ease.InOutBounce);
                canvasGroup.DOFade(1, fadeTime);
                break;
            default:
                break;
        }
    }
    private void ButtonAppearAnimate()
    {
        for (int i = 0; i < buttonObject.Count; i++)
        {
            buttonObject[i].gameObject.SetActive(false); 

            int index = i; 
            StartCoroutine(ShowButtonWithScaleEffect(buttonObject[i], fadeTime, scaleAmount, index * 0.1f));
        }
    }
    IEnumerator ShowButtonWithScaleEffect(Button button, float duration, float scaleAmount, float delay)
    {
        yield return new WaitForSeconds(delay);
        button.gameObject.SetActive(true);
        button.transform.localScale = Vector3.zero;
        button.transform.DOScale(scaleAmount, duration).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(duration);
        button.transform.DOScale(1f, duration).SetEase(Ease.InBack);
    }
}

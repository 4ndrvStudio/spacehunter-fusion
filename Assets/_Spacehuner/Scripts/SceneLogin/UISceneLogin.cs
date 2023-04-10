using UnityEngine;
using SH.Account;
using SH.Define;
using TMPro;
using SH.PlayerData;
using SH.AzureFunction;

namespace SH
{
    public class UISceneLogin : MonoBehaviour
    {
        // [SerializeField] private GameObject _slotCharacterPanel = default;

        // [SerializeField] private GameObject _loginPanel = default;

        [SerializeField] private Canvas _canvas = default;

        [SerializeField] private TextMeshProUGUI _tmpVersion = default;

        private void Start()
        {
           // _canvas.worldCamera = UIManager.Instance.UICamera;
            _tmpVersion.SetText($"v{Application.version}");
        }
    }
}
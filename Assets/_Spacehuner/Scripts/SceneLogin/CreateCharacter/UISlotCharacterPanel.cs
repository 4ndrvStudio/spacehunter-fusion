using SH.PlayerData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Account
{
    public class UISlotCharacterPanel : MonoBehaviour
    {
        [SerializeField] private UISlotCharacter _slotCharacter1 = default;

        [SerializeField] private UISlotCharacter _slotCharacter2 = default;

        [SerializeField] private UISlotCharacter _slotCharacter3 = default;

        [SerializeField] private GameObject _loginPanel = default;

        private void OnEnable()
        {
            _slotCharacter1.Setup();
            _slotCharacter2.Setup();
            _slotCharacter3.Setup();
        }

        public void OnLogoutClick()
        {
            _loginPanel.SetActive(true);
            SHLocalData.Instance.RemoveLoginData();
            _loginPanel.GetComponent<CanvasGroup>().alpha = 1;
            gameObject.SetActive(false);
        }
    }

}

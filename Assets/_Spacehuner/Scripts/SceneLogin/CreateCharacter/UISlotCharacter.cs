using SH.Define;
using SH.PlayerData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using SH.Multiplayer;
namespace SH.Account
{
    public class UISlotCharacter : MonoBehaviour
    {
        [SerializeField] private int _index = default;

        [SerializeField] private int _price = default;

        [SerializeField] private int _levelUnlock = default;

        [SerializeField] private SlotCharacterState _state = default;

        [SerializeField] private UICreateCharacterPanel _createCharacterPanel = default;

        [Header("Lock")]

        [SerializeField] private GameObject _lock = default;

        [SerializeField] private TextMeshProUGUI _tmpSlotNumber = default;

        [SerializeField] private TextMeshProUGUI _tmpUnlockAt = default;

        [SerializeField] private TextMeshProUGUI _tmpPrice = default;

        [Header("Unlock")]

        [SerializeField] private GameObject _unlock = default;

        [SerializeField] private TextMeshProUGUI _tmpSlotNumberUnlock = default;

        [Header("Exist")]

        [SerializeField] private GameObject _hunter = default;

        [SerializeField] private TextMeshProUGUI _tmpSlotNumberExist = default;

        [SerializeField] private TextMeshProUGUI _tmpLevel = default;

        [SerializeField] private TextMeshProUGUI _tmpId = default;

        [SerializeField] private GameObject _bgId = default;

        [SerializeField] private Character _character = default;

        [SerializeField] private GameObject _slotCharacterPanel = default;

        [SerializeField] private UICharacter _uiCharacter = default;


        public void Setup()
        {
            _character = PlayerDataManager.Character.Data.Characters[_index];
            _state = _character.State;
            _tmpSlotNumber.SetText($"slot {_index + 1}");
            _tmpPrice.SetText($"{_price} SCE");
            _tmpUnlockAt.SetText($"Unlock at level {_levelUnlock}");
            _tmpSlotNumberExist.SetText($"slot {_index + 1}");
            _tmpSlotNumberUnlock.SetText($"slot {_index + 1}");
            _tmpLevel.SetText($"Level {_character.Level}");

            if(_state == SlotCharacterState.Unlock)
            {
                if(_character.CharacterType == 0)
                {
                    HandleUnlockState();
                }
                else
                {
                    HandleFilledExistState();
                }
            }
            else
            {
                HandleLockState();
            }
        }

        private void HandleLockState()
        {
            _lock.SetActive(true);
            _unlock.SetActive(false);
            _hunter.SetActive(false);
        }

        private void HandleUnlockState()
        {
            _lock.SetActive(false);
            _unlock.SetActive(true);
            _hunter.SetActive(false);
        }

        private void HandleFilledExistState()
        {
            _lock.SetActive(false);
            _unlock.SetActive(false);
            _hunter.SetActive(true);
            _bgId.SetActive(_character.Id != "0");
            _uiCharacter.Setup(_character.CharacterType);
        }

        #region Lock

        #endregion

        #region Unlock
        public void OnCreateCharacterClick()
        {
            _createCharacterPanel.gameObject.SetActive(true);
            _createCharacterPanel.Setup(_index);
            _slotCharacterPanel.SetActive(false);
        }

        public void OnImportCharacterClick()
        {
            UIManager.Instance.ShowAlert("Please visit our website to import hunters!", AlertType.Normal);
        }

        public async void OnPlayCharacterClick()
        {
            PlayerDataManager.Character.Data.CharacterInUse = _character;
            //UIManager.Instance.LoadScene(SceneName.SceneStation);
            Network_ClientManager.StartGame(SceneDefs.scene_stationFusion);
            
        }
        #endregion
    }
}

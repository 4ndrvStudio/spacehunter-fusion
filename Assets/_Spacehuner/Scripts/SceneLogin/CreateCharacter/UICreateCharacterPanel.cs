using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SH.Define;
using SH.PlayerData;
using SH.AzureFunction;

namespace SH.Account
{
    public class UICreateCharacterPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmpSex = default;

        [SerializeField] private TextMeshProUGUI _tmpName = default;

        [SerializeField] private List<UICharacterSelect> _lstCharacterType = default;

        [SerializeField] private List<Image> _lstBtnRace = default;

        [SerializeField] private UICharacterSelect _characterSelected = default;

        [SerializeField] private int _slotIndex = default;

        [SerializeField] private GameObject _slotCharacterPanel = default;

        private void OnEnable()
        {
            OnAllClick();
        }

        public void Setup(int slotIndex)
        {
            _slotIndex = slotIndex;
        }

        public void OnBackClick()
        {
            gameObject.SetActive(false);
            _slotCharacterPanel.SetActive(true);
        }

        public void OnCreateClick()
        {
            if (_characterSelected == null)
                return;
            PlayerDataManager.CallFunction<CreateCharacterRespone>(new CreateCharacterRequest((int)_characterSelected.CharacterType, _slotIndex), (resp) =>
            {
                if (string.IsNullOrEmpty(resp.Error))
                {
                    Debug.Log("Create character success!");

                    PlayerDataManager.CallFunction<GetUserDataRespone>(new GetUserDataRequest(), (resp) =>
                    {
                        if (string.IsNullOrEmpty(resp.Error))
                        {
                            PlayerDataManager.Instance.Setup(resp);
                            gameObject.SetActive(false);
                            _slotCharacterPanel.SetActive(true);
                            //UIManager.Instance.LoadScene(SceneName.SceneStation);
                        }
                        else
                        {
                            Debug.LogError(resp.Error);
                            UIManager.Instance.ShowAlert(resp.Error, AlertType.Error);
                        }
                    });
                }
                else
                {
                    Debug.LogError(resp.Error);
                    UIManager.Instance.ShowAlert(resp.Error, AlertType.Error);
                }
            });
        }

        public void SetCharacterSelect(UICharacterSelect selected)
        {
            _characterSelected = selected;
        }

        public void OnAllClick()
        {
            _lstCharacterType.ForEach(item => item.gameObject.SetActive(true));
            _lstBtnRace.ForEach(item => item.color = Color.white);
            _lstCharacterType[0].OnItemClick();
        }

        public void OnVasinClick()
        {
            _lstCharacterType.ForEach(item =>
            {
                if (item.CharacterType == CharacterType.VasinMale || item.CharacterType == CharacterType.VasinFemale)
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
            });

            _lstBtnRace.ForEach(item => item.color = Color.white);
            _lstBtnRace[0].color = Color.red;
        }

        public void OnHumesClick()
        {
            _lstCharacterType.ForEach(item =>
            {
                if (item.CharacterType == CharacterType.HumesMale || item.CharacterType == CharacterType.HumesFemale)
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
            });
            _lstBtnRace.ForEach(item => item.color = Color.white);
            _lstBtnRace[1].color = Color.yellow;
        }

        public void OnDisryClick()
        {
            _lstCharacterType.ForEach(item =>
            {
                if (item.CharacterType == CharacterType.DisryMale || item.CharacterType == CharacterType.DisryFemale)
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
            });
            _lstBtnRace.ForEach(item => item.color = Color.white);
            _lstBtnRace[2].color = new Color(0, 0.9294118f, 0.9607843f, 1);
        }

        public void OnMutasClick()
        {
            _lstCharacterType.ForEach(item =>
            {
                if (item.CharacterType == CharacterType.MutasMale || item.CharacterType == CharacterType.MutasFemale)
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
            });
            _lstBtnRace.ForEach(item => item.color = Color.white);
            _lstBtnRace[3].color = Color.green;
        }

        public void OnMabitClick()
        {
            _lstCharacterType.ForEach(item =>
            {
                if (item.CharacterType == CharacterType.MabitMale || item.CharacterType == CharacterType.MabitFemale)
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
            });
            _lstBtnRace.ForEach(item => item.color = Color.white);
            _lstBtnRace[4].color = Color.red;
        }

        public void SetSex(string sex)
        {
            _tmpSex.SetText(sex);
        }

        public void SetName(string name)
        {
            _tmpName.SetText(name);
        }
    }
}
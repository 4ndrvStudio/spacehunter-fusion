using SH.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Account
{
    public class UICharacterSelect : MonoBehaviour
    {
        [SerializeField] private CharacterType _characterType = default;

        [SerializeField] private bool _isMale = default;

        [SerializeField] private UICreateCharacterPanel _controller = default;

        [SerializeField] private UICharacter _uiCharacter = default;


        public CharacterType CharacterType => _characterType;

        public void OnItemClick()
        {
            _controller.SetSex(_isMale ? "Male" : "Female");
            _controller.SetName(_characterType.ToString());
            _controller.SetCharacterSelect(this);
            _uiCharacter.Setup(_characterType);
        }
    }
}
using SH.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH
{
    public class UICharacter : MonoBehaviour
    {
        [SerializeField] private CharacterType _characterType = CharacterType.DisryFemale;
        [SerializeField] private Vector3 _pos = default;
        [SerializeField] private Vector3 _rot = default;
        [SerializeField] private Vector3 _scale = default;
        private GameObject _character = default;

        public void Setup()
        {
            if (_character != null)
                DestroyImmediate(_character);
            var character =  Resources.Load<GameObject>($"Characters/{_characterType}");
            _character = Instantiate(character, transform);
            _character.transform.localPosition = _pos;
            _character.transform.localRotation = new Quaternion(_rot.x, _rot.y, _rot.z, 0);
            _character.transform.localScale = _scale;
        }

        public void Setup(CharacterType characterType)
        {
            if (_character != null)
                DestroyImmediate(_character);
            _characterType = characterType;
            var character = Resources.Load<GameObject>($"Characters/{_characterType}");
            _character = Instantiate(character, transform);
            _character.transform.localPosition = _pos;
            _character.transform.localRotation = new Quaternion(_rot.x, _rot.y, _rot.z, 0);
            _character.transform.localScale = _scale;
        }
    }
}

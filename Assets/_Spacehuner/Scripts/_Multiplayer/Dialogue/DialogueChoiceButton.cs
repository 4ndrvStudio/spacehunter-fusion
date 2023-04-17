using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SH.Dialogue
{
    public class DialogueChoiceButton : MonoBehaviour
    {

        [SerializeField] private Button _choiceBtn;
        [SerializeField] private TextMeshProUGUI _choiceTextContent;
        
        public int Index;

        // Start is called before the first frame update
        void Start()
        {
            _choiceBtn.onClick.AddListener(() => DialogueManager.Instance.MakeChoice(Index));
        }


        public void SetContent(string content) => _choiceTextContent.text = content;

    }

}

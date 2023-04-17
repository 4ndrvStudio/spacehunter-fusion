using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SH.Dialogue
{
    public class DialogueStartChatButton : MonoBehaviour
    {
        [SerializeField] private Button _startChatBtn;
        [SerializeField] private TextMeshProUGUI _textContent;
        public string Name;
        [SerializeField] private TextAsset _dialogueContent;



        public void SetButtonContent(string name, TextAsset dialogueContent) { 
            Name = name;
            _textContent.text = "Chat to " +  name;
            _dialogueContent =dialogueContent;
            _startChatBtn.onClick.AddListener(() => {
                DialogueManager.Instance.EnterDialogueMode(_dialogueContent);
                
            }); 
        }

        
    }

}

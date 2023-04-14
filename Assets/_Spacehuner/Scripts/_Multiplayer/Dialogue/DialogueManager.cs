using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
namespace SH
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;

        [SerializeField] private GameObject _dialoguePanel;
        [SerializeField] private TextMeshProUGUI _dialogueText;

        private Story _currentStory;

        private bool _dialogueIsPlaying;

        [SerializeField] private GameObject[] _choices;
        private TextMeshProUGUI[] _choicesText;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        void Start()
        {
            _dialogueIsPlaying = false;
            _dialoguePanel.SetActive(false);

            _choicesText = new TextMeshProUGUI[_choices.Length];
            int index = 0;
            foreach (GameObject choice in _choices) {
                _choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
                index ++ ;
            }

        }
        // Update is called once per frame
        void Update()
        {
            if (_dialogueIsPlaying == false) return;

            if(Input.GetKeyDown(KeyCode.T)) ContinueStory();

        }

        public void EnterDialogueMode(TextAsset inkJson)
        {
            _currentStory = new Story(inkJson.text);
            _dialogueIsPlaying = true;
            _dialoguePanel.SetActive(true);

            ContinueStory();

        }


        private void ExitDialogueMode()
        {
            _dialogueIsPlaying = false;
            _dialoguePanel.SetActive(false);
            _dialogueText.text = "";
        }

        private void ContinueStory()
        {
            if (_currentStory.canContinue)
            {
                _dialogueText.text = _currentStory.Continue();
                
                if(_currentStory.currentChoices.Count > 0) {
                    DisplayChoice();
                    Debug.Log(_currentStory.currentChoices[0].text);
                } else {
                    HideChoice();
                }
               
            }
            else
            {
                ExitDialogueMode();
            }
        }

        private void DisplayChoice() {
            List<Choice> currentChoices = _currentStory.currentChoices;
            int index = 0;
            foreach(Choice choice in currentChoices) {
                _choices[index].gameObject.SetActive(true);
                _choicesText[index].text = choice.text;
                index ++;
            }

        } 
        private void HideChoice() {
             List<Choice> currentChoices = _currentStory.currentChoices;
            int index = 0;
            foreach(Choice choice in currentChoices) {
                _choices[index].gameObject.SetActive(false);
                index ++;
            }
        }

        public void MakeChoice(int index) {
            _currentStory.ChooseChoiceIndex(index);
            ContinueStory();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using SH.Multiplayer;

namespace SH.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;

        [SerializeField] private GameObject _dialoguePanel;
        [SerializeField] private GameObject _dialogueChoicePanel;
        [SerializeField] private GameObject _choiceBtnOb;
        [SerializeField] private GameObject _dialogueStartChatPanel;
        [SerializeField] private GameObject _startChatBtnOb;
       
        [SerializeField] private TextMeshProUGUI _dialogueText;

        [SerializeField] private List<Choice> _currentChoicesList = new List<Choice>();
        [SerializeField] private List<Button> _continueBtnList = new List<Button>();

        private List<GameObject> _currentChoicesBtnList = new List<GameObject>();
        private List<GameObject> _startChatBtnList = new List<GameObject>();


        private Story _currentStory;

        public static bool DialogueIsPlaying;

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
            DialogueIsPlaying = false;
            _dialoguePanel.SetActive(false);
            _dialogueChoicePanel.SetActive(false);

            _continueBtnList.ForEach(continueBtn =>
            {
                continueBtn.onClick.AddListener(() => ContinueStory());
            });

        }
   

        public void EnterDialogueMode(TextAsset inkJson)
        {
            if (DialogueIsPlaying == true) return;

            _currentStory = new Story(inkJson.text);
            DialogueIsPlaying = true;

            _dialoguePanel.SetActive(true);

            //UIControllerManager.Instance.ActiveController(false);
            UIControllerManager.Instance.HideAllController();

            RemoveAllStartChatBtn();

            ContinueStory();
        }


        private void ExitDialogueMode()
        {
            DialogueIsPlaying = false;
            _dialoguePanel.SetActive(false);
            _dialogueText.text = "";
            //UIControllerManager.Instance.ActiveController(true);
            UIControllerManager.Instance.DisplayController();
        }

        private void ContinueStory()
        {
            if (_currentStory.canContinue)
            {
                _dialogueText.text = _currentStory.Continue();

                if (_currentStory.currentChoices.Count > 0)
                {

                    DisplayChoice();

                }
                else
                {

                    HideChoice();
                }

            }
            else
            {
                if (_currentStory.currentChoices.Count <= 0)
                    ExitDialogueMode();
            }
        }

        private void DisplayChoice()
        {

            _currentChoicesList.Clear();


            _currentChoicesBtnList.ForEach(choiceBtn =>
            {
                Destroy(choiceBtn);
            });

            _currentChoicesBtnList.Clear();

            _dialogueChoicePanel.SetActive(true);

            _currentChoicesList = _currentStory.currentChoices;

            int index = 0;

            foreach (Choice choice in _currentChoicesList)
            {

                GameObject choiceBtn = Instantiate(_choiceBtnOb, _dialogueChoicePanel.transform);

                choiceBtn.gameObject.SetActive(true);

                DialogueChoiceButton choiceButtonScript = choiceBtn.GetComponent<DialogueChoiceButton>();

                choiceButtonScript.Index = index;

                choiceButtonScript.SetContent(choice.text);

                _currentChoicesBtnList.Add(choiceBtn);

                index++;
            }

        }
        private void HideChoice() => _dialogueChoicePanel.SetActive(false);

        public void AddStartChatBtn(string name, TextAsset content)
        {
          

            if(_startChatBtnList.FindIndex(btn => btn.GetComponent<DialogueStartChatButton>().Name  == name) != -1)
                return;

            GameObject startChatBtn = Instantiate(_startChatBtnOb,_dialogueStartChatPanel.transform);
            
            startChatBtn.GetComponent<DialogueStartChatButton>().SetButtonContent(name,content);

            _startChatBtnList.Add(startChatBtn);
            

            

        }

        public void RemoveAllStartChatBtn() {
            _startChatBtnList.ForEach(startChatBtn => {
                Destroy(startChatBtn);
            });
            _startChatBtnList.Clear();
        }

        public void RemoveStartChatBtn(string name)
        {

            GameObject targetBtn = _startChatBtnList.Find(startChatBtn =>
                 startChatBtn.GetComponent<DialogueStartChatButton>().Name == name);
           
            if(targetBtn == null) return;
            
            _startChatBtnList.Remove(targetBtn);
            
            Destroy(targetBtn);

        }

        public void MakeChoice(int index)
        {
            _currentStory.ChooseChoiceIndex(index);

            ContinueStory();
        }
    }

}

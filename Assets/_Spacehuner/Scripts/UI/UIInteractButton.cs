using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SH.Dialogue;

namespace SH.Multiplayer
{
    public enum InteractButtonType {
        Chat,
        Building
    }

    public class InteractButtonCustomProperties {
        //key:
        public static string Name = "Name";
        public static string ChatContent = "ChatContent";
        public static string BuildingInteractType = "BuildingInteractType";
        
    }

    public class UIInteractButton : MonoBehaviour
    {   

        [SerializeField] private Button _btn;
        [SerializeField] private InteractButtonType _interactButtonType;
        [SerializeField] private TextMeshProUGUI _btnTextContent;
        
        //Chat Content
        [HideInInspector] public string NPCName;
        [HideInInspector] public TextAsset ChatContent;

        //Building Content;
        [HideInInspector] public BuildingName BuildingName;
        [HideInInspector] public BuildingInteractType BuildingInteractType;

        public bool IsSet = false;
        public int Id;



        public void SetContentOfButton(InteractButtonType interactButtonType, Dictionary<string, object> customProperties) {
            
          
            switch(interactButtonType) {
                case InteractButtonType.Chat: SetContentForChat(customProperties);
                    break;
                case InteractButtonType.Building: SetContentForBuilding(customProperties);
                    break;
            }

            IsSet = true;
            
            this.gameObject.SetActive(true);

        }

        private void SetContentForChat(Dictionary<string, object> customProperties) 
        {

            NPCName = (string) customProperties[InteractButtonCustomProperties.Name];
            ChatContent = (TextAsset) customProperties[InteractButtonCustomProperties.ChatContent];

            _btnTextContent.text = $"Chat to {NPCName}";
            _btn.onClick.RemoveAllListeners();
            _btn.onClick.AddListener(() => {
                
                DialogueManager.Instance.EnterDialogueMode(ChatContent);
                DisableBtn();
            });

        }

        private void SetContentForBuilding( Dictionary<string, object> customProperties)
        {
            BuildingName = (BuildingName) customProperties[InteractButtonCustomProperties.Name];
            
            BuildingInteractType = (BuildingInteractType) customProperties[InteractButtonCustomProperties.BuildingInteractType];

            
            _btnTextContent.text = $"{BuildingInteractType.ToString()} to {BuildingName.ToString()}";
            
            _btn.onClick.RemoveAllListeners();
            
            _btn.onClick.AddListener(() => {
                switch(BuildingInteractType) {
                        case BuildingInteractType.Enter: BuildingManager.Instance.EnterBuilding(BuildingName);
                  
                            break;
                        case BuildingInteractType.Exit : BuildingManager.Instance.ExitBuilding(BuildingName);
                       
                            break;
                }
                DisableBtn();
            });


        }

        public void DisableBtn() {
            IsSet = false;
            Id = 0;
            this.gameObject.SetActive(false);
        }



        
    }

}

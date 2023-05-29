using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Settings
{
    [SerializeField]
    [CreateAssetMenu(fileName = "GlobalsSettings", menuName = "Space Hunter /Global Settings")]
    public class GlobalSettings : ScriptableObject
    {
        public string LoadingScene = "scene_loading";
        public string LoginScene = "scene_login";
        
        
       
    }

}

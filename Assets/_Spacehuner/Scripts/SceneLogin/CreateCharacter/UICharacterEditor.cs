using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace SH
{
    [CustomEditor(typeof(UICharacter))]
    public class UICharacterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            UICharacter uiCharacter = (UICharacter)target;
            if(GUILayout.Button("Setup"))
            {
                uiCharacter.Setup();
            }
        }
    }
}
#endif
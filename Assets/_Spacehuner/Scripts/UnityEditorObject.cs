using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH
{
    public class UnityEditorObject : MonoBehaviour
    {
        private void Start()
        {
#if !UNITY_EDITOR
          gameObject.SetActive(false);
#endif
        }
    }
}

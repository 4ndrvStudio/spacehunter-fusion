using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.UI
{
    public class UIMenuPanel : MonoBehaviour
    {
        public UIMenuTabName TabName;

        public virtual void Display() { }

        public virtual void Hide() { }
    }

}

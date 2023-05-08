using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

namespace SH.Multiplayer
{
    public class Region : NetworkBehaviour
    {
        public TextMeshProUGUI _regionText;

        public override void FixedUpdateNetwork()
        {   
            _regionText.text = Runner.SessionInfo.Region;
        }
    }

}

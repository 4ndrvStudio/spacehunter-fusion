using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_PlayerWeaponHandler : NetworkBehaviour
    {
        
        
        [Networked(OnChanged = nameof(OnWeaponInUseIdChanged))]
        [HideInInspector] public NetworkString<_16> WeaponInUseId { get; set; }
        
        
        

        static void OnWeaponInUseIdChanged(Changed<Network_PlayerWeaponHandler> changed)
        {
            changed.Behaviour.OnWeaponInUseIdChanged();
        }

        private void OnWeaponInUseIdChanged()
        {

        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SH.Multiplayer;

namespace SH
{
    public class MusicBuilding : Building
    {
        [SerializeField] private MusicManager _musicManager;

        public override void Enter()
        {
            base.Enter();
            _musicManager.SetUpMusic();
        }
        public override void Exit()
        {
            base.Exit();
            _musicManager.TurnOffMusic();
        }
    }

}

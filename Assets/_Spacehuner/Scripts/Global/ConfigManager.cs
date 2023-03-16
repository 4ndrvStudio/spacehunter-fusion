using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH
{
    public class ConfigManager : MonoBehaviour
    {
        public static ConfigManager Instance = null;

        [SerializeField] private MineralConfig _mineralConfig = null;
        public MineralConfig MineralConfig => _mineralConfig;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
    }
}

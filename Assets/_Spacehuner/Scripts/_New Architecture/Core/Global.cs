using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using SH.Settings;
using SH.Utilities;

namespace SH.Core
{
    public interface IGlobalService
    {
        void Initialize();
        void Tick();
        void Deinitialize();
    }

    public static class Global
    {
        // Public 
        public static GlobalSettings Settings { get; private set; }

        // Private 
        private static bool _isInitialized;
        private static List<IGlobalService> _globalServices = new List<IGlobalService>();

        public static void Quit()
        {
            Deinitialize();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        	Application.Quit();
#endif
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InitializeSubSystem()
        {
            if (Application.isBatchMode == true)
            {
                UnityEngine.AudioListener.volume = 0.0f;
            }

#if UNITY_EDITOR
            if (Application.isPlaying == false)
                return;
#endif
            if (PlayerLoopUtility.HasPlayerLoopSystem(typeof(Global)) == false)
            {
                PlayerLoopUtility.AddPlayerLoopSystem(typeof(Global), typeof(Update.ScriptRunBehaviourUpdate), BeforeUpdate, AfterUpdate);
            }

            Application.quitting -= OnApplicationQuit;
            Application.quitting += OnApplicationQuit;

            _isInitialized = true;

            Application.quitting -= OnApplicationQuit;
            Application.quitting += OnApplicationQuit;

            _isInitialized = true;

        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeBeforeSceneLoad()
        {
            Initialize();

        }

        private static void OnApplicationQuit()
        {
            Deinitialize();
        }


        private static void Initialize()
        {
            if (_isInitialized == false)
                return;

            GlobalSettings[] globalSettings = Resources.LoadAll<GlobalSettings>("");
            Settings = globalSettings.Length > 0 ? Object.Instantiate(globalSettings[0]) : null;

            PrepareGlobalServices();

            _isInitialized = true;
        }

        private static void Deinitialize()
        {
            if (_isInitialized == false)
                return;
            for (int i = _globalServices.Count - 1; i >= 0; i--)
            {
                var service = _globalServices[i];
                if (service != null)
                {
                    service.Deinitialize();
                }
            }

            _isInitialized = false;
        }

        private static void PrepareGlobalServices()
        {
            // PlayerService = new PlayerService();

            // _globalServices.Add(PlayerService);

            for (int i = 0; i < _globalServices.Count; i++)
            {
                _globalServices[i].Initialize();
            }
        }

        private static void BeforeUpdate()
        {
            for (int i = 0; i < _globalServices.Count; i++)
            {
                _globalServices[i].Tick();
            }
        }

        private static void AfterUpdate()
        {
            if (Application.isPlaying == false)
            {
                PlayerLoopUtility.RemovePlayerLoopSystems(typeof(Global));
            }
        }


    }

}


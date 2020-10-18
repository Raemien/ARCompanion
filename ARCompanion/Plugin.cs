using BeatSaberMarkupLanguage.MenuButtons;
using BeatSaberMarkupLanguage.Settings;
using BS_Utils.Utilities;
using IPA;
using IPA.Config;
using IPA.Utilities;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using IPALogger = IPA.Logging.Logger;

namespace ARCompanion
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class ARCompanion
    {
        public const string assemblyName = "ARCompanion";

        public static XRCameraBehaviour xrcamBehaviour;
        public static OpenVRCameraManager openvrCameraManager;

        [Init]
        public ARCompanion(IPALogger logger, [IPA.Config.Config.Prefer("json")] IConfigProvider cfgProvider)
        {
            Logger.log = logger;
        }


        [OnEnable]
        public void Start()
        {
            PersistentSingleton<Settings>.TouchInstance();
            PersistentSingleton<UISingleton>.TouchInstance();
            UISingleton.RegMenuButton();

            BSMLSettings.instance.AddSettingsMenu("ARCompanion", "ARCompanion.Views.SettingsMenu.bsml", Settings.instance);

            SceneManager.sceneLoaded += OnSceneLoaded;
            InitBackground();

        }

        [OnDisable]
        public void Disable()
        {
            BSMLSettings.instance.RemoveSettingsMenu(Settings.instance);
            UISingleton.RemoveMenuButton();
        }

        private void OnSceneLoaded(Scene newScene, LoadSceneMode sceneMode)
        {
            var config = Settings.instance;
            EnvironmentHider.hiddenObjs = 0;
            InitBackground();
            xrcamBehaviour.hmdRef = null;
            if (config.SelectedWebcam != "SteamVR")
            {
                xrcamBehaviour.webcamTexture.Stop();
            }
            switch (newScene.name)
            {
                case "MenuViewControllers":
                case "GameCore":
                case "Credits":
                case "HealthWarning":
                    if (config.EnableBackgrounds)
                    {
                        xrcamBehaviour.InitCameraPlane();
                    }
                    break;
            }

        }

        private void InitBackground()
        {
            string curscene = SceneManager.GetActiveScene().name;
            var config = Settings.instance;
            if (xrcamBehaviour == null)
            {
                xrcamBehaviour = new GameObject(nameof(XRCameraBehaviour)).AddComponent<XRCameraBehaviour>();
                UnityEngine.Object.DontDestroyOnLoad(xrcamBehaviour);
            }
            if (openvrCameraManager == null && XRSettings.loadedDeviceName == "OpenVR")
            {
                openvrCameraManager = xrcamBehaviour.gameObject.AddComponent<OpenVRCameraManager>();
                openvrCameraManager.enabled = false;
                UnityEngine.Object.DontDestroyOnLoad(openvrCameraManager);
            }
            xrcamBehaviour.enabled = config.EnableBackgrounds;
        }
    }
}
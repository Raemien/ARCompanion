using BeatSaberMarkupLanguage.Attributes;
using BS_Utils.Utilities;
using UnityEngine;

namespace ARCompanion
{
    internal class PluginConfig
    {
        public bool RegenerateConfig = true;
    }


    class Settings : PersistentSingleton<Settings>
    {
        private Config config;

        [UIAction("trigger-togglebackgrounds")]
        public void ToggleBackgrounds(bool val)
        {
            ARCompanion.InitBackground();
            GameObject camObject = ARCompanion.xrcamBehaviour.planeObject;
            if (val == true)
            {
                ARCompanion.xrcamBehaviour.InitCameraPlane("None");
                EnvironmentHider.HideEnvironmentObjects(true, true);
            }
            else
            {
                ARCompanion.xrcamBehaviour.InitCameraPlane(SelectedWebcam);
            }
            camObject.SetActive(val);
        }
        [UIAction("hideobj-refresh")] private void RefreshHiddenObjects() => EnvironmentHider.HideEnvironmentObjects(true);

        //General
        [UIValue("enabled")]
        public bool EnableBackgrounds
        {
            get => config.GetBool("General", "Enabled", true);
            set => config.SetBool("General", "Enabled", value);
        }

        //Camera
        public string SelectedWebcam
        {
            get => config.GetString("Camera", "Selected Webcam", "Auto");
            set => config.SetString("Camera", "Selected Webcam", value);
        }
        public bool UndistortRawFeed
        {
            get => config.GetBool("Camera", "Undistort HMD Camera", true);
            set => config.SetBool("Camera", "Undistort HMD Camera", value);
        }
        public float ProjectionScale
        {
            get => config.GetFloat("Camera", "Projection Scale", 10);
            set => config.SetFloat("Camera", "Projection Scale", value);
        }
        public float ProjectionDistance
        {
            get => config.GetFloat("Camera", "Projection Distance", 50);
            set => config.SetFloat("Camera", "Projection Distance", value);
        }
        public float ProjectionXOffset
        {
            get => config.GetFloat("Camera", "Position Offset X", 0);
            set => config.SetFloat("Camera", "Position Offset X", value);
        }
        public float ProjectionYOffset
        {
            get => config.GetFloat("Camera", "Position Offset Y", 0);
            set => config.SetFloat("Camera", "Position Offset Y", value);
        }


        //Hidden Objects
        [UIValue("hideobj-platform")]
        public bool HidePlatform
        {
            get => config.GetBool("Hidden Objects", "Hide Platform", true);
            set => config.SetBool("Hidden Objects", "Hide Platform", value);
        }
        [UIValue("hideobj-fog")]
        public bool HideFog
        {
            get => config.GetBool("Hidden Objects", "Hide Smoke", true);
            set => config.SetBool("Hidden Objects", "Hide Smoke", value);
        }
        public bool HideMenuEnv
        {
            get => config.GetBool("Hidden Objects", "Hide Menu Decorations", true);
            set => config.SetBool("Hidden Objects", "Hide Menu Decorations", value);
        }
        public bool HideGameEnv
        {
            get => config.GetBool("Hidden Objects", "Hide GameCore Decorations", true);
            set => config.SetBool("Hidden Objects", "Hide GameCore Decorations", value);
        }

        public bool RegenerateConfig { get; internal set; }

        public void Awake()
        {
            config = new Config("ARCompanion");
        }
    }

}
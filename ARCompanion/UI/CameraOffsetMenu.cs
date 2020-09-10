using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ARCompanion
{
    class CameraOffsetMenu : BSMLResourceViewController
    {
        public override string ResourceName => "ARCompanion.Views.CameraOffsetMenu.bsml";

        //Initial Values
        private string _DefaultPreset = "";
        private float _ProjectionScale = Settings.instance.ProjectionScale;
        private float _ProjectionDist = Settings.instance.ProjectionDistance;
        private float _ProjectionX = Settings.instance.ProjectionXOffset;
        private float _ProjectionY = Settings.instance.ProjectionYOffset;

        //Values
        [UIValue("projection-scale")]
        private float ProjectionScale
        {
            get => _ProjectionScale;
            set
            {
                _ProjectionScale = value;
                Settings.instance.ProjectionScale = ProjectionScale;
                NotifyPropertyChanged();
            }
        }

        [UIValue("projection-dist")]
        private float ProjectionDistance
        {
            get => _ProjectionDist;
            set
            {
                _ProjectionDist = value;
                Settings.instance.ProjectionDistance = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("projection-offset-x")]
        private float ProjectionX
        {
            get => _ProjectionX;
            set
            {
                _ProjectionX = value;
                Settings.instance.ProjectionXOffset = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("projection-offset-y")]
        private float ProjectionY
        {
            get => _ProjectionY;
            set
            {
                _ProjectionY = value;
                Settings.instance.ProjectionYOffset = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("default-preset")]
        private string DefaultPreset 
        {
            get => _DefaultPreset;
            set
            {
                _DefaultPreset = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("preset-options")]
        private List<object> HardwarePresets = new object[] { "Generic", "Vive Wands (HTC Vive)", "Knuckles (HTC Vive)", "Knuckles (Valve Index)", "OpenVR Projection" }.ToList();


        //Actions

        [UIAction("set-preset")]
        public void SetPreset(string preset)
        {
            switch (preset)
            {
                case "Vive Wands (HTC Vive)":
                    ProjectionDistance = 70; // change
                    ProjectionScale = 16; // these
                    ProjectionX = 0; // values
                    ProjectionY = -26; // later!
                    break;
                case "Knuckles (HTC Vive)":
                    ProjectionDistance = 70;
                    ProjectionScale = 16;
                    ProjectionX = 6;
                    ProjectionY = -32;
                    break;
                case "OpenVR Projection":
                    ProjectionDistance = 70;
                    ProjectionScale = 36;
                    ProjectionX = 8;
                    ProjectionY = -32;
                    break;
                default:
                    ProjectionDistance = 50;
                    ProjectionScale = 10;
                    ProjectionX = 0;
                    ProjectionY = 0;
                    break;
            }
        }

        [UIAction("apply-projection")]
        private void _ApplyProjection() => ARCompanion.xrcamBehaviour.InitCameraPlane();
        [UIAction("reset-projection")]
        private void _ResetProjection()
        {
            ProjectionDistance = 50;
            ProjectionScale = 10;
            ProjectionX = 0;
            ProjectionY = 0;
            DefaultPreset = "Generic";
            ARCompanion.xrcamBehaviour.InitCameraPlane();
        }
    }
}

using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.IO;
using UnityEngine;

namespace ARCompanion
{
    class CameraOffsetMenu : BSMLResourceViewController
    {
        public override string ResourceName => "ARCompanion.Views.CameraOffsetMenu.bsml";

        //Values
        [UIValue("projection-scale")] private float _ProjectionScale => Settings.instance.ProjectionScale;
        [UIValue("projection-dist")] private float _ProjectionDistance => Settings.instance.ProjectionDistance;
        [UIValue("projection-offset-x")] private float _ProjectionX => Settings.instance.ProjectionXOffset;
        [UIValue("projection-offset-y")] private float _ProjectionY => Settings.instance.ProjectionYOffset;

        //Actions
        [UIAction("projectionscale-onchange")]
        private void _SetProjectionScale(float newval)
        {
            Settings.instance.ProjectionScale = newval;
        }

        [UIAction("projectionposx-onchange")] private void _SetProjectionX(float newval) => Settings.instance.ProjectionXOffset = newval;
        [UIAction("projectionposy-onchange")] private void _SetProjectionY(float newval) => Settings.instance.ProjectionYOffset = newval;
        [UIAction("projectiondist-onchange")] private void _SetProjectionDist(float newval) => Settings.instance.ProjectionDistance = newval;

        [UIAction("apply-projection")]
        private void _ApplyProjection() => ARCompanion.xrcamBehaviour.InitCameraPlane();
        [UIAction("reset-projection")]
        private void _ResetProjection() 
        {
            Settings.instance.ProjectionDistance = 50;
            Settings.instance.ProjectionScale = 10;
            ARCompanion.xrcamBehaviour.InitCameraPlane();
        }
    }
}

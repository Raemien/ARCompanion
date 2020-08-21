using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.IO;
using UnityEngine;

namespace ARCompanion
{
    class SideConfigMenuViewController : BSMLResourceViewController
    {
        public override string ResourceName => "ARCompanion.Views.SideConfigMenu.bsml";

        //Values
        [UIValue("enabled")] private bool _EnableSky => Settings.instance.EnableBackgrounds;
        [UIValue("hideobj-menu")] private bool _HideArrows => Settings.instance.HideMenuEnv;
        [UIValue("hideobj-game")] private bool _HidePillars => Settings.instance.HideGameEnv;


        //Actions
        [UIAction("enabled-onchange")]
        private void _SetEnableSky(bool newval)
        {
            Settings.instance.EnableBackgrounds = newval;
            Settings.instance.ToggleBackgrounds(newval);
        }
        [UIAction("hideobj-menu-onchange")]
        private void _SetHideMenuEnvs(bool newval)
        {
            Settings.instance.HideMenuEnv = newval;
            EnvironmentHider.HideEnvironmentObjects(true);
        }

        [UIAction("hideobj-game-onchange")]
        private void _SetHideGameEnvs(bool newval)
        {
            Settings.instance.HideGameEnv = newval;
            EnvironmentHider.HideEnvironmentObjects(true);
        }
    }
}

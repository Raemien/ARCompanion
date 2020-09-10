﻿using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using BS_Utils.Utilities;
using HMUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace ARCompanion
{
    class ARMenuViewController : BSMLResourceViewController
    {
        public override string ResourceName => "ARCompanion.Views.WebcamMenu.bsml";
        [UIComponent("webcam-list")] public CustomListTableData webcamList;
        private List<string> _WebcamList = new List<string>();

        [UIAction("on-select-cam")]
        private void SetBackground(TableView table, int selectedrow)
        {
            if (ARCompanion.xrcamBehaviour != null)
            {
                string chosencam = webcamList.data[selectedrow].text;
                Settings.instance.SelectedWebcam = chosencam;
                if (chosencam == "SteamVR")
                {
                    Settings.instance.UndistortRawFeed = true;
                    FindObjectOfType<CameraOffsetMenu>().SetProperty("DefaultPreset", "OpenVR Projection");
                    FindObjectOfType<CameraOffsetMenu>().SetPreset("OpenVR Projection");
                }
                ARCompanion.xrcamBehaviour.InitCameraPlane(chosencam);
                Logger.Log("Set camera to " + chosencam);
            }
        }
        [UIAction("#post-parse")]
        private void SetupWebcamList()
        {
            if (!_WebcamList.Contains("None"))
            {
                bool hmdHasCamera = false;
                OpenVR.TrackedCamera.HasCamera(OpenVR.k_unTrackedDeviceIndex_Hmd, ref hmdHasCamera);
                Texture2D autoicon = UIUtilities.LoadTextureFromResources("ARCompanion.Resources.Icons.auto.png");

                CustomListTableData.CustomCellInfo defaultcamCellInfo = new CustomListTableData.CustomCellInfo("None", "No camera");
                CustomListTableData.CustomCellInfo autocamCellInfo = new CustomListTableData.CustomCellInfo("Auto", "Automatically find any headset cameras.", autoicon);

                webcamList.data.Add(defaultcamCellInfo);
                webcamList.data.Add(autocamCellInfo);
                _WebcamList.Add("None");
                _WebcamList.Add("Auto");

                EVRSettingsError error = EVRSettingsError.None;
                bool cameraIsEnabled = OpenVR.Settings.GetBool(OpenVR.k_pch_Camera_Section, OpenVR.k_pch_Camera_EnableCamera_Bool, ref error);
                if (error != EVRSettingsError.None)
                {
                    Logger.Log("Error getting OpenVR camera settings: " + error, IPA.Logging.Logger.Level.Error);
                }
                else if (hmdHasCamera && cameraIsEnabled)
                {
                    Texture2D openvricon = UIUtilities.LoadTextureFromResources("ARCompanion.Resources.Icons.openvr.png");
                    CustomListTableData.CustomCellInfo openvrcamCellInfo = new CustomListTableData.CustomCellInfo("SteamVR", "Use SteamVR's camera system. (Recommended)", openvricon);
                    webcamList.data.Add(openvrcamCellInfo);
                    _WebcamList.Add("SteamVR");
                }

            }
            RefreshWebcamList();
        }

        public void RefreshWebcamList() 
        {
            try
            {
                WebCamDevice[] avaliableCameras = WebCamTexture.devices;
                foreach (var camera in avaliableCameras)
                {
                    string camName = camera.name;
                    if (camName != null && !_WebcamList.Contains(camName))
                    {
                        string subText = camera.kind.ToString() + " Webcam";
                        CustomListTableData.CustomCellInfo customCellInfo = new CustomListTableData.CustomCellInfo(camName, subText, null);
                        webcamList.data.Add(customCellInfo);
                        _WebcamList.Add(camName);
                    }
                }
                webcamList.tableView.ReloadData();
            }
            catch (Exception e)
            {
                Logger.Log(e.Message, IPA.Logging.Logger.Level.Error);
                throw;
            }

        }

    }
}

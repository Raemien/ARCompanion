using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using BS_Utils.Utilities;
using HMUI;
using System;
using System.Collections.Generic;
using UnityEngine;

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
                ARCompanion.xrcamBehaviour.InitCameraPlane(chosencam);
                Logger.Log("Set camera to " + chosencam);
            }
        }
        [UIAction("#post-parse")]
        private void SetupWebcamList()
        {
            if (!_WebcamList.Contains("None"))
            {
                CustomListTableData.CustomCellInfo defaultskyCellInfo = new CustomListTableData.CustomCellInfo("None", "No camera");
                CustomListTableData.CustomCellInfo autoskyCellInfo = new CustomListTableData.CustomCellInfo("Auto", "Automatically find any headset cameras.");

                webcamList.data.Add(defaultskyCellInfo);
                webcamList.data.Add(autoskyCellInfo);
                _WebcamList.Add("None");
                _WebcamList.Add("Auto");
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

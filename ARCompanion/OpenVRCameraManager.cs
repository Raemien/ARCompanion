using UnityEngine;
using Valve.VR;

namespace ARCompanion
{
    public class OpenVRCameraManager : MonoBehaviour
    {
        public Texture2D tcamTex;
        GameObject planeObject;
        private void OnEnable()
        {
            var config = Settings.instance;
            SteamVR_TrackedCamera.VideoStreamTexture camSource = SteamVR_TrackedCamera.Source(config.UndistortRawFeed);
            camSource.Acquire();
            this.enabled = camSource.hasCamera;
        }

        private void OnDisable()
        {
            var config = Settings.instance;
            SteamVR_TrackedCamera.VideoStreamTexture camSource = SteamVR_TrackedCamera.Source(config.UndistortRawFeed);
            camSource.Release();
        }

        private void Update()
        {
            var config = Settings.instance;
            planeObject = ARCompanion.xrcamBehaviour.planeObject;
            SteamVR_TrackedCamera.VideoStreamTexture videoSource = SteamVR_TrackedCamera.Source(config.UndistortRawFeed);
            tcamTex = videoSource.texture;
            if (planeObject != null)
            {
                ARCompanion.xrcamBehaviour.planeMat.SetTexture("_Tex", tcamTex);
                planeObject.transform.localRotation = Quaternion.Euler(-90, 0, -180);
            }

        }
    }
}

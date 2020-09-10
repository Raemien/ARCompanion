using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

namespace ARCompanion
{

    public class XRCameraBehaviour : MonoBehaviour
    {
        public readonly string[] monocameraDevices = { "HTC Vive" };
        public readonly string[] dualcameraDevices = { "eTronVideo", "VIVE Pro Multimedia Camera", "VIVE Pro Camera" }; // Vive Pro cameras have yet to be tested.
        private readonly string[] cameraObjects = { "Origin/MainMenuCamera", "Wrapper/Origin/MainMenuCamera", "Wrapper/Origin/MainCamera" };
        private AssetBundle shaderAssetBundle;
        public WebCamTexture webcamTexture;
        public Material planeMat;
        public Shader planeShader;
        public GameObject planeObject;
        public GameObject planeContainer;
        public GameObject hmdRef;
        public string webcamName;

        private void Update()
        {
            try
            {
                var config = Settings.instance;

                if (planeObject == null || planeContainer == null || planeMat == null)
                {
                    InitCameraPlane(config.SelectedWebcam);
                }
                if (webcamTexture != null)
                {
                    if (Input.GetKeyDown(KeyCode.J))
                    {
                        RefreshWebcam();
                    }
                    if (Camera.main != null || planeContainer == null)
                    {
                        SetCamOffsets();
                        planeContainer.transform.parent = Camera.main.transform;
                    }
                    ARCompanion.xrcamBehaviour.planeContainer.SetActive(true);
                }

                string curscene = SceneManager.GetActiveScene().name;

                switch (curscene)
                {
                    case "MenuViewControllers":
                    case "GameCore":
                        EnvironmentHider.HideEnvironmentObjects(true, config.SelectedWebcam == "None");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        private void Awake()
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            webcamTexture = new WebCamTexture();
            webcamName = "";

            if (devices.Length > 0)
            {
                RefreshWebcam();
            }
        }
        public void InitCameraPlane(string camname = "")
        {
            var config = Settings.instance;
            if (planeObject == null)
            {
                planeContainer = new GameObject();
                planeContainer.name = "_ARContainer";
                planeObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
                planeObject.transform.SetParent(planeContainer.transform);
                planeObject.name = "ARCameraPlane";
            }

            if (camname == "None" || config.SelectedWebcam == "None")
            {
                EnvironmentHider.HideEnvironmentObjects(true, true);
                planeObject.SetActive(false);
                return;
            }

            planeObject.SetActive(true);
            Renderer planeRenderer = planeObject.GetComponent<Renderer>();

            if (planeMat == null || !planeMat.HasProperty("_Tex"))
            {
                Stream abStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ARCompanion.Resources.ARCam");
                byte[] assetbin = new byte[abStream.Length];
                abStream.Read(assetbin, 0, (int)abStream.Length);
                shaderAssetBundle = AssetBundle.LoadFromMemory(assetbin);
                planeShader = shaderAssetBundle.LoadAllAssets<Shader>().First();
                planeMat = shaderAssetBundle.LoadAllAssets<Material>().First();
            }

            planeRenderer.material = planeMat;
            planeMat.SetTexture("_Tex", webcamTexture);
            planeMat.color.ColorWithAlpha(0);

            webcamName = camname;

            planeObject.transform.localEulerAngles = new Vector3(90, 0, 180);
            if (shaderAssetBundle != null)
            {
                shaderAssetBundle.Unload(false);
            }
            RefreshWebcam();
        }

        private void UnloadBackground()
        {
            if (shaderAssetBundle != null)
            {
                shaderAssetBundle.Unload(false);
            }
            planeObject.SetActive(false);
        }
        private void SetCamOffsets()
        {
            var config = Settings.instance;
            float pscale = config.ProjectionScale;
            float pdist = config.ProjectionDistance;
            float pposx = config.ProjectionXOffset;
            float pposy = config.ProjectionYOffset;

            if (config.SelectedWebcam == "None")
            {
                return;
            }

            if (dualcameraDevices.Contains(config.SelectedWebcam) || dualcameraDevices.Contains(webcamName))
            {
                planeMat.SetTextureOffset("_Tex", new Vector2((float)0.5, 0));
                planeMat.SetTextureScale("_Tex", new Vector2((float)0.5, 1));
                planeObject.transform.localScale = new Vector3(pscale * 2, pscale, pscale);
            }
            else
            {
                planeMat.SetTextureOffset("_Tex", new Vector2(0, 0));
                planeMat.SetTextureScale("_Tex", new Vector2(1, 1));
                planeObject.transform.localScale = Vector3.one * pscale;
            }
            planeObject.transform.localScale = Vector3.one * pscale;
            planeObject.transform.localPosition = new Vector3(pposx, pposy, pdist);
            planeContainer.transform.localEulerAngles = Vector3.zero;
        }
        private void RefreshWebcam(string cameraName = "")
        {
            var config = Settings.instance;
            string newwebcam = cameraName == "" ? config.SelectedWebcam : cameraName;
            if (newwebcam == "Auto")
            {
                WebCamDevice[] avaliableWebCams = WebCamTexture.devices;
                InputDevice controllerRef = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                string preset = "";
                string inputRefName = "";
                if (controllerRef != null && controllerRef.name != null)
                {
                    inputRefName = controllerRef.name;
                }
                foreach (var camera in WebCamTexture.devices)
                {
                    switch (camera.name)
                    {
                        case "HTC Vive":
                            config.SelectedWebcam = camera.name;
                            newwebcam = camera.name;
                            preset = inputRefName.Contains("Vive. Controller MV") ? "Vive Wands (HTC Vive)" : "Knuckles (HTC Vive)";
                            break;
                        case "eTronVideo":
                            config.SelectedWebcam = camera.name;
                            newwebcam = camera.name;
                            preset = "Knuckles (Valve Index)";
                            break;
                        case "VIVE Pro Camera":
                            config.SelectedWebcam = camera.name;
                            newwebcam = camera.name;
                            preset = "None";
                            break;
                        default:
                            break;
                    }
                }
                new CameraOffsetMenu().SetPreset(preset);
                planeObject.SetActive(true);
                planeObject.transform.rotation = Quaternion.Euler(90, 0, 180);
            }
            EnvironmentHider.HideEnvironmentObjects(true, newwebcam == "Auto" || newwebcam == "None" || !(config.HideGameEnv || config.HideMenuEnv));

            if (newwebcam != "Auto")
            {
                webcamTexture.Stop();
                webcamTexture.deviceName = newwebcam;
                webcamTexture.Play();
            }
            else
            {
                Logger.Log("No headset cameras could be found.", IPA.Logging.Logger.Level.Info);
            }
        }

    }
}

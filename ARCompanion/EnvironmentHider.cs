using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ARCompanion
{
    internal static class EnvironmentHider
    {
        public static int hiddenObjs;
        public static void HideEnvironmentObjects(bool forceHide = false, bool unHide = false)
        {
            if (hiddenObjs < 65536 || forceHide) // Tacky workaround for onSceneLoaded events not reliably triggering with environments
            {
                try
                {
                    var config = Settings.instance;
                    const string obj_pplace_menu = "MenuEnvironment/DefaultEnvironment/PlayersPlace";
                    const string obj_pplace = "Environment/PlayersPlace";
                    const string obj_smoke = "Environment/BigSmokePS";
                    const string obj_chevron = "Environment/SpawnRotationChevron";
                    const string obj_menu = "MenuEnvironment";
                    Scene curScene = SceneManager.GetActiveScene();
                    GameObject environmentRoot;
                    Renderer[] appendedRenderers = { };
                    Renderer[] excludedRenderers = { };

                    switch (curScene.name)
                    {
                        case "GameCore":
                            environmentRoot = GameObject.Find(obj_pplace).transform.parent.gameObject;
                            foreach (var obj in curScene.GetRootGameObjects().Where(obj => obj.name.Contains("Ring")))
                            {
                                appendedRenderers = appendedRenderers.Concat(obj.GetComponentsInChildren<Renderer>()).ToArray();
                            }
                            if (GameObject.Find(obj_smoke) != null)
                            {
                                excludedRenderers = excludedRenderers.AddRangeToArray(!config.HideFog ? GameObject.Find(obj_smoke).GetComponentsInChildren<Renderer>() : Array.Empty<Renderer>());
                            }
                            excludedRenderers = excludedRenderers.AddRangeToArray(!config.HidePlatform && !GameObject.Find(obj_chevron) ? GameObject.Find(obj_pplace).GetComponentsInChildren<Renderer>() : Array.Empty<Renderer>());
                            break;
                        case "MenuViewControllers":
                            environmentRoot = GameObject.Find(obj_pplace_menu).transform.parent.parent.gameObject;
                            appendedRenderers = Array.Empty<Renderer>();
                            break;
                        default:
                            environmentRoot = null;
                            break;
                    }

                    if (environmentRoot != null)
                    {
                        Renderer[] renderers = environmentRoot.GetComponentsInChildren<Renderer>();
                        if (appendedRenderers.Length > 0)
                        {
                            renderers = renderers.Concat(appendedRenderers).ToArray();
                        }
                        renderers = renderers.Except(excludedRenderers).ToArray();
                        bool hiddensetting = curScene.name == "MenuViewControllers" ? !config.HideMenuEnv : !config.HideGameEnv;

                        foreach (Renderer renderer in renderers)
                        {
                            renderer.enabled = hiddensetting || unHide;
                            hiddenObjs += forceHide ? 1 : 0;
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}

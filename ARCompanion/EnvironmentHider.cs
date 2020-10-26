using HarmonyLib;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ARCompanion
{
    internal static class EnvironmentHider
    {
        public static int hiddenObjs;
        public static void HideEnvironmentObjects(bool forceHide = false, bool unHide = false, int cycles = 10)
        {
            if (DateTimeOffset.Now.ToUnixTimeSeconds() % 2 != 0)
            {
                return;
            }
            if (hiddenObjs < 65536 || forceHide) // Tacky workaround for onSceneLoaded events not reliably triggering with environments
            {
                try
                {
                    var config = Settings.instance;
                    // Normal Objects (nice and organised)
                    const string obj_pplace = "Environment/PlayersPlace";
                    const string obj_smoke = "Environment/BigSmokePS";
                    const string obj_chevron = "Environment/SpawnRotationChevron";
                    const string obj_menu = "MenuEnvironment";
                    const string obj_game = "Environment";
                    // Multiplayer Objects (literally anarchy)
                    const string obj_specply = "Environment/GrandstandSpectatingSpot/MultiplayerLocalInactivePlayerController(Clone)/Origin";
                    const string obj_multidecor = "MultiplayerLocalActivePlayerController(Clone)/IsActiveObjects";
                    const string obj_multilobby = "MenuEnvironment/MultiplayerLobbyEnvironment";
                    const string obj_multiscore = "ArenaScoreBoard";
                    const string obj_grandstand = "Environment/GrandstandSpectatingSpot";
                    Scene curScene = SceneManager.GetActiveScene();
                    GameObject environmentRoot;
                    Renderer[] appendedRenderers = { };
                    Renderer[] excludedRenderers = { };

                    switch (curScene.name)
                    {
                        case "GameCore":
                            if (GameObject.Find(obj_multiscore) == null) // GameCore: normal environment
                            {
                                environmentRoot = GameObject.Find(obj_pplace).transform.parent.gameObject;
                                foreach (var obj in curScene.GetRootGameObjects().Where(obj => obj.name.Contains("Ring")))
                                {
                                    appendedRenderers = appendedRenderers.Concat(obj.GetComponentsInChildren<Renderer>()).ToArray();
                                }
                                excludedRenderers = excludedRenderers.AddRangeToArray(!config.HidePlatform && !GameObject.Find(obj_chevron) ? GameObject.Find(obj_pplace).GetComponentsInChildren<Renderer>() : Array.Empty<Renderer>());
                            }
                            else // GameCore: mulitplayer environment
                            {
                                environmentRoot = GameObject.Find(obj_game);
                                GameObject grandstand = null;
                                GameObject multidecor = null;
                                GameObject specply = null;
                                if (cycles > 2)
                                {
                                    grandstand = GameObject.Find(obj_grandstand);
                                    multidecor = GameObject.Find(obj_multidecor);
                                    specply = GameObject.Find(obj_specply);
                                }
                                MultiplayerConnectedPlayerFacade[] playerPlatforms = UnityEngine.Object.FindObjectsOfType<MultiplayerConnectedPlayerFacade>();

                                foreach (var playerenv in playerPlatforms)
                                {   
                                    Transform plyenvtrans = playerenv.gameObject.transform;
                                    appendedRenderers = appendedRenderers.Concat(plyenvtrans.Find("Construction").GetComponentsInChildren<Renderer>()).ToArray();
                                    appendedRenderers = appendedRenderers.Concat(plyenvtrans.Find("Lasers").GetComponentsInChildren<Renderer>()).ToArray();
                                }

                                if (grandstand != null)
                                {
                                    appendedRenderers = appendedRenderers.Concat(grandstand.GetComponentsInChildren<Renderer>()).ToArray();
                                }
                                if (multidecor != null)
                                {
                                    appendedRenderers = appendedRenderers.Concat(multidecor.transform.Find("Construction").GetComponentsInChildren<Renderer>()).ToArray();
                                    appendedRenderers = appendedRenderers.Concat(multidecor.transform.Find("Lasers").GetComponentsInChildren<Renderer>()).ToArray();
                                }
                                if (specply != null)
                                {
                                    excludedRenderers = excludedRenderers.AddRangeToArray(specply.GetComponentsInChildren<Renderer>()).ToArray();
                                }
                            }
                            if (GameObject.Find(obj_smoke) != null)
                            {
                                excludedRenderers = excludedRenderers.AddRangeToArray(!config.HideFog ? GameObject.Find(obj_smoke).GetComponentsInChildren<Renderer>() : Array.Empty<Renderer>());
                            }
                            break;
                        case "MenuViewControllers":
                            environmentRoot = GameObject.Find(obj_menu);
                            appendedRenderers = Array.Empty<Renderer>();
                            if (GameObject.Find(obj_multilobby) != null) // MenuViewControllers: multiplayer environment
                            {
                                appendedRenderers = appendedRenderers.Concat(GameObject.Find(obj_multilobby).GetComponentsInChildren<Renderer>()).ToArray();
                                foreach (var avatarplatform in UnityEngine.Object.FindObjectsOfType<MultiplayerLobbyAvatarPlace>())
                                {
                                    appendedRenderers = appendedRenderers.Concat(avatarplatform.gameObject.GetComponentsInChildren<Renderer>()).ToArray();
                                }
                            }
                            break;
                        default:
                            environmentRoot = null;
                            break;
                    }

                    if (environmentRoot != null) // Apply hidden renderers
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
                            //hiddenObjs += forceHide ? 1 : 0;
                        }
                        cycles += forceHide ? 1 : 0;
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

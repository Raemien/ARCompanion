using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;

namespace ARCompanion
{
    public class UISingleton : PersistentSingleton<UISingleton>
    {
        public static MenuButton menuButton = new MenuButton("AR Settings", "Augmented Reality options", OpenARMenu);
        private static ARMenuFlowCoordinator augMenuFlowCoordinator;

        private static void OpenARMenu()
        {
            if (augMenuFlowCoordinator == null)
            {
                augMenuFlowCoordinator = BeatSaberUI.CreateFlowCoordinator<ARMenuFlowCoordinator>();
            }
            BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(augMenuFlowCoordinator, null, false, false);
        }
        public static void RegMenuButton()
        {
            MenuButtons.instance.RegisterButton(menuButton);
        }
        public static void RemoveMenuButton()
        {
            MenuButtons.instance.UnregisterButton(menuButton);
        }
    }
}

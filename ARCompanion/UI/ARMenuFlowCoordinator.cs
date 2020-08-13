using BeatSaberMarkupLanguage;
using HMUI;

namespace ARCompanion
{
    public class ARMenuFlowCoordinator : FlowCoordinator
    {
        private ARMenuViewController bgMenuView;
        private SideConfigMenuViewController sideConfigView;

        private void Awake()
        {
            if (!bgMenuView)
            {
                bgMenuView = BeatSaberUI.CreateViewController<ARMenuViewController>();
            }
            if (!sideConfigView)
            {
                sideConfigView = BeatSaberUI.CreateViewController<SideConfigMenuViewController>();
            }
        }

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (firstActivation)
            {
                title = "AR Companion";
                showBackButton = true;
                ProvideInitialViewControllers(bgMenuView, sideConfigView);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this, null, false);
        }


    }
}

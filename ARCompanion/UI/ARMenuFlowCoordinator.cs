using BeatSaberMarkupLanguage;
using HMUI;

namespace ARCompanion
{
    public class ARMenuFlowCoordinator : FlowCoordinator
    {
        private ARMenuViewController bgMenuView;
        private CameraOffsetMenu cameraOffsetView;
        private SideConfigMenuViewController sideConfigView;

        private void Awake()
        {
            if (!bgMenuView)
            {
                bgMenuView = BeatSaberUI.CreateViewController<ARMenuViewController>();
            }
            if (!cameraOffsetView)
            {
                cameraOffsetView = BeatSaberUI.CreateViewController<CameraOffsetMenu>();
            }
            if (!sideConfigView)
            {
                sideConfigView = BeatSaberUI.CreateViewController<SideConfigMenuViewController>();
            }
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation)
            {
                SetTitle("AR Companion");
                showBackButton = true;
                ProvideInitialViewControllers(bgMenuView, sideConfigView, cameraOffsetView);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Vertical);
        }

    }
}

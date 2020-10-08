# AR Companion
A plugin that uses a headset-mounted camera as a make-shift Augmented/Mixed Reality setup. Useful for XR streamers.

## Useful information:
- If the video device disconnects from the PC, press the J key to reload the feed.
- Offsets can be applied within the ARCompanion menu to better align with your headset view.
- The "OpenVR Projection" preset doesn't yet account for the Valve Index's camera displacement. If you're an Index user, please open an issue with better offset settings.
## Configuration:
* **Selected Webcam:** The video device name to use. "eTronVideo" for the Valve Index's dual cameras.
* **Projection Distance:** Distance in metres to move the background plane.
* **Projection Scale:** Size in meters to scale the background plane.
* **Position Offset X** Left/right positional modifier for the background plane.
* **Position Offset Y** Up/down positional modifier for the background plane.
## Camera Compatibility:
|Camera| Compatible
|-------------------------------|-----------|
|**Generic USB webcams**| Yes|
| **HTC Vive camera**| Yes|
| **Valve Index dual cameras**| Yes|
| **Rift S IR Camera**| No|
| **Quest/Quest 2 IR Cameras**| No|
| **Vive Cosmos Camera**| Untested|
| **Windows Mixed Reality Cameras**| Untested|

Note that most webcams detected by Windows can be used as an alternative to the headset's built-in camera.

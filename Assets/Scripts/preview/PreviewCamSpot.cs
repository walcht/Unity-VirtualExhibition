using UnityEngine;
using Common;

/// <summary>
///     Attach this script to any GameObject you want the main camera controller to transition to.
///     Make sure that there is a camera spot UI attached to a child of this GameObject.
/// </summary>
public class PreviewCamSpot : MonoBehaviour
{
    public CamControllerSettings camControllerSettings;
    public Canvas cameraSpotUI;

    private void OnEnable()
    {
        PreviewUIManager.Instance.AddSpotCameraUI(cameraSpotUI.gameObject);
    }
    public void OnClick()
    {
        PreviewUIManager.Instance.CurrentCameraSpotUI = cameraSpotUI;
        PreviewCamController.Instance.Settings = camControllerSettings;
    }
}

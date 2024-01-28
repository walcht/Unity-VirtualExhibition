using System.Runtime.InteropServices;
using Common;
using UnityEngine;

public class CustomizableObject : MonoBehaviour, ICustomizableObject
{
    public GameObject customizationUI;
    public GameObject previewUI;
    public CamControllerSettings camControllerSettings;
    public StandRelatedData StandRelatedDataSO;

    public CamControllerSettings GetCamControllerSettings() => camControllerSettings;

    public GameObject GetCustomizationUI() => customizationUI;

    public GameObject GetPreviewUI() => previewUI;

    [DllImport("__Internal")]
    private static extern void ExhibitionCustomizeSponsorBannerEvent(
        int sponsor_banner_index,
        int width,
        int height
    );

    [DllImport("__Internal")]
    private static extern void ExhibitionCustomizeSponsorDiscEvent(
        int area_index,
        int width,
        int height
    );

    [DllImport("__Internal")]
    private static extern void ExhibitionCustomizeSponsorScreenEvent(int width, int height);

    [DllImport("__Internal")]
    private static extern void ExhibitionCustomizeSponsorCylinderEvent(
        int sponsor_cylinde_index,
        int width,
        int height
    );

    [DllImport("__Internal")]
    private static extern void EntranceCustomizeSponsorBanner00Event(int width, int height);

    [DllImport("__Internal")]
    private static extern void EntranceCustomizeSponsorBanner01Event(int width, int height);

    [DllImport("__Internal")]
    private static extern void EntranceCustomizeSponsorScreenEvent(
        int area_index,
        int width,
        int height
    );

    private void OnEnable()
    {
        // inform PreviewUIManager about this customizable object if this is a preview scene
        if (
            CustomizationManager.Instance.isThisEntrancePreview
            || CustomizationManager.Instance.isThisExhibitionPreview
        )
            PreviewUIManager.Instance.AddCustomizableObject(gameObject);
    }

    public void OnPreviewUIClick()
    {
        PreviewUIManager.Instance.OnEnterCustomizablePreview(GetComponent<ICustomizableObject>());
        PreviewCamController.Instance.Settings = camControllerSettings;
    }

    // EVENT DISPATCHERS
    public void OnExhibitionCustomizeSponsorCylinder(int sponsor_cylinder_index)
    {
        if (sponsor_cylinder_index > 3)
            Debug.LogError("There are only 4 customizable exhibition sponsor cylinders.");
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ExhibitionCustomizeSponsorCylinderEvent(
            sponsor_cylinder_index,
            StandRelatedDataSO.SponsorCylinder_dimensions[0].width,
            StandRelatedDataSO.SponsorCylinder_dimensions[0].height
        );
#endif
    }

    public void OnExhibitionCustomizeSponsorScreen()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ExhibitionCustomizeSponsorScreenEvent(
            StandRelatedDataSO.SponsorScreen_dimensions[0].width,
            StandRelatedDataSO.SponsorScreen_dimensions[0].height
        );
#endif
    }

    public void OnExhibitionCustomizeSponsorDisc(int area_index)
    {
        if (area_index > 3)
            Debug.LogError("There are only 4 cutomizable sponsor disc areas.");
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ExhibitionCustomizeSponsorDiscEvent(
            area_index,
            StandRelatedDataSO.SponsorDisk_dimensions[area_index].width,
            StandRelatedDataSO.SponsorDisk_dimensions[area_index].height
        );
#endif
    }

    public void OnExhibitionCustomizeSponsorBanner(int sponsor_banner_index)
    {
        if (sponsor_banner_index > 3)
            Debug.LogError(
                "There are only 4 sponsor banners, make sure provided index doesn't go beyond the limit."
            );
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ExhibitionCustomizeSponsorBannerEvent(
            sponsor_banner_index,
            StandRelatedDataSO.SponsorBanner_type00_dimensions[0].width,
            StandRelatedDataSO.SponsorBanner_type00_dimensions[0].height
        );
#endif
    }

    public void OnEntranceCustomizeSponsorBanner00()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        EntranceCustomizeSponsorBanner00Event(
            StandRelatedDataSO.SponsorBanner_type00_dimensions[0].width,
            StandRelatedDataSO.SponsorBanner_type00_dimensions[0].height
        );
#endif
    }

    public void OnEntranceCustomizeSponsorBanner01()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        EntranceCustomizeSponsorBanner01Event(
            StandRelatedDataSO.SponsorBanner_type00_dimensions[0].width,
            StandRelatedDataSO.SponsorBanner_type00_dimensions[0].height
        );
#endif
    }

    public void OnEntranceCustomizeSponsorScreen(int area_index)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        EntranceCustomizeSponsorScreenEvent(
            area_index,
            StandRelatedDataSO.SponsorCubeScreen_dimensions[area_index].width,
            StandRelatedDataSO.SponsorCubeScreen_dimensions[area_index].height
        );
#endif
    }
}

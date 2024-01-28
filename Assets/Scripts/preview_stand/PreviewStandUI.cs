using System.Runtime.InteropServices;
using UnityEngine;

public class PreviewStandUI : MonoBehaviour
{
    public delegate void CharacterChangeHandler(int position, int index_delta_update);
    public event CharacterChangeHandler CharacterChangeEvent;

    public StandRelatedData standRelatedDataSO;
    
    [DllImport("__Internal")] private static extern void CustomizeBannerAreaEvent(int area_id, int width, int height);
    [DllImport("__Internal")] private static extern void CustomizeStandAreaEvent(int area_id, int width, int height);

    public void OnCharacterChange00(int index_delta_update)
    {
        if (CharacterChangeEvent != null)
        {
            CharacterChangeEvent.Invoke(00, index_delta_update);
            return;
        }
    }

    public void OnCharacterChange01(int index_delta_update)
    {
        if (CharacterChangeEvent != null)
        {
            CharacterChangeEvent.Invoke(01, index_delta_update);
            return;
        }
    }

    // Stands Event Invokers
    public void OnCustomizeStandXL(int area_id)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        CustomizeStandAreaEvent(area_id, standRelatedDataSO.XL_dimensions[area_id].width, standRelatedDataSO.XL_dimensions[area_id].height);
#endif
    }

    public void OnCustomizeStandLL(int area_id)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        CustomizeStandAreaEvent(area_id, standRelatedDataSO.LL_dimensions[area_id].width, standRelatedDataSO.LL_dimensions[area_id].height);
#endif
    }

    public void OnCustomizeStandLR(int area_id)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        CustomizeStandAreaEvent(area_id, standRelatedDataSO.LR_dimensions[area_id].width, standRelatedDataSO.LR_dimensions[area_id].height);
#endif
    }

    public void OnCustomizeStandM(int area_id)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        CustomizeStandAreaEvent(area_id, standRelatedDataSO.M_dimensions[area_id].width, standRelatedDataSO.M_dimensions[area_id].height);
#endif
    }

    public void OnCustomizeStandS(int area_id)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        CustomizeStandAreaEvent(area_id, standRelatedDataSO.S_dimensions[area_id].width, standRelatedDataSO.S_dimensions[area_id].height);
#endif
    }

    // Stand XL Banners Event Invokers

    public void OnCustomizeStandXLBanner00(int area_id)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        CustomizeBannerAreaEvent(area_id, standRelatedDataSO.Banner_XL_00_dimensions[area_id].width, standRelatedDataSO.Banner_XL_00_dimensions[area_id].height);
#endif
    }

    public void OnCustomizeStandXLBanner01(int area_id)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        CustomizeBannerAreaEvent(area_id, standRelatedDataSO.Banner_XL_01_dimensions[area_id].width, standRelatedDataSO.Banner_XL_01_dimensions[area_id].height);
#endif
    }

    public void OnCustomizeStandXLBanner02(int area_id)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        CustomizeBannerAreaEvent(area_id, standRelatedDataSO.Banner_XL_02_dimensions[area_id].width, standRelatedDataSO.Banner_XL_02_dimensions[area_id].height);
#endif
    }

    public void OnCustomizeStandXLBanner03(int area_id)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        CustomizeBannerAreaEvent(area_id, standRelatedDataSO.Banner_XL_03_dimensions[area_id].width, standRelatedDataSO.Banner_XL_03_dimensions[area_id].height);
#endif
    }

    // Stand LR Banners Event Invokers
    public void OnCustomizeStandLANDRBanner00(int area_id)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        CustomizeBannerAreaEvent(area_id, standRelatedDataSO.Banner_LANDR_00_dimensions[area_id].width, standRelatedDataSO.Banner_LANDR_00_dimensions[area_id].height);
#endif
    }

    public void OnCustomizeStandLANDRBanner01(int area_id)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        CustomizeBannerAreaEvent(area_id, standRelatedDataSO.Banner_LANDR_01_dimensions[area_id].width, standRelatedDataSO.Banner_LANDR_01_dimensions[area_id].height);
#endif
    }

    public void OnCustomizeStandLANDRBanner02(int area_id)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        CustomizeBannerAreaEvent(area_id, standRelatedDataSO.Banner_LANDR_02_dimensions[area_id].width, standRelatedDataSO.Banner_LANDR_02_dimensions[area_id].height);
#endif
    }

    // Stand M Banners Event Invokers
    public void OnCustomizeStandMBanner00(int area_id)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        CustomizeBannerAreaEvent(area_id, standRelatedDataSO.Banner_M_00_dimensions[area_id].width, standRelatedDataSO.Banner_M_00_dimensions[area_id].height);
#endif
    }

    public void OnCustomizeStandMBanner01(int area_id)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        CustomizeBannerAreaEvent(area_id, standRelatedDataSO.Banner_M_01_dimensions[area_id].width, standRelatedDataSO.Banner_M_01_dimensions[area_id].height);
#endif
    }
}

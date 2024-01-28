using System.Collections.Generic;
using Common;
using UnityEngine;

[CreateAssetMenu(fileName = "StandRelatedData", menuName = "SO/StandRelatedData")]
/// <summary>
///     Holds all necessary data related to stands such as: Stand character positions,
///     Stand camera position, etc.
/// </summary>
public class StandRelatedData : ScriptableObject
{
    // STAND AREAS DIMENSIONS
    public List<Dimension> XL_dimensions = new List<Dimension>();
    public List<Dimension> LL_dimensions = new List<Dimension>();
    public List<Dimension> LR_dimensions = new List<Dimension>();
    public List<Dimension> M_dimensions = new List<Dimension>();
    public List<Dimension> S_dimensions = new List<Dimension>();

    // XL BANNERS AREAS DIMENSIONS
    public List<Dimension> Banner_XL_00_dimensions = new List<Dimension>();
    public List<Dimension> Banner_XL_01_dimensions = new List<Dimension>();
    public List<Dimension> Banner_XL_02_dimensions = new List<Dimension>();
    public List<Dimension> Banner_XL_03_dimensions = new List<Dimension>();

    // LANDR BANNERS AREAS DIMENSIONS
    public List<Dimension> Banner_LANDR_00_dimensions = new List<Dimension>();
    public List<Dimension> Banner_LANDR_01_dimensions = new List<Dimension>();
    public List<Dimension> Banner_LANDR_02_dimensions = new List<Dimension>();

    // M BANNERS AREAS DIMENSIONS
    public List<Dimension> Banner_M_00_dimensions = new List<Dimension>();
    public List<Dimension> Banner_M_01_dimensions = new List<Dimension>();

    // SPONSOR AREAS DIMENSIONS
    public List<Dimension> SponsorBanner_type00_dimensions = new List<Dimension>();
    public List<Dimension> SponsorDisk_dimensions = new List<Dimension>();
    public List<Dimension> SponsorCylinder_dimensions = new List<Dimension>();
    public List<Dimension> SponsorCubeScreen_dimensions = new List<Dimension>();
    public List<Dimension> SponsorScreen_dimensions = new List<Dimension>();

    // STAND COLLIDER PREFAB
    public GameObject StandCollider;

    // STAND COLLIDER POSITIONS
    public Vector3 XLRightColliderPos;
    public Vector3 XLLeftColliderPos;
    public Vector3 LRColliderPos;
    public Vector3 LLColliderPos;
    public Vector3 MColliderPos;
    public Vector3 SColliderPos;

    // STAND CHARACTER(S) POSITIONS
    public Vector3 XLRightCharacterPos;
    public Vector3 XLLeftCharacterPos;
    public Vector3 LRCharacterPos;
    public Vector3 LLCharacterPos;
    public Vector3 MCharacterPos;
    public Vector3 SCharacterPos;

    /// <summary>
    ///     Call this function to get the dimensions of the provided Customizable Area
    /// </summary>
    /// <param name="customizableAreaID">Customizable Area ID whose dimension are going to be returned</param>
    /// <returns>Return dimensions; width, height of the customizable area.</returns>
    public Dimension GetAreaDimensions(CustomizableArea customizableAreaID)
    {
        int height = 0;
        int width = 0;
        switch (customizableAreaID)
        {
            case CustomizableArea.STAND_XL_AREA_00:
                width = XL_dimensions[0].width;
                height = XL_dimensions[0].height;
                break;
            case CustomizableArea.STAND_XL_AREA_01:
                width = XL_dimensions[1].width;
                height = XL_dimensions[1].height;
                break;
            case CustomizableArea.STAND_XL_AREA_02:
                width = XL_dimensions[2].width;
                height = XL_dimensions[2].height;
                break;
            case CustomizableArea.STAND_XL_AREA_03:
                width = XL_dimensions[3].width;
                height = XL_dimensions[3].height;
                break;

            case CustomizableArea.STAND_LL_AREA_00:
                width = LL_dimensions[0].width;
                height = LL_dimensions[0].height;
                break;
            case CustomizableArea.STAND_LL_AREA_01:
                width = LL_dimensions[1].width;
                height = LL_dimensions[1].height;
                break;
            case CustomizableArea.STAND_LL_AREA_02:
                width = LL_dimensions[2].width;
                height = LL_dimensions[2].height;
                break;

            case CustomizableArea.STAND_LR_AREA_00:
                width = LR_dimensions[0].width;
                height = LR_dimensions[0].height;
                break;
            case CustomizableArea.STAND_LR_AREA_01:
                width = LR_dimensions[1].width;
                height = LR_dimensions[1].height;
                break;
            case CustomizableArea.STAND_LR_AREA_02:
                width = LR_dimensions[2].width;
                height = LR_dimensions[2].height;
                break;

            case CustomizableArea.STAND_M_AREA_00:
                width = M_dimensions[0].width;
                height = M_dimensions[0].height;
                break;
            case CustomizableArea.STAND_M_AREA_01:
                width = M_dimensions[1].width;
                height = M_dimensions[1].height;
                break;

            case CustomizableArea.STAND_S_AREA_00:
                width = S_dimensions[0].width;
                height = S_dimensions[0].height;
                break;
            case CustomizableArea.STAND_S_AREA_01:
                width = S_dimensions[1].width;
                height = S_dimensions[1].height;
                break;

            case CustomizableArea.XL_BANNER_00_AREA_00:
                width = Banner_XL_00_dimensions[0].width;
                height = Banner_XL_00_dimensions[0].height;
                break;
            case CustomizableArea.XL_BANNER_00_AREA_01:
                width = Banner_XL_00_dimensions[1].width;
                height = Banner_XL_00_dimensions[1].height;
                break;

            case CustomizableArea.XL_BANNER_01_AREA_00:
                width = Banner_XL_01_dimensions[0].width;
                height = Banner_XL_01_dimensions[0].height;
                break;
            case CustomizableArea.XL_BANNER_01_AREA_01:
                width = Banner_XL_01_dimensions[1].width;
                height = Banner_XL_01_dimensions[1].height;
                break;

            case CustomizableArea.XL_BANNER_02_AREA_00:
                width = Banner_XL_02_dimensions[0].width;
                height = Banner_XL_02_dimensions[0].height;
                break;

            case CustomizableArea.XL_BANNER_03_AREA_00:
                width = Banner_XL_03_dimensions[0].width;
                height = Banner_XL_03_dimensions[0].height;
                break;
            case CustomizableArea.XL_BANNER_03_AREA_01:
                width = Banner_XL_03_dimensions[1].width;
                height = Banner_XL_03_dimensions[1].height;
                break;

            case CustomizableArea.LANDR_BANNER_00_AREA_00:
                width = Banner_LANDR_00_dimensions[0].width;
                height = Banner_LANDR_00_dimensions[0].height;
                break;

            case CustomizableArea.LANDR_BANNER_01_AREA_00:
                width = Banner_LANDR_01_dimensions[0].width;
                height = Banner_LANDR_01_dimensions[0].height;
                break;

            case CustomizableArea.LANDR_BANNER_02_AREA_00:
                width = Banner_LANDR_02_dimensions[0].width;
                height = Banner_LANDR_02_dimensions[0].height;
                break;

            case CustomizableArea.M_BANNER_00_AREA_00:
                width = Banner_M_00_dimensions[0].width;
                height = Banner_M_00_dimensions[0].height;
                break;

            case CustomizableArea.M_BANNER_01_AREA_00:
                width = Banner_M_01_dimensions[0].width;
                height = Banner_M_01_dimensions[0].height;
                break;

            case CustomizableArea.SPONSOR_BANNER_00_AREA_00:
                width = SponsorBanner_type00_dimensions[0].width;
                height = SponsorBanner_type00_dimensions[0].height;
                break;

            case CustomizableArea.SPONSOR_CYLINDER_00_AREA_00:
                width = SponsorCylinder_dimensions[0].width;
                height = SponsorCylinder_dimensions[0].height;
                break;

            case CustomizableArea.SPONSOR_DISK_AREA_00:
                width = SponsorDisk_dimensions[0].width;
                height = SponsorDisk_dimensions[0].height;
                break;
            case CustomizableArea.SPONSOR_DISK_AREA_01:
                width = SponsorDisk_dimensions[1].width;
                height = SponsorDisk_dimensions[1].height;
                break;
            case CustomizableArea.SPONSOR_DISK_AREA_02:
                width = SponsorDisk_dimensions[2].width;
                height = SponsorDisk_dimensions[2].height;
                break;
            case CustomizableArea.SPONSOR_DISK_AREA_03:
                width = SponsorDisk_dimensions[3].width;
                height = SponsorDisk_dimensions[3].height;
                break;

            case CustomizableArea.SPONSOR_CUBE_SCREEN_AREA_00:
                width = SponsorCubeScreen_dimensions[0].width;
                height = SponsorCubeScreen_dimensions[0].height;
                break;
            case CustomizableArea.SPONSOR_CUBE_SCREEN_AREA_01:
                width = SponsorCubeScreen_dimensions[1].width;
                height = SponsorCubeScreen_dimensions[1].height;
                break;
            case CustomizableArea.SPONSOR_CUBE_SCREEN_AREA_02:
                width = SponsorCubeScreen_dimensions[2].width;
                height = SponsorCubeScreen_dimensions[2].height;
                break;
            case CustomizableArea.SPONSOR_CUBE_SCREEN_AREA_03:
                width = SponsorCubeScreen_dimensions[3].width;
                height = SponsorCubeScreen_dimensions[3].height;
                break;
            case CustomizableArea.SPONSOR_SCREEN_AREA_00:
                width = SponsorScreen_dimensions[0].width;
                height = SponsorScreen_dimensions[0].height;
                break;
            default:
                break;
        }

        Dimension tmp_dim = new Dimension();
        tmp_dim.height = height;
        tmp_dim.width = width;
        return tmp_dim;
    }
}

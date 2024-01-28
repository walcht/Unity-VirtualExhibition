using UnityEngine;

/// <summary>
///     This ScriptableObject holds all data necessary for various UI elements such as:
///     UIRequest, UIStand, etc...
/// </summary>
[CreateAssetMenu(fileName = "UIData", menuName = "SO/UIData")]
public class UIData : ScriptableObject
{
    public GameObject UIRequest;

    // REQUEST UI POSITIONS DATA
    public Vector3 XLStandUIRequestPosition;
    public Vector3 LLStandUIRequestPosition;
    public Vector3 LRStandUIRequestPosition;
    public Vector3 MStandUIRequestPosition;
    public Vector3 SStandUIRequestPosition;

    public Vector3[] XLBannersUIRequestPosition = new Vector3[4];
    public Vector3[] LBannersUIRequestPosition = new Vector3[3];
    public Vector3[] MBannersUIRequestPosition = new Vector3[2];
}

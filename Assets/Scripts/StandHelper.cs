using System.Collections.Generic;
using Common;
using UnityEngine;

/// <summary>
///     Stand helper class that contains an API for interacting with the attached stand.
///     Make sure to attach this class to EVERY stand prefab.
/// </summary>
public class StandHelper : MonoBehaviour
{
    public StandType Type;
    public StandRelatedData StandRelatedDataSO;
    public UIData UIDataSO;

    [HideInInspector]
    public StandCustomizationData standCustomizationData = new StandCustomizationData();

    // in case of XL stand, this holds two instantiated StandColliders
    List<GameObject> instantiatedStandColliders = new List<GameObject>();

    // REFERENCES INSTANTIATED STAND CHARACTERS
    GameObject instantiatedRightCharacter; // all purchased stands have this character
    GameObject instantiatedLeftCharacter = null; // only for XL stand since it requires 2 stand characters

    // REFERENCES INSTANTIATED UI REQUEST (Please change this later)
    GameObject instantiatedUIRequest;

    /// <summary>
    ///     Instantiates stand collider(s) that is/are used for starting an interaction with the stand.
    /// </summary>
    public void InstantiateStandColliders()
    {
        switch (Type)
        {
            case StandType.XL:

                {
                    GameObject tmp00 = Instantiate<GameObject>(
                        StandRelatedDataSO.StandCollider,
                        transform
                    );
                    GameObject tmp01 = Instantiate<GameObject>(
                        StandRelatedDataSO.StandCollider,
                        transform
                    );

                    tmp00.transform.localPosition = StandRelatedDataSO.XLRightColliderPos;
                    tmp01.transform.localPosition = StandRelatedDataSO.XLLeftColliderPos;

                    instantiatedStandColliders.Add(tmp00);
                    instantiatedStandColliders.Add(tmp01);
                }
                break;

            case StandType.LL:

                {
                    GameObject tmp = Instantiate<GameObject>(
                        StandRelatedDataSO.StandCollider,
                        transform
                    );
                    tmp.transform.localPosition = StandRelatedDataSO.LLColliderPos;
                    instantiatedStandColliders.Add(tmp);
                }

                break;

            case StandType.LR:

                {
                    GameObject tmp = Instantiate<GameObject>(
                        StandRelatedDataSO.StandCollider,
                        transform
                    );
                    tmp.transform.localPosition = StandRelatedDataSO.LRColliderPos;
                    instantiatedStandColliders.Add(tmp);
                }
                break;

            case StandType.M:

                {
                    GameObject tmp = Instantiate<GameObject>(
                        StandRelatedDataSO.StandCollider,
                        transform
                    );
                    tmp.transform.localPosition = StandRelatedDataSO.MColliderPos;
                    instantiatedStandColliders.Add(tmp);
                }
                break;

            case StandType.S:

                {
                    GameObject tmp = Instantiate<GameObject>(
                        StandRelatedDataSO.StandCollider,
                        transform
                    );
                    tmp.transform.localPosition = StandRelatedDataSO.SColliderPos;
                    instantiatedStandColliders.Add(tmp);
                }
                break;

            default:
                break;
        }

        foreach (GameObject item in instantiatedStandColliders)
        {
            item.GetComponent<StandCollider>().ParentStand = this;
        }
    }

    /// <summary>
    ///     Destroys previously instantiated stand collider(s).
    /// </summary>
    public void DestroyStandColliders()
    {
        foreach (var item in instantiatedStandColliders)
        {
            Destroy(item);
        }
    }

    /// <summary>
    ///     Initializes and sets stand character(s) for this stand.
    /// </summary>
    /// <param name="rightStandCharacter">All purchased stands should have this character set.</param>
    /// <param name="leftStandCharacter">Only XL stands are required to provide secondary stand character.
    /// </param>
    public void SetStandCharacter(
        GameObject rightStandCharacter,
        GameObject leftStandCharacter = null
    )
    {
        instantiatedRightCharacter = Instantiate<GameObject>(rightStandCharacter, transform);
        switch (Type)
        {
            case StandType.XL:
                instantiatedRightCharacter.transform.localPosition =
                    StandRelatedDataSO.XLRightCharacterPos;

                instantiatedLeftCharacter = Instantiate<GameObject>(leftStandCharacter, transform);
                instantiatedLeftCharacter.transform.localPosition =
                    StandRelatedDataSO.XLLeftCharacterPos;
                break;

            case StandType.LL:
                instantiatedRightCharacter.transform.localPosition =
                    StandRelatedDataSO.LLCharacterPos;
                break;

            case StandType.LR:
                instantiatedRightCharacter.transform.localPosition =
                    StandRelatedDataSO.LRCharacterPos;
                break;

            case StandType.M:
                instantiatedRightCharacter.transform.localPosition =
                    StandRelatedDataSO.MCharacterPos;
                break;

            case StandType.S:
                instantiatedRightCharacter.transform.localPosition =
                    StandRelatedDataSO.SCharacterPos;
                break;

            default:
                break;
        }
    }

    ////////////////////////////////////////////// UI REQUEST RELATED ///////////////////////////////////////
    public void InitializeUIRequest()
    {
        switch (Type)
        {
            case StandType.XL:
                instantiatedUIRequest = Instantiate<GameObject>(UIDataSO.UIRequest, transform);
                instantiatedUIRequest.transform.localPosition = UIDataSO.XLStandUIRequestPosition;
                break;

            case StandType.LL:
                instantiatedUIRequest = Instantiate<GameObject>(UIDataSO.UIRequest, transform);
                instantiatedUIRequest.transform.localPosition = UIDataSO.LLStandUIRequestPosition;
                break;

            case StandType.LR:
                instantiatedUIRequest = Instantiate<GameObject>(UIDataSO.UIRequest, transform);
                instantiatedUIRequest.transform.localPosition = UIDataSO.LRStandUIRequestPosition;
                break;

            case StandType.M:
                instantiatedUIRequest = Instantiate<GameObject>(UIDataSO.UIRequest, transform);
                instantiatedUIRequest.transform.localPosition = UIDataSO.MStandUIRequestPosition;
                break;

            case StandType.S:
                instantiatedUIRequest = Instantiate<GameObject>(UIDataSO.UIRequest, transform);
                instantiatedUIRequest.transform.localPosition = UIDataSO.SStandUIRequestPosition;
                break;

            default:
                break;
        }
    }

    public void DestroyUIRequest()
    {
        Destroy(instantiatedUIRequest);
    }

    public bool IsShowUIRequestEnabled() => true;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
}

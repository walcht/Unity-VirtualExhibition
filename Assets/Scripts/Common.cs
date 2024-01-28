using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public static class HelperUtilities
    {
        public static GameObject GetChildGameObject(
            GameObject fromGameObject,
            string withName,
            bool raise_error_if_not_found = true
        )
        {
            Transform[] childrenGameObjects = fromGameObject.GetComponentsInChildren<Transform>();
            foreach (var child in childrenGameObjects)
                if (child.gameObject.name == withName)
                    return child.gameObject;
            if (raise_error_if_not_found)
                Debug.LogError(
                    string.Format(
                        "Could NOT find child with name: {0} of GameObject: {1}",
                        withName,
                        fromGameObject.name
                    )
                );
            return null;
        }
    }

    [System.Serializable]
    public class SerializableDictionary<TKey, TValue>
        : Dictionary<TKey, TValue>,
            ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keys = new List<TKey>();

        [SerializeField]
        private List<TValue> values = new List<TValue>();

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count)
                throw new System.Exception(
                    string.Format(
                        "there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable.",
                        keys.Count,
                        values.Count
                    )
                );

            for (int i = 0; i < keys.Count; i++)
                this.Add(keys[i], values[i]);
        }
    }

    [System.Serializable]
    public class DictionaryStringStandInfo : SerializableDictionary<string, StandInfo> { }

    [System.Serializable]
    public class StandInfo
    {
        public StandType Type;
        public GameObject ReferencedStand;
    }

    public class StandPackInfo
    {
        public GameObject ReferencedStandPack;
        public Pack Type;
    }

    /// <summary>
    ///     Holds customization data assigned to a particular stand.
    /// </summary>
    public class StandCustomizationData
    {
        public string standName;
        public string logoDownloadURL;
        public string meetLink;
        public string pdfLink;
        public string website;
        public string videoURL;
        public string shortDescription;
        public string phoneNumber;
        public string address;
        public string standPosition;
        public string standID; // Not to confuse this with StandPosition! This is assigned from the _id from customization JSON data
    }

    [System.Serializable]
    public class Dimension
    {
        public int width;
        public int height;
    }

    interface IUIRequest
    {
        public bool IsShowUIRequestEnabled();
        public void InitializeUIRequest();
        public void DestroyUIRequest();
    }

    /// <summary>
    ///     Interface for getting child stand elements from stand packs.
    ///     For packs spawned on the right side, CCW is followed. CW order is followed for the other side.
    /// </summary>
    public interface IPackStands
    {
        /// <summary>
        ///     Returns information about the child stand.
        /// </summary>
        /// <param name="number">Identifier for the child stand. See convention on how child stands are numbered.</param>
        /// <returns>Information about the child stand if it exists whithin this pack, NULL otherwise.</returns>
        public StandInfo GetChildStand(int number);
    }

    public interface ICustomizableObject
    {
        public CamControllerSettings GetCamControllerSettings();
        public GameObject GetCustomizationUI();
        public GameObject GetPreviewUI();
    }

    public enum Pack
    {
        PACK00,
        PACK01,
        PACK02,
        PACK03
    }

    public enum Layout
    {
        SMALL,
        MEDIUM,
        LARGE
    }

    public enum EnterTrigger
    {
        ENTER_EXHIBITION,
        ENTER_ENTRANCE,
        ENTER_AUDITORIUM,
        EXIT
    }

    public enum StandType
    {
        XL,
        LL,
        LR,
        M,
        S
    }

    public enum UIMapMode
    {
        ZOOMED_IN,
        ZOOMED_OUT
    }

    public enum StandAction
    {
        INTERACTION,
        WEBSITE,
        BROCHURE,
        MEET,
        WATCH_VIDEO
    }

    public enum ButtonSprite
    {
        KEYBOARD_ENTER,
        KEYBOARD_SPACE,
        KEYBOARD_ESC,
        KEYBOARD_V,
        KEYBOARD_E,
        KEYBOARD_M,
        KEYBOARD_WASD,
        KEYBOARD_ARROWS,
        KEYBOARD,
        MOUSE
    }

    public enum CustomizableArea
    {
        STAND_XL_AREA_00,
        STAND_XL_AREA_01,
        STAND_XL_AREA_02,
        STAND_XL_AREA_03,
        STAND_LL_AREA_00,
        STAND_LL_AREA_01,
        STAND_LL_AREA_02,
        STAND_LR_AREA_00,
        STAND_LR_AREA_01,
        STAND_LR_AREA_02,
        STAND_M_AREA_00,
        STAND_M_AREA_01,
        STAND_S_AREA_00,
        STAND_S_AREA_01,
        XL_BANNER_00_AREA_00,
        XL_BANNER_00_AREA_01,
        XL_BANNER_01_AREA_00,
        XL_BANNER_01_AREA_01,
        XL_BANNER_02_AREA_00,
        XL_BANNER_03_AREA_00,
        XL_BANNER_03_AREA_01,
        LANDR_BANNER_00_AREA_00,
        LANDR_BANNER_01_AREA_00,
        LANDR_BANNER_02_AREA_00,
        M_BANNER_00_AREA_00,
        M_BANNER_01_AREA_00,
        SPONSOR_BANNER_00_AREA_00,
        SPONSOR_CYLINDER_00_AREA_00,
        SPONSOR_DISK_AREA_00,
        SPONSOR_DISK_AREA_01,
        SPONSOR_DISK_AREA_02,
        SPONSOR_DISK_AREA_03,
        SPONSOR_CUBE_SCREEN_AREA_00,
        SPONSOR_CUBE_SCREEN_AREA_01,
        SPONSOR_CUBE_SCREEN_AREA_02,
        SPONSOR_CUBE_SCREEN_AREA_03,
        SPONSOR_SCREEN_AREA_00
    }

    public enum Gender
    {
        MALE,
        FEMALE,
        NON_BINARY,
        OTHER
    }

    public struct BannerInfo
    {
        public int Type;
        public GameObject ReferencedBanner;
    }

    /// <summary>
    ///     These settings control the behaviour of a camera controller.
    /// </summary>
    [System.Serializable]
    public class CamControllerSettings
    {
        public Transform rotateAround;
        public float distanceToRotateAround;
        public float rotationSpeed; // in degrees per second
        public float minHorizontalAngle = Mathf.NegativeInfinity;
        public float maxHorizontalAngle = Mathf.Infinity;
        public float minVerticalAngle = Mathf.NegativeInfinity;
        public float maxVerticalAngle = Mathf.Infinity;
    }

    public class RequestData
    {
        public RequestData(
            GameObject _customizable,
            string _url,
            bool _shareableMaterial = false,
            bool _isComponentImageUI = false
        )
        {
            Customizable = _customizable;
            url = _url;
            shareableMaterial = _shareableMaterial;
            isComponentImgaeUI = _isComponentImageUI;
        }

        public GameObject Customizable;
        public string url;
        public bool shareableMaterial;
        public bool isComponentImgaeUI;
    }
}

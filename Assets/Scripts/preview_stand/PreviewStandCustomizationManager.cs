using Common;
using DesignPatterns;
using UnityEngine;
using System.Runtime.InteropServices;
using JSONContainer;
using Utils;

public class PreviewStandCustomizationManager : Singleton<PreviewStandCustomizationManager>
{
    public bool isThisCustomizable = true;

    public GameObject XLStand;
    public GameObject LLStand;
    public GameObject LRStand; public GameObject MStand;
    public GameObject SStand;

    public GameObject[] XLBanners = new GameObject[4];
    public GameObject[] LLBanners = new GameObject[3];
    public GameObject[] LRBanners = new GameObject[3];
    public GameObject[] MBanners = new GameObject[2];

    public GameObject[] XLFurniture = new GameObject[4];
    public GameObject[] LLFurniture = new GameObject[3];
    public GameObject[] LRFurniture = new GameObject[3];

    public GameObject XLTV;
    public GameObject LRTV;
    public GameObject LLTV;

    public GameObject[] Characters = new GameObject[18];
    public StandRelatedData StandRelatedDataSO;

    StandDataContainer standCustomizationData;
    string mediaStaticURL;

    StandInfo instantiatedStandInfo;
    BannerInfo instantiatedBannerInfo;

    GameObject instantiatedCharacter00;
    GameObject instantiatedCharacter01;
    int currentCharacter00Index = 0;
    int currentCharacter01Index = 0;

    private void OnEnable()
    {
        PreviewStandUIManager.Instance.CharacterChangeEvent += OnCharacterChange;
    }

    void Start()
    {
         standCustomizationData = CustomizationManager.Instance.standCustomizationData;
         mediaStaticURL = CustomizationManager.Instance.mediaStaticURL;

         InstantiateStand();
         CustomizeStand();
         TextureRequestManager.Instance.StartServingTextureRequests();
         if (isThisCustomizable) MakeStandCustomizable();
    }

    private void OnDisable()
    {
        if (PreviewStandUIManager.Instance != null)
        {
            PreviewStandUIManager.Instance.CharacterChangeEvent -= OnCharacterChange;
        }
    }

    /// <summary>
    ///     Instantiates the appropriate stand according to the type provided in customization data.
    ///     Make sure to call this before CustomizeStand!
    /// </summary>
    private void InstantiateStand()
    {
        switch (standCustomizationData.Type.ToUpper())
        {
            case "XL":
                instantiatedStandInfo = new StandInfo { ReferencedStand = Instantiate<GameObject>(XLStand), Type = StandType.XL};
                PreviewStandCamController.Instance.SetXLCamProperties();
                break;

            case "LL": 
                instantiatedStandInfo = new StandInfo { ReferencedStand = Instantiate<GameObject>(LLStand), Type = StandType.LL };
                PreviewStandCamController.Instance.SetLLCamProperties();
                break;

            case "LR": 
                instantiatedStandInfo = new StandInfo { ReferencedStand = Instantiate<GameObject>(LRStand), Type = StandType.LR };
                PreviewStandCamController.Instance.SetLRCamProperties();
                break;

            case "M": 
                instantiatedStandInfo = new StandInfo { ReferencedStand = Instantiate<GameObject>(MStand), Type = StandType.M };
                PreviewStandCamController.Instance.SetMCamProperties();
                break;

            case "S": 
                instantiatedStandInfo = new StandInfo { ReferencedStand = Instantiate<GameObject>(SStand), Type = StandType.S };
                PreviewStandCamController.Instance.SetSCamProperties();
                break;

            default: 
                break;
        }
    }

    /// <summary>
    ///     Customizes previously instantiated stand according to provided customization data.
    /// </summary>
    private void CustomizeStand()
    {
        // stand texture request and assignement
        TextureRequestManager.Instance.AddRequest(new RequestData
        (
            instantiatedStandInfo.ReferencedStand,
            Url.GenerateURL(
                mediaStaticURL,
                standCustomizationData.TextureDownloadUrl,
                useCache: false
                )
        ));

        // banner customization
        if (standCustomizationData.Banner.Purchased)
        {
            switch (instantiatedStandInfo.Type)
            {
                case StandType.XL:
                    instantiatedBannerInfo = new BannerInfo
                    {
                        ReferencedBanner = Instantiate<GameObject>(XLBanners[standCustomizationData.Banner.BannerType], instantiatedStandInfo.ReferencedStand.transform),
                        Type = standCustomizationData.Banner.BannerType
                    };
                    break;

                case StandType.LL:
                    instantiatedBannerInfo = new BannerInfo
                    {
                        ReferencedBanner = Instantiate<GameObject>(LLBanners[standCustomizationData.Banner.BannerType], instantiatedStandInfo.ReferencedStand.transform),
                        Type = standCustomizationData.Banner.BannerType
                    };
                    break;

                case StandType.LR:
                    instantiatedBannerInfo = new BannerInfo
                    {
                        ReferencedBanner = Instantiate<GameObject>(LRBanners[standCustomizationData.Banner.BannerType], instantiatedStandInfo.ReferencedStand.transform),
                        Type = standCustomizationData.Banner.BannerType
                    };
                    break;

                case StandType.M:
                    instantiatedBannerInfo = new BannerInfo
                    {
                        ReferencedBanner = Instantiate<GameObject>(MBanners[standCustomizationData.Banner.BannerType], instantiatedStandInfo.ReferencedStand.transform),
                        Type = standCustomizationData.Banner.BannerType
                    };
                    break;

                default:
                    break;
            }
            TextureRequestManager.Instance.AddRequest(new RequestData
            (
                instantiatedBannerInfo.ReferencedBanner,
                Url.GenerateURL(
                    mediaStaticURL,
                    standCustomizationData.Banner.TextureDownloadUrl,
                    useCache: false
                )
            ));
        }

        // initializing stand character indexes
        currentCharacter00Index = standCustomizationData.CaracterType00;
        currentCharacter01Index = standCustomizationData.CaracterType01;

        // stand character customization
        InstantiateStandCharacter();


        // furniture and TV customization
        switch (instantiatedStandInfo.Type)
        {

            case StandType.XL:
                if (standCustomizationData.Furniture.Purchased) Instantiate<GameObject>(XLFurniture[standCustomizationData.Furniture.FurnitureType], instantiatedStandInfo.ReferencedStand.transform);
                if (standCustomizationData.Tv.Purchased) Instantiate<GameObject>(XLTV, instantiatedStandInfo.ReferencedStand.transform);
                break;

            case StandType.LL:
                if (standCustomizationData.Furniture.Purchased) Instantiate<GameObject>(LLFurniture[standCustomizationData.Furniture.FurnitureType], instantiatedStandInfo.ReferencedStand.transform);
                if (standCustomizationData.Tv.Purchased) Instantiate<GameObject>(LLTV, instantiatedStandInfo.ReferencedStand.transform);
                break;

            case StandType.LR:
                if (standCustomizationData.Furniture.Purchased) Instantiate<GameObject>(LRFurniture[standCustomizationData.Furniture.FurnitureType], instantiatedStandInfo.ReferencedStand.transform);
                if (standCustomizationData.Tv.Purchased) Instantiate<GameObject>(LRTV, instantiatedStandInfo.ReferencedStand.transform);
                break;
            default:
                break;
        }

    }
    /// <summary>
    ///     Update stand character when UI instantiates CharacterChangeEvent
    /// </summary>
    /// <param name="position">00: LEFT, 01: RIGHT</param>
    /// <param name="index_delta_update">this will be added to the current character index</param>
    public void OnCharacterChange(int position, int index_delta_update)
    {
        if (position == 00) // LEFT SIDE OR SOLO STAND CHARACTER
        {
            int index_to_be_assigned = currentCharacter00Index + index_delta_update;
            if ((index_to_be_assigned < Characters.Length) && (index_to_be_assigned + index_delta_update >= 0))
            {
                currentCharacter00Index = index_to_be_assigned;
                Destroy(instantiatedCharacter00);
                InstantiateStandCharacter();
// invoking a character change event for Frontend React application
#if UNITY_WEBGL == true && UNITY_EDITOR == false
                CustomizeCharacterEvent(position, currentCharacter00Index);
#endif
            }

        } 
        else    // RIGHT SIDE
        {
            int index_to_be_assigned = currentCharacter01Index + index_delta_update;
            if ((index_to_be_assigned < Characters.Length) && (index_to_be_assigned + index_delta_update >= 0))
            {
                currentCharacter01Index = index_to_be_assigned;
                Destroy(instantiatedCharacter01);
                InstantiateStandCharacter();
// invoking a character change event for Frontend React application
#if UNITY_WEBGL == true && UNITY_EDITOR == false    
                CustomizeCharacterEvent(position, currentCharacter01Index);
#endif
            }

        }

    }

    /// <summary>
    ///     Call this from frontEnd React application after updating the texture to refresh stand's texture.
    /// </summary>
    public void RefreshStandTexture()
    {
        if (instantiatedStandInfo.ReferencedStand != null)
        {
            // re-request and assign stand texture
            TextureRequestManager.Instance.AddRequest(new RequestData
            (
                instantiatedStandInfo.ReferencedStand,
                Url.GenerateURL(
                    mediaStaticURL,
                    standCustomizationData.TextureDownloadUrl,
                    useCache: false
                )
            ));
            TextureRequestManager.Instance.StartServingTextureRequests();
        }
    }

    /// <summary>
    ///     Call this from frontEnd React application after updating the texture to refresh banner's texture.
    /// </summary>
    public void RefreshBannerTexture()
    {
        if (instantiatedBannerInfo.ReferencedBanner != null)
        {
            TextureRequestManager.Instance.AddRequest(new RequestData
            (
                instantiatedBannerInfo.ReferencedBanner,
                Url.GenerateURL(
                    mediaStaticURL,
                    standCustomizationData.Banner.TextureDownloadUrl,
                    useCache: false
                )
            ));
            TextureRequestManager.Instance.StartServingTextureRequests();
        }
    }

    /// <summary>
    ///     Instantiates stand character(s) according to character indexes properties.
    ///     Make sure to initialize these indexes before calling this function
    /// </summary>
    private void InstantiateStandCharacter()
    {
        // furniture, TV and character(s) customization
        switch (instantiatedStandInfo.Type)
        {

            case StandType.XL:
                if (instantiatedCharacter00 != null) Destroy(instantiatedCharacter00);
                (instantiatedCharacter00 = Instantiate<GameObject>(Characters[currentCharacter00Index], instantiatedStandInfo.ReferencedStand.transform))
                    .transform.localPosition = StandRelatedDataSO.XLLeftCharacterPos;

                if (instantiatedCharacter01 != null) Destroy(instantiatedCharacter01);
                (instantiatedCharacter01 = Instantiate<GameObject>(Characters[currentCharacter01Index], instantiatedStandInfo.ReferencedStand.transform))
                    .transform.localPosition = StandRelatedDataSO.XLRightCharacterPos;
                break;

            case StandType.LL:
                if (instantiatedCharacter00 != null) Destroy(instantiatedCharacter00);
                (instantiatedCharacter00 = Instantiate<GameObject>(Characters[currentCharacter00Index], instantiatedStandInfo.ReferencedStand.transform))
                    .transform.localPosition = StandRelatedDataSO.LLCharacterPos;
                break;

            case StandType.LR:
                if (instantiatedCharacter00 != null) Destroy(instantiatedCharacter00);
                (instantiatedCharacter00 = Instantiate<GameObject>(Characters[currentCharacter00Index], instantiatedStandInfo.ReferencedStand.transform))
                    .transform.localPosition = StandRelatedDataSO.LRCharacterPos;
                break;

            case StandType.M:
                if (instantiatedCharacter00 != null) Destroy(instantiatedCharacter00);
                (instantiatedCharacter00 = Instantiate<GameObject>(Characters[currentCharacter00Index], instantiatedStandInfo.ReferencedStand.transform))
                    .transform.localPosition = StandRelatedDataSO.MCharacterPos;
                break;

            case StandType.S:
                if (instantiatedCharacter00 != null) Destroy(instantiatedCharacter00);
                (instantiatedCharacter00 = Instantiate<GameObject>(Characters[currentCharacter00Index], instantiatedStandInfo.ReferencedStand.transform))
                    .transform.localPosition = StandRelatedDataSO.SCharacterPos;
                break;
            default:
                break;
        }
    }

    /// <summary>
    ///     Call this function to make the preview of this stand interactive thus user can customize
    ///     their stand using customization UI in the preview window.
    /// </summary>
    private void MakeStandCustomizable()
    {
        PreviewStandUIManager.Instance.InstantiateUIStand(instantiatedStandInfo, instantiatedBannerInfo);
    }

    [DllImport("__Internal")] private static extern void CustomizeCharacterEvent(int position, int character_id);
}

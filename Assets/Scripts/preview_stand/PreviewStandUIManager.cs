using DesignPatterns;
using Common;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreviewStandUIManager : SafeSingleton<PreviewStandUIManager>
{
    public delegate void CharacterChangeHandler(int position, int index_delta_update);
    public event CharacterChangeHandler CharacterChangeEvent;


    public GameObject UIXLStand;
    public GameObject UILLStand;
    public GameObject UILRStand;
    public GameObject UIMStand;
    public GameObject UISStand;

    public GameObject[] UIXLBanners = new GameObject[4];
    public GameObject[] UILANDRBanners = new GameObject[3];
    public GameObject[] UIMBanners = new GameObject[2];

    public Button PreviewButton;
    public TMP_Text PreviewButtonText;

    GameObject instantiatedUIStand = null;
    GameObject instantiatedUIBanner = null;

    bool isInPreviewMode;

    private void OnEnable()
    {
        PreviewButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (PreviewStandCustomizationManager.Instance.isThisCustomizable)
        {
            PreviewButton.gameObject.SetActive(true);
            PreviewButton.onClick.AddListener(OnPreviewButtonClick);
        }

        isInPreviewMode = false;
    }

    private void OnDestroy()
    {
        if (instantiatedUIStand != null)    // if UI stand is instantiated, unsubscribe from character change event
        {
            instantiatedUIStand.GetComponent<PreviewStandUI>().CharacterChangeEvent -= OnCharacterChange;
        }
        
    }

    public void InstantiateUIStand(StandInfo standInfo, BannerInfo bannerInfo)
    {
        if (standInfo.ReferencedStand == null) return;
        switch (standInfo.Type)
        {
            case StandType.XL:
                instantiatedUIStand = Instantiate<GameObject>(UIXLStand, standInfo.ReferencedStand.transform);
                break;

            case StandType.LL:
                instantiatedUIStand = Instantiate<GameObject>(UILLStand, standInfo.ReferencedStand.transform);
                break;

            case StandType.LR:
                instantiatedUIStand = Instantiate<GameObject>(UILRStand, standInfo.ReferencedStand.transform);
                break;

            case StandType.M:
                instantiatedUIStand = Instantiate<GameObject>(UIMStand, standInfo.ReferencedStand.transform);
                break;

            case StandType.S:
                instantiatedUIStand = Instantiate<GameObject>(UISStand, standInfo.ReferencedStand.transform);
                break;

            default:
                break;
        }

        // subscribing to character change event
        instantiatedUIStand.GetComponent<PreviewStandUI>().CharacterChangeEvent += OnCharacterChange;

        if (bannerInfo.ReferencedBanner != null)
        {
            switch (standInfo.Type)
            {
                case StandType.XL:
                    instantiatedUIBanner = Instantiate<GameObject>(UIXLBanners[bannerInfo.Type], bannerInfo.ReferencedBanner.transform);
                    break;

                case StandType.LR:
                case StandType.LL:
                    instantiatedUIBanner = Instantiate<GameObject>(UILANDRBanners[bannerInfo.Type], bannerInfo.ReferencedBanner.transform);
                    break;

                case StandType.M:
                    instantiatedUIBanner = Instantiate<GameObject>(UIMBanners[bannerInfo.Type], bannerInfo.ReferencedBanner.transform);
                    break;

                default:
                    break;
            }
        }

    }

    public void ShowStandCustomizationUI()
    {
        if (instantiatedUIStand == null) return;
        instantiatedUIStand.SetActive(true);

        if (instantiatedUIBanner == null) return;
        instantiatedUIBanner.SetActive(true);
    }
    public void HideStandCustomizationUI()
    {
        if (instantiatedUIStand == null) return;
        instantiatedUIStand.SetActive(false);

        if (instantiatedUIBanner == null) return;
        instantiatedUIBanner.SetActive(false);
    }

    public void OnPreviewButtonClick()
    {
        if (!isInPreviewMode)
        {
            HideStandCustomizationUI();
            PreviewButtonText.text = "Personnaliser";
            isInPreviewMode = true;
            return;
        }
        ShowStandCustomizationUI();
        PreviewButtonText.text = "Visualiser";
        isInPreviewMode = false;
    }

    public void OnCharacterChange(int position, int index_delta_update)
    {
        if (CharacterChangeEvent != null)
        {
            CharacterChangeEvent.Invoke(position, index_delta_update);
            return;
        }
    }
}

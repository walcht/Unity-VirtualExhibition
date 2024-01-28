using System.Runtime.InteropServices;
using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStand : MonoBehaviour
{
    public InputEventLayer InputLayer;
    public Image StandMenuLogo;
    public Button WebsiteButton;
    public Button BrochureButton;
    public Button MeetButton;
    public Button UploadButton;
    public Button VideoButton;
    public Button ReturnButton;
    public TMP_Text StandName;
    public TMP_Text Description;
    public TMP_Text PhoneNumber;
    public TMP_Text Address;

    [DllImport("__Internal")]
    private static extern void UploadPDFEvent(string id_stand);

    private StandCustomizationData _standCustomizationData;

    void Awake()
    {
        WebsiteButton.onClick.AddListener(OnCheckWebsite);
        MeetButton.onClick.AddListener(OnJoinMeet);
        UploadButton.onClick.AddListener(OnUploadFile);
        BrochureButton.onClick.AddListener(OnCheckPDF);
        ReturnButton.onClick.AddListener(OnReturn);
    }

    void OnDisable()
    {
        _standCustomizationData = null;
        WebsiteButton.interactable = true;
        BrochureButton.interactable = true;
        MeetButton.interactable = true;
        UploadButton.interactable = true;
        VideoButton.interactable = true;
        ReturnButton.interactable = true;
    }

    /// <summary>
    ///     Corrects the navigation between buttons.
    ///     Call this function after enabling buttons interactivity.
    /// </summary>
    private void RefreshButtonsNavigation()
    {
        Navigation WebsiteButtonNav = WebsiteButton.navigation;
        Navigation BrochureButtonNav = BrochureButton.navigation;
        Navigation MeetButtonNav = MeetButton.navigation;
        Navigation UploadButtonNav = UploadButton.navigation;
        Navigation VideoButtonNav = VideoButton.navigation;
        Navigation ReturnButtonNav = ReturnButton.navigation;
        // for each button and for each navigation direction, an ordered list of buttons is provided.
        // the first button in the list that is interactable will be set as navigation target.
        Button[] website_button_select_onRight =
        {
            BrochureButton,
            MeetButton,
            UploadButton,
            VideoButton
        };
        Button[] website_button_select_onLeft =
        {
            VideoButton,
            UploadButton,
            MeetButton,
            BrochureButton
        };
        Button[] brochure_button_select_onRight =
        {
            MeetButton,
            UploadButton,
            VideoButton,
            WebsiteButton
        };
        Button[] brochure_button_select_onLeft =
        {
            WebsiteButton,
            VideoButton,
            UploadButton,
            MeetButton
        };
        Button[] meet_button_select_onRight =
        {
            UploadButton,
            VideoButton,
            WebsiteButton,
            BrochureButton
        };
        Button[] meet_button_select_onLeft =
        {
            BrochureButton,
            WebsiteButton,
            VideoButton,
            UploadButton
        };
        Button[] upload_button_select_onRight =
        {
            VideoButton,
            WebsiteButton,
            BrochureButton,
            MeetButton
        };
        Button[] upload_button_select_onLeft =
        {
            MeetButton,
            BrochureButton,
            WebsiteButton,
            VideoButton
        };
        Button[] video_button_select_onRight =
        {
            WebsiteButton,
            BrochureButton,
            MeetButton,
            UploadButton
        };
        Button[] video_button_select_onLeft =
        {
            UploadButton,
            MeetButton,
            BrochureButton,
            WebsiteButton
        };
        Button[] return_button_select_onUp =
        {
            WebsiteButton,
            BrochureButton,
            MeetButton,
            UploadButton,
            VideoButton
        };
        Button[] return_button_select_onDown =
        {
            VideoButton,
            UploadButton,
            MeetButton,
            BrochureButton,
            WebsiteButton
        };
        for (int i = 0; i < website_button_select_onRight.Length; i++)
        {
            if (website_button_select_onRight[i].interactable)
            {
                WebsiteButtonNav.selectOnRight = website_button_select_onRight[i];
                break;
            }
        }
        for (int i = 0; i < website_button_select_onLeft.Length; i++)
        {
            if (website_button_select_onLeft[i].interactable)
            {
                WebsiteButtonNav.selectOnLeft = website_button_select_onLeft[i];
                break;
            }
        }
        for (int i = 0; i < brochure_button_select_onRight.Length; i++)
        {
            if (brochure_button_select_onRight[i].interactable)
            {
                BrochureButtonNav.selectOnRight = brochure_button_select_onRight[i];
                break;
            }
        }
        for (int i = 0; i < brochure_button_select_onLeft.Length; i++)
        {
            if (brochure_button_select_onLeft[i].interactable)
            {
                BrochureButtonNav.selectOnLeft = brochure_button_select_onLeft[i];
                break;
            }
        }
        for (int i = 0; i < meet_button_select_onRight.Length; i++)
        {
            if (meet_button_select_onRight[i].interactable)
            {
                MeetButtonNav.selectOnRight = meet_button_select_onRight[i];
                break;
            }
        }
        for (int i = 0; i < meet_button_select_onLeft.Length; i++)
        {
            if (meet_button_select_onLeft[i].interactable)
            {
                MeetButtonNav.selectOnLeft = meet_button_select_onLeft[i];
                break;
            }
        }
        for (int i = 0; i < upload_button_select_onRight.Length; i++)
        {
            if (upload_button_select_onRight[i].interactable)
            {
                UploadButtonNav.selectOnRight = upload_button_select_onRight[i];
                break;
            }
        }
        for (int i = 0; i < upload_button_select_onLeft.Length; i++)
        {
            if (upload_button_select_onLeft[i].interactable)
            {
                UploadButtonNav.selectOnLeft = upload_button_select_onLeft[i];
                break;
            }
        }
        for (int i = 0; i < video_button_select_onRight.Length; i++)
        {
            if (video_button_select_onRight[i].interactable)
            {
                VideoButtonNav.selectOnRight = video_button_select_onRight[i];
                break;
            }
        }
        for (int i = 0; i < video_button_select_onLeft.Length; i++)
        {
            if (video_button_select_onLeft[i].interactable)
            {
                VideoButtonNav.selectOnLeft = video_button_select_onLeft[i];
                break;
            }
        }
        // setting return button navigation
        for (int i = 0; i < return_button_select_onUp.Length; i++)
        {
            if (return_button_select_onUp[i].interactable)
            {
                ReturnButtonNav.selectOnUp = return_button_select_onUp[i];
                break;
            }
        }
        for (int i = 0; i < return_button_select_onDown.Length; i++)
        {
            if (return_button_select_onDown[i].interactable)
            {
                ReturnButtonNav.selectOnDown = return_button_select_onDown[i];
                break;
            }
        }
        WebsiteButton.navigation = WebsiteButtonNav;
        BrochureButton.navigation = BrochureButtonNav;
        MeetButton.navigation = MeetButtonNav;
        UploadButton.navigation = UploadButtonNav;
        VideoButton.navigation = VideoButtonNav;
        ReturnButton.navigation = ReturnButtonNav;
        // Select first active button if any else select Return
        ReturnButton.Select();
        foreach (var btn in return_button_select_onUp)
        {
            if (btn.interactable)
            {
                btn.Select();
                break;
            }
        }
    }

    public StandCustomizationData StandData
    {
        private get { return _standCustomizationData; }
        set
        {
            _standCustomizationData = value;
            if (string.IsNullOrWhiteSpace(value.website))
                WebsiteButton.interactable = false;
            if (string.IsNullOrWhiteSpace(value.pdfLink))
                BrochureButton.interactable = false;
            if (string.IsNullOrWhiteSpace(value.meetLink))
                MeetButton.interactable = false;
            if (string.IsNullOrWhiteSpace(value.videoURL))
                VideoButton.interactable = false;
            // if a logo texture is available use it, otherwise set the Logo to its default texture
            if (!string.IsNullOrWhiteSpace(value.logoDownloadURL))
            {
                TextureRequestManager.Instance.AddRequest(
                    new RequestData(
                        StandMenuLogo.gameObject,
                        CustomizationManager.Instance.mediaStaticURL + value.logoDownloadURL,
                        false,
                        true
                    )
                );
                TextureRequestManager.Instance.StartServingTextureRequests();
            }
            if (!string.IsNullOrWhiteSpace(value.standName))
                StandName.text = value.standName;
            else
                StandName.text = string.Empty;
            if (!string.IsNullOrWhiteSpace(value.shortDescription))
                Description.text = value.shortDescription;
            else
                Description.text = "Pas de description";
            if (!string.IsNullOrWhiteSpace(value.phoneNumber))
                PhoneNumber.text = value.phoneNumber;
            else
                PhoneNumber.text = "Non disponible";
            if (!string.IsNullOrWhiteSpace(value.address))
                Address.text = value.address;
            else
                Address.text = "Non disponible";
            RefreshButtonsNavigation();
        }
    }

    void OnCheckWebsite()
    {
        Application.OpenURL(_standCustomizationData.website);
        StatisticsManager.Instance.OnStandActionPerformed(
            _standCustomizationData.standID,
            StandAction.WEBSITE
        );
    }

    void OnJoinMeet()
    {
        Application.OpenURL(_standCustomizationData.meetLink);
        StatisticsManager.Instance.OnStandActionPerformed(
            _standCustomizationData.standID,
            StandAction.MEET
        );
    }

    void OnCheckPDF()
    {
        Application.OpenURL(_standCustomizationData.pdfLink);
        StatisticsManager.Instance.OnStandActionPerformed(
            _standCustomizationData.standID,
            StandAction.BROCHURE
        );
    }

    void OnReturn()
    {
        InputLayer.InvokeUIStandExitEvent();
    }

    void OnUploadFile()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
           UploadPDFEvent(_standCustomizationData.standPosition);     
#endif
    }

    void OnWatchVideo()
    {
        StatisticsManager.Instance.OnStandActionPerformed(
            _standCustomizationData.standID,
            StandAction.WATCH_VIDEO
        );
    }
}

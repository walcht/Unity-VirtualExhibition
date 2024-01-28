using System.Collections;
using UnityEngine;
using DesignPatterns;
using UnityEngine.UI;
using JSONContainer;
using TMPro;
using System.Runtime.InteropServices;
using Common;

public class PreviewStandMenuManager : Singleton<PreviewStandMenuManager>
{
    public GameObject       ChildStandMenu;                   // references stand menu that is CHILD of THIS gameObject
    public Image            StandMenuLogo;

    public Button           WebsiteButton;
    public Button           BrochureButton;
    public Button           MeetButton;
    public Button           UploadButton;
    public Button           ReturnButton;

    public TMP_Text         Description;

    public TMP_Text         PhoneTextField;
    public TMP_Text         AddressTextField;

    public TMP_Text         CompanyName;

    string mediaStaticURL;
    MenuRelatedDataContainer             menuCustomizationData;

    [DllImport("__Internal")] private static extern void UploadCVEvent();
    [DllImport("__Internal")] private static extern void ExitPreviewStandMenuEvent();


    private void OnEnable()
    {
        ChildStandMenu.SetActive(false);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        menuCustomizationData = CustomizationManager.Instance.standMenuCustomizationData;
        mediaStaticURL = CustomizationManager.Instance.mediaStaticURL;

        StartCoroutine(Init());
    }


    public void OnCheckWebsite()
    {
        Application.OpenURL(menuCustomizationData.Menu.Website);
    }

    public void OnJoinMeet()
    {
        Application.OpenURL(menuCustomizationData.Menu.MeetLink);
    }

    public void OnCheckPDF()
    {
        Application.OpenURL(menuCustomizationData.Menu.PdfDownloadUrl);
    }

    public void OnReturn()
    {
        // maybe send an event to FrontEnd React application?
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ExitPreviewStandMenuEvent();
#endif
    }

    public void OnUploadFile()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        UploadCVEvent();
#endif
    }
    public void OnWatchVideo()
    {

    }

    /// <summary>
    ///     Start this coroutine to initialize customization process.
    ///     This is done in a coroutine for synchronization reasons.
    /// </summary>
    /// <returns>Checks each frame whether JSON has been initialized before continuing the initialization process.</returns>
    IEnumerator Init()
    {
        // No need for the return button to be interactable
        ReturnButton.interactable = false;

        if (menuCustomizationData != null)
        {
            if (menuCustomizationData.Menu != null)
            {
                // disabling buttons whose customization data isn't provided
                if (menuCustomizationData.Menu.Website == null) WebsiteButton.interactable = false;
                if (menuCustomizationData.Menu.PdfDownloadUrl == null) BrochureButton.interactable = false;
                if (menuCustomizationData.Menu.MeetLink == null) MeetButton.interactable = false;

                // setting description, phone number and address
                if (menuCustomizationData.Menu.Description != null) Description.text = menuCustomizationData.Menu.Description;
                if (menuCustomizationData.Menu.PhoneNumber != null) PhoneTextField.text = menuCustomizationData.Menu.PhoneNumber;
                if (menuCustomizationData.Menu.Address != null) AddressTextField.text = menuCustomizationData.Menu.Address;

            }

            // if a logo texture is available use it, otherwise set the Logo to its default texture
            if (menuCustomizationData.Logo != null)
            {
                TextureRequestManager.Instance.AddRequest(new RequestData
                    (
                        StandMenuLogo.gameObject,
                        mediaStaticURL + menuCustomizationData.Logo,
                        false,
                        true
                    )
                );

                TextureRequestManager.Instance.StartServingTextureRequests();
            }

            if (menuCustomizationData.StandName != null) CompanyName.text = menuCustomizationData.StandName;

            // enabling\showing stand interation menu
            ChildStandMenu.SetActive(true);

            // refreshing buttons navigation
            RefreshButtonsNavigation();
            
        }
        else
        {
            Debug.LogError("Stand interaction menu customization data isn\'t set up properly.");
        }

        yield return null;

    }

    /// <summary>
    ///     Corrects the navigation between buttons.
    ///     Call this function after enabling\disabling buttons interactability.
    /// </summary>
    void RefreshButtonsNavigation()
    {
        Navigation WebsiteButtonNav     = WebsiteButton.navigation;
        Navigation BrochureButtonNav    = BrochureButton.navigation;
        Navigation MeetButtonNav        = MeetButton.navigation;
        Navigation UploadButtonNav      = UploadButton.navigation;
        Navigation ReturnButtonNav      = ReturnButton.navigation;

        // for each button and for each navigation direction, an ordered list of buttons is provided.
        // the first button in the list that is interactable will be set as navigation target.
        Button[]    website_button_select_onRight       =   { BrochureButton, MeetButton, UploadButton};
        Button[]    website_button_select_onLeft        =   { UploadButton, MeetButton, BrochureButton};

        Button[]    brochure_button_select_onRight      =   { MeetButton, UploadButton, WebsiteButton};
        Button[]    brochure_button_select_onLeft       =   { WebsiteButton, UploadButton, MeetButton};

        Button[]    meet_button_select_onRight          =   { UploadButton, WebsiteButton, BrochureButton};
        Button[]    meet_button_select_onLeft           =   { BrochureButton, WebsiteButton, UploadButton};

        Button[]    upload_button_select_onRight        =   { WebsiteButton, BrochureButton, MeetButton};
        Button[]    upload_button_select_onLeft         =   { MeetButton, BrochureButton, WebsiteButton};

        Button[]    video_button_select_onRight         =   { WebsiteButton, BrochureButton, MeetButton, UploadButton};
        Button[]    video_button_select_onLeft          =   { UploadButton, MeetButton, BrochureButton, WebsiteButton};

        Button[] return_button_select_onRight           = { WebsiteButton, BrochureButton, MeetButton, UploadButton};
        Button[] return_button_select_onLeft            = { UploadButton, MeetButton, BrochureButton, WebsiteButton};

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

        // setting return button navigation
        for (int i = 0; i < return_button_select_onRight.Length; i++)
        {
            if (return_button_select_onRight[i].interactable)
            {
                ReturnButtonNav.selectOnRight = return_button_select_onRight[i];
                break;
            }
        }

        for (int i = 0; i < return_button_select_onLeft.Length; i++)
        {
            if (return_button_select_onLeft[i].interactable)
            {
                ReturnButtonNav.selectOnLeft = return_button_select_onLeft[i];
                break;
            }
        }

        WebsiteButton.navigation    = WebsiteButtonNav;
        BrochureButton.navigation   = BrochureButtonNav;
        MeetButton.navigation       = MeetButtonNav;
        UploadButton.navigation     = UploadButtonNav;
        ReturnButton.navigation     = ReturnButtonNav;
    }

    public void OnNavigateToReturnButton() {return;}
}

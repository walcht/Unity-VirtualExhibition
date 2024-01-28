using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using Common;
using UnityEngine.UI;
using TMPro;

public class PreviewUIManager : SafeSingleton<PreviewUIManager>
{
    public bool isThisCustomizable = true;
    public Button PreviewButton;                                                // preview toggle, if enabled, customization UI is hidden
    public Button GoBackButton;

    public NavigationButton LeftNavButton;                                          // when clicked, camera moves left
    public NavigationButton RightNavButton;                                         // when clicked, camera moves right

    // UI DIMENSION POPUP
    public UIDimensionPopup UIDimensionPopupComponent;

    public TMPro.TMP_Text PreviewText;

    public PreviewCamSpot defaultCameraSpot;                // default camera spot that the mainCamera will initially follow and a customizable object Preview exit
                                                        // returns to this default camera spot.

    List<GameObject> customizableObjects = new List<GameObject>();
    List<GameObject> cameraSpotUIs = new List<GameObject>();

    bool isCurrentlyInPreviewMode = false;                  // Preview mode disables all non-HUD UI elements for better visibility

    ICustomizableObject currentlyCustomizableObject;             // at a time, at most one customizable object could be zoomed-in on.
    Canvas _currentCameraSpotUI;               // the spot camera UI GameObject where the current mainCamera is at.
                                                    // this UI should be currently disabled until another spotCamera GameObject is chosen.

    TMP_Text _previewButtonText;
    const string _previewActivatedText = "Visualiser";
    const string _previewDisabledText = "Personnaliser";

    private void OnEnable()
    {
        _previewButtonText = PreviewButton.GetComponentInChildren<TMP_Text>();

        if (isThisCustomizable)
        {
            PreviewButton.gameObject.SetActive(true);
            PreviewButton.onClick.AddListener(OnPreviewButtonClick);
        }
        else
        {
            PreviewButton.gameObject.SetActive(false);
        }

        GoBackButton.gameObject.SetActive(false);
        UIDimensionPopupComponent.gameObject.SetActive(false);
    }

    private void Start()
    {
        PreviewCamController.Instance.Settings = defaultCameraSpot.camControllerSettings;
        CurrentCameraSpotUI = defaultCameraSpot.cameraSpotUI;

        HideCustomizationUI();
        //HidePreviewUIs();
        if (isThisCustomizable) ShowPreviewUIs();
    }
    public void AddSpotCameraUI(GameObject spotCameraUI) => cameraSpotUIs.Add(spotCameraUI);
    public void AddCustomizableObject(GameObject customizable) => customizableObjects.Add(customizable);
    public void ShowPreviewUIs() { foreach (var customiazble in customizableObjects) customiazble.GetComponent<ICustomizableObject>().GetPreviewUI().SetActive(true); }
    public void HidePreviewUIs() { foreach (var customiazble in customizableObjects) customiazble.GetComponent<ICustomizableObject>().GetPreviewUI().SetActive(false); }
    public void HideCustomizationUI() { foreach (var customizable in customizableObjects) customizable.GetComponent<ICustomizableObject>().GetCustomizationUI().SetActive(false); }
    public void ShowCameraSpotUIs(bool showAll = false)
    { 
        foreach (var cameraSpotUI in cameraSpotUIs) cameraSpotUI.SetActive(true);
        if (!showAll) _currentCameraSpotUI.gameObject.SetActive(false);
    }
    public void HideCameraSpotUIs() { foreach (var cameraSpotUI in cameraSpotUIs) cameraSpotUI.SetActive(false); }

    public void ShowDimensionPopupUI(int width, int height)
    {
        UIDimensionPopupComponent.gameObject.SetActive(true);
        UIDimensionPopupComponent.SetDimension(height, width);
    }

    public void HideDimensionPopupUI()
    {
        UIDimensionPopupComponent.SetDimension(0, 0);
        UIDimensionPopupComponent.gameObject.SetActive(false);
    }

    public void OnPreviewButtonClick()
    {
        // if we are currently focusing on a customizable object
        if (currentlyCustomizableObject != null)
        {
            if (isCurrentlyInPreviewMode)
            {
                isCurrentlyInPreviewMode = false;
                currentlyCustomizableObject.GetCustomizationUI().SetActive(true);

                _previewButtonText.text = _previewActivatedText;

                return;
            }

            isCurrentlyInPreviewMode = true;
            currentlyCustomizableObject.GetCustomizationUI().SetActive(false);

            _previewButtonText.text = _previewDisabledText;

            return;
        }

        if (isCurrentlyInPreviewMode)
        {
            isCurrentlyInPreviewMode = false;
            ShowPreviewUIs();

            _previewButtonText.text = _previewActivatedText;

            return;
        }

        isCurrentlyInPreviewMode = true;
        HidePreviewUIs();

        _previewButtonText.text = _previewDisabledText;

    }

    /// <summary>
    ///     Called each time a transition (zoom-in) to a customizable object is executed.
    ///     Preview UIs are disabled and the customizable's customizable UI is shown.
    /// </summary>
    /// <param name="customizable">The customizable object to which the transition was executed.</param>
    public void OnEnterCustomizablePreview(ICustomizableObject customizable)
    {
        currentlyCustomizableObject = customizable;
        GoBackButton.gameObject.SetActive(true);

        HideCameraSpotUIs();
        HidePreviewUIs();
        customizable.GetCustomizationUI().SetActive(true);
    }

    /// <summary>
    ///     Called each time a transition (zoom-out) out of a customizable object is executed.
    ///     Preview UIs are enbaled and the customizable's cutomization UI is disabled.
    /// </summary>
    public void OnExitCustomizablePreview()
    {
        currentlyCustomizableObject.GetCustomizationUI().SetActive(false);
        currentlyCustomizableObject = null;

        GoBackButton.gameObject.SetActive(false);

        //_currentCameraSpotUI = defaultCameraSpot;
        PreviewCamController.Instance.Settings = defaultCameraSpot.GetComponent<PreviewCamSpot>().camControllerSettings;

        ShowCameraSpotUIs();
        if (isCurrentlyInPreviewMode)
        {
            isCurrentlyInPreviewMode = false;
            ShowPreviewUIs();

            _previewButtonText.text = _previewActivatedText;
        }
        ShowPreviewUIs();
        
    }


    public Canvas CurrentCameraSpotUI
    {
        get => _currentCameraSpotUI;
        set
        {
            _currentCameraSpotUI?.gameObject.SetActive(true);
            _currentCameraSpotUI = value;
            _currentCameraSpotUI.gameObject.SetActive(false);
        }
    }
}

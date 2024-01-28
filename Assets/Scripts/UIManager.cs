using System;
using System.Collections;
using Common;
using DesignPatterns;
using UnityEngine;

/// <summary>
///     Manages almost all UI elements in all scenes.
/// </summary>
public class UIManager : SafeSingleton<UIManager>
{
    [Tooltip("Scene management Scriptable object layer")]
    public SceneManagementLayer SceneLayer;

    [Tooltip("Input events Scriptable object layer")]
    public InputEventLayer InputLayer;

    [Tooltip("UI child that shows a list of purchased stands")]
    public UIInfoDesk UIInfoDeskChild;

    [Tooltip("UI child that will be shown once user instantiates an interaction with the stand")]
    public UIStand UIStandChild;

    [Tooltip("UI child for showing generic button notifications")]
    public UIButtonNotification UIButtonNotificationChild;

    [Tooltip("UI child for the main menu")]
    public UIMainMenu UIMainMenuChild;

    [Tooltip("UI child for showing list of attendees and their contact information")]
    public UINetworking UINetworkingChild;

    [Tooltip("UI child for showing controls")]
    public UITutorial UITutorialChild;

    [Tooltip("Contains Minimap and additional HUD UI elements")]
    public GameObject UIHud;

    [Tooltip("Minimap UI")]
    public UIMinimap UIMapChild;

    [Tooltip("UI child for showing video entries")]
    public UIAuditorium UIAuditoriumChild;

    [Tooltip("UI child for playing videos (most likely in the Auditorium)")]
    public UIVideoPlayer UIVideoPlayerChild;

    [Tooltip("UI child for showing loading progress")]
    public UILoading UILoadingChild;
    public Texture SExhibitionMap;

    [Tooltip(
        "GTM stands for how much the cursor moves on the minimap for 1 meter of movement in game"
    )]
    public float SExhibitionGTMUnit = 0.009543f;
    public float SExhibitionXOffset = 0.00f;
    public float SExhibitionYOffset = 0.00f;
    public Texture MExhibitionMap;
    public float MExhibitionGTMUnit = 0.008074f;
    public float MExhibitionXOffset = 0.00f;
    public float MExhibitionYOffset = 0.00f;
    public Texture LExhibitionMap;
    public float LExhibitionGTMUnit = 0.006100f;
    public float LExhibitionXOffset = 0.00f;
    public float LExhibitionYOffset = 0.00f;
    public Texture EntranceMap;
    public float EntranceGTMUnit = 0.00928424f;
    public float EntranceXOffset = 0.00f;
    public float EntranceYOffset = 0.00f;

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////// UNITY CALLBACKS //////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    private void OnEnable()
    {
        // initially disable all UIs
        UIHud.SetActive(false);
        UIStandChild.gameObject.SetActive(false);
        UIInfoDeskChild.gameObject.SetActive(false);
        UINetworkingChild.gameObject.SetActive(false);
        UIMainMenuChild.gameObject.SetActive(false);
        UIAuditoriumChild.gameObject.SetActive(false);
        UIVideoPlayerChild.gameObject.SetActive(false);
        UIButtonNotificationChild.gameObject.SetActive(false);
        // scene loading\unloading callbacks
        SceneLayer.ExhibitionSceneLoadedEvent += OnExhibitionSceneLoaded;
        SceneLayer.EntranceSceneLoadedEvent += OnEntranceSceneLoaded;
        SceneLayer.AuditoriumSceneLoadedEvent += OnAuditoriumSceneLoaded;
        SceneLayer.BeforeLoadSceneEvent += OnBeforeSceneLoad;
        // texture request callbacks
        TextureRequestManager.Instance.TextureRequestStartEvent += OnTextureRequestStart;
        TextureRequestManager.Instance.TextureRequestCompletionEvent += OnTextureRequestCompletion;
        // input-related callbacks
        InputLayer.UINetworkingEvent += OnStartUINetworkingInteraction;
        InputLayer.UITutorialEvent += OnStartUITutorialInteraction;
        InputLayer.UIMainMenuEvent += OnStartUIMainMenuInteraction;
    }

    private void OnDisable()
    {
        // scene loading\unloading callbacks
        SceneLayer.ExhibitionSceneLoadedEvent -= OnExhibitionSceneLoaded;
        SceneLayer.EntranceSceneLoadedEvent -= OnEntranceSceneLoaded;
        SceneLayer.AuditoriumSceneLoadedEvent -= OnAuditoriumSceneLoaded;
        SceneLayer.BeforeLoadSceneEvent -= OnBeforeSceneLoad;
        // texture request callbacks
        if (TextureRequestManager.Instance != null)
        {
            TextureRequestManager.Instance.TextureRequestStartEvent -= OnTextureRequestStart;
            TextureRequestManager.Instance.TextureRequestCompletionEvent -=
                OnTextureRequestCompletion;
        }
        // input-related callbacks
        InputLayer.UINetworkingEvent -= OnStartUINetworkingInteraction;
        InputLayer.UITutorialEvent -= OnStartUITutorialInteraction;
        InputLayer.UIMainMenuEvent -= OnStartUIMainMenuInteraction;
    }

    private void Start()
    {
        StartCoroutine(InitUIInfoDesk());
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////// EVENT HANDLERS ///////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    private void OnTextureRequestStart(GameObject customizable)
    {
        IUIRequest customizableUIRequest = customizable.GetComponent<IUIRequest>();
        if (customizableUIRequest != null && customizableUIRequest.IsShowUIRequestEnabled())
        {
            customizableUIRequest.InitializeUIRequest();
        }
    }

    private void OnTextureRequestCompletion(GameObject customizable)
    {
        IUIRequest customizableUIRequest = customizable.GetComponent<IUIRequest>();
        if (customizableUIRequest != null && customizableUIRequest.IsShowUIRequestEnabled())
            customizableUIRequest.DestroyUIRequest();
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////// SETTERS //////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    ///     Sets main message text in loading UI.
    ///     DONE.
    /// </summary>
    /// <param name="msg">Main message to set</param>
    public void SetLoadingUIMainText(string msg) => UILoadingChild.mainMsg.text = msg;

    /// <summary>
    ///     Sets loading UI logging message text.
    ///     DONE.
    /// </summary>
    /// <param name="msg">log message to display in loading UI.</param>
    public void SetLoadingLogText(string msg) => UILoadingChild.logOutput.text = msg;

    /// <summary>
    ///     Sets startup loading bar progress.
    ///     DONE.
    /// </summary>
    /// <param name="progress">progress between 0 and 1</param>
    public void SetLoadingUIProgress(float progress)
    {
        UILoadingChild.loadingProgress.value = progress;
        UILoadingChild.progText.text = ((int)(progress * 100)).ToString();
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////// COROUTINES ////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////

    private IEnumerator InitUIInfoDesk()
    {
        while (CustomizationManager.Instance.exhibitionCustomizationData == null)
            yield return null;
        foreach (
            StandCustomizationData stand in CustomizationManager.Instance.GetStandCustomizationDatas()
        )
            UIInfoDeskChild.InstantiateEntryForStand(stand);
    }

    /// <summary>
    ///     The main character has to call this function to inform the minimap component to follow it.
    /// </summary>
    /// <param name="transformToFollow">Main Character's transform that will be followed by the minimap's cursor</param>
    public void SetMapCharacterToFollow(Transform transformToFollow)
    {
        UIMapChild.SetMainCharacterTransform(transformToFollow);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////// SHOW/HIDE UIs ///////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    ///     Shows MainMenu UI and enters its input mode (allowing the user to exit it and disabling its
    ///     instantiation).
    /// </summary>
    public void ShowUIMainMenu()
    {
        InputLayer.EnterUIMainMenuInputMode();
        UIMainMenuChild.gameObject.SetActive(true);
    }

    /// <summary>
    ///     Hides MainMenu UI and exits its input mode.
    /// </summary>
    public void HideUIMainMenu()
    {
        InputLayer.ExitUIMainMenuInputMode();
        UIMainMenuChild.gameObject.SetActive(false);
    }

    /// <summary>
    ///     Shows Tutorial UI and enters its input mode (allowing the user to exit it and disabling its instantiation).
    /// </summary>
    public void ShowUITutorial()
    {
        InputLayer.EnterUITutorialInputMode();
        UITutorialChild.gameObject.SetActive(true);
    }

    /// <summary>
    ///     Hides Tutorial UI and exits its input mode.
    ///     DONE.
    /// </summary>
    public void HideUITutorial()
    {
        InputLayer.ExitUITutorialInputMode();
        UITutorialChild.gameObject.SetActive(false);
    }

    /// <summary>
    ///     Shows Networking UI and enters its input mode (allowing the user to exit it and disabling its instantiation).
    /// </summary>
    public void ShowUINetworking()
    {
        InputLayer.EnterUINetworkingInputMode();
        UINetworkingChild.gameObject.SetActive(true);
    }

    /// <summary>
    ///     Hides Networking UI and exits its input mode.
    /// </summary>
    public void HideUINetworking()
    {
        InputLayer.ExitUINetworkingInputMode();
        UINetworkingChild.gameObject.SetActive(false);
    }

    /// <summary>
    ///     Shows Hud UI (Minimap etc...) and enters its input mode which enables the instantiation of,
    ///     among other UIs, the MainMenu and Tutorial from non-UI input.
    /// </summary>
    public void ShowUIHud()
    {
        InputLayer.EnterUIHudInputMode();
        UIHud.SetActive(true);
    }

    /// <summary>
    ///     Hides Hud UI (Minimap etc...) and exits its input mode which disables the instantiation of,
    ///     among other UIs, the MainMenu and Tutorial from non-UI input.
    /// </summary>
    public void HideUIHud()
    {
        UIHud.SetActive(false);
        InputLayer.ExitUIHudInputMode();
    }

    /// <summary>
    ///     Shows loading UI. Set this between scenes and at startup.
    /// </summary>
    public void ShowLoadingUI()
    {
        UILoadingChild.gameObject.SetActive(true);
    }

    /// <summary>
    ///     Hides loading UI. Call this this at the start of every scene.
    /// </summary>
    public void HideLoadingUI()
    {
        UILoadingChild.gameObject.SetActive(false);
    }

    /// <summary>
    ///     Shows UI button notification and enters its input mode.
    /// </summary>
    /// <param name="buttonSprite">Which button sprite to use.</param>
    /// <param name="descriptiveText">The action that is done once the button is pressed.</param>
    public void ShowUIButtonNotification(ButtonSprite buttonSprite, string descriptiveText)
    {
        InputLayer.EnterUINotificationInputMode();
        UIButtonNotificationChild.SetButtonSprite(buttonSprite);
        UIButtonNotificationChild.SetDescriptiveText(descriptiveText);
        UIButtonNotificationChild.gameObject.SetActive(true);
    }

    /// <summary>
    ///     Hides button notification UI and exits its input mode.
    /// </summary>
    public void HideUIButtonNotification()
    {
        InputLayer.ExitUINotificationInputMode();
        UIButtonNotificationChild.gameObject.SetActive(false);
    }

    /// <summary>
    ///     Shows Auditorium UI and enters its input mode.
    /// </summary>
    public void ShowUIAuditorium()
    {
        InputLayer.EnterUIAuditoriumInputMode();
        UIAuditoriumChild.gameObject.SetActive(true);
    }

    /// <summary>
    ///     Hides Auditorium UI and exits its input mode.
    /// </summary>
    public void HideUIAuditorium()
    {
        InputLayer.ExitUIAuditoriumInputMode();
        UIAuditoriumChild.gameObject.SetActive(false);
    }

    /// <summary>
    ///     Shows VideoPlayer UI and enters its input mode.
    /// </summary>
    /// <param name="videoUri">URI of the video to play</param>
    public void ShowUIVideoPlayer(Uri videoUri)
    {
        InputLayer.EnterUIVideoPlayerInputMode();
        UIVideoPlayerChild.VideoURI = videoUri;
        UIVideoPlayerChild.gameObject.SetActive(true);
    }

    /// <summary>
    ///     Hides VideoPlayer UI and exits its input mode.
    /// </summary>
    public void HideUIVideoPlayer()
    {
        InputLayer.ExitUIVideoPlayerInputMode();
        UIVideoPlayerChild.gameObject.SetActive(false);
    }

    /// <summary>
    ///     Shows Stand UI and enters its input mode.
    /// </summary>
    /// <param name="standCustomizationData">Stand customization data of the calling stand</param>
    public void ShowUIStand(StandCustomizationData standCustomizationData)
    {
        InputLayer.EnterUIStandInputMode();
        UIStandChild.StandData = standCustomizationData;
        UIStandChild.gameObject.SetActive(true);
    }

    /// <summary>
    ///     Hides Stand UI and exits its input mode.
    ///     DONE.
    /// </summary>
    public void HideUIStand()
    {
        InputLayer.ExitUIStandInputMode();
        UIStandChild.gameObject.SetActive(false);
    }

    /// <summary>
    ///     Shows Info desk UI and enters its input mode.
    ///     DONE.
    /// </summary>
    public void ShowUIInfoDesk()
    {
        InputLayer.EnterUIInfoDeskInputMode();
        UIInfoDeskChild.gameObject.SetActive(true);
    }

    /// <summary>
    ///     Hides Info desk UI and exits its input mode.
    /// </summary>
    public void HideUIInfo()
    {
        InputLayer.ExitUIInfoDeskInputMode();
        UIInfoDeskChild.gameObject.SetActive(false);
    }

    /// <summary>
    ///     Shows log output. Useful for debugging and potentially enhancing user experience.
    /// </summary>
    public void ShowLoadingUILog() => UILoadingChild.logOutput.gameObject.SetActive(true);

    /// <summary>
    ///     Hides log output.
    /// </summary>
    public void HideLoadingUILog() => UILoadingChild.logOutput.gameObject.SetActive(false);

    public void HideUIMinimap()
    {
        InputLayer.DisableUIMapInput();
        UIMapChild.gameObject.SetActive(false);
    }

    public void ShowUIMinimap()
    {
        InputLayer.EnableUIMapInput();
        UIMapChild.gameObject.SetActive(true);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////// OTHER ///////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    ///     Enters Preview Entrance Scene UI mode.
    /// </summary>
    public void EnterPreviewEntranceMode()
    {
        HideUIHud();
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////// SCENE LOADED EVENT HANDLERS ////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    ///     DONE.
    /// </summary>
    void OnEntranceSceneLoaded()
    {
        HideLoadingUI();
        ShowUIHud();
        if (CustomizationManager.Instance.isThisEntrancePreview)
        {
            EnterPreviewEntranceMode();
            return;
        }
        UIMapChild.SetMapImage(EntranceMap, EntranceGTMUnit);
    }

    /// <summary>
    ///     DONE.
    /// </summary>
    /// <param name="layout"></param>
    void OnExhibitionSceneLoaded(Layout layout)
    {
        HideLoadingUI();
        ShowUIHud();
        switch (layout)
        {
            case Layout.SMALL:
                UIMapChild.SetMapImage(SExhibitionMap, SExhibitionGTMUnit);
                break;
            case Layout.MEDIUM:
                UIMapChild.SetMapImage(MExhibitionMap, MExhibitionGTMUnit);
                break;
            case Layout.LARGE:
                UIMapChild.SetMapImage(LExhibitionMap, LExhibitionGTMUnit);
                break;
            default:
                break;
        }
    }

    /// <summary>
    ///     Called when Auditorium scene is loaded. Instantiates Auditorium with adequate
    ///     input modes and disables UIHud.
    ///     DONE.
    /// </summary>
    void OnAuditoriumSceneLoaded()
    {
        Cursor.lockState = CursorLockMode.Confined;
        InputLayer.UIAuditoriumExitEvent += OnUIAuditoriumExit;
        InputLayer.DisableLocomotionInput();
        HideUIHud();
        HideLoadingUI();
        ShowUIAuditorium();
    }

    // TODO: Hide loading UI when exiting Auditorium scene

    /// <summary>
    ///     Called when Auditorium UI instantiated from entering the Auditorium scene is exited.
    ///     DONE.
    /// </summary>
    void OnUIAuditoriumExit()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InputLayer.UIAuditoriumExitEvent -= OnUIAuditoriumExit;
        InputLayer.EnableLocomotionInput();
        ShowUIHud();
        HideUIAuditorium();
        SceneLayer.LoadEntranceScene();
    }

    /// <summary>
    ///     Called when user instantiates MainMenu UI from non-UI input.
    /// </summary>

    void OnStartUIMainMenuInteraction()
    {
        InputLayer.UIMainMenuExitEvent += OnEndUIMainMenuInteraction;
        InputLayer.DisableLocomotionInput();
        HideUIHud();
        ShowUIMainMenu();
    }

    /// <summary>
    ///     Called when MainMenu UI was instantiated from non-UI input is closed.
    /// </summary>
    void OnEndUIMainMenuInteraction()
    {
        InputLayer.UIMainMenuExitEvent -= OnEndUIMainMenuInteraction;
        InputLayer.EnableLocomotionInput();
        ShowUIHud();
        HideUIMainMenu();
    }

    /// <summary>
    ///     Called when user instantiates Networling UI from non-UI input.
    /// </summary>
    void OnStartUINetworkingInteraction()
    {
        InputLayer.UINetworkingExitEvent += OnEndUINetworkingInteraction;
        InputLayer.DisableLocomotionInput();
        HideUIHud();
        ShowUINetworking();
    }

    /// <summary>
    ///     Called when Networking UI that was instantiated from non-UI input is closed.
    ///     DONE.
    /// </summary>
    void OnEndUINetworkingInteraction()
    {
        InputLayer.UINetworkingExitEvent -= OnEndUINetworkingInteraction;
        InputLayer.EnableLocomotionInput();
        ShowUIHud();
        HideUINetworking();
    }

    /// <summary>
    ///     Called when user instantiates Tutorial UI from non-UI input.
    ///     DONE.
    /// </summary>
    void OnStartUITutorialInteraction()
    {
        InputLayer.UITutorialExitEvent += OnEndUITutorialInteraction;
        InputLayer.DisableLocomotionInput();
        HideUIHud();
        ShowUITutorial();
    }

    /// <summary>
    ///     Called when Tutorial UI that was instantiated from non-UI input is closed.
    ///     DONE.
    /// </summary>
    void OnEndUITutorialInteraction()
    {
        InputLayer.UITutorialExitEvent -= OnEndUITutorialInteraction;
        InputLayer.EnableLocomotionInput();
        ShowUIHud();
        HideUITutorial();
    }

    void OnBeforeSceneLoad(AsyncOperation operation)
    {
        ShowLoadingUI();
        SetLoadingUIMainText("chargement des données de scène");
        HideLoadingUILog();
        StartCoroutine(UpdateLoadingProgressBar());
        IEnumerator UpdateLoadingProgressBar()
        {
            yield return null;
            while (!operation.isDone)
            {
                SetLoadingUIProgress(operation.progress);
                yield return null;
            }
            StopCoroutine(nameof(UpdateLoadingProgressBar));
        }
    }
}

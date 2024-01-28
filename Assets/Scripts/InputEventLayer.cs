using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputEventLayer", menuName = "SO/InputEventLayer")]
public class InputEventLayer : ScriptableObject
{
    // Locomotion events
    public event Action<Vector2> LookEvent;

    // UIMap events
    public event Action UIMapChangeModeEvent;

    // UI events
    public event Action UINotificationEvent;
    public event Action UINetworkingEvent;
    public event Action UITutorialEvent;
    public event Action UIMainMenuEvent;
    public event Action UIPlayStopVideoEvent;
    public event Action UITutorialExitEvent;
    public event Action UIMainMenuExitEvent;
    public event Action UIStandExitEvent;
    public event Action UIInfoDeskExitEvent;
    public event Action UINetworkingExitEvent;
    public event Action UIVideoPlayerExitEvent;
    public event Action UIAuditoriumExitEvent;
    public InputActionAsset asset;
    InputActionMap Locomotion;
    InputActionMap UI;
    InputAction Locomotion_look;
    InputAction Locomotion_walk;
    InputAction Locomotion_sidewalk;
    InputAction UIMap_changemode;
    InputAction UINotification;
    InputAction UINetworking;
    InputAction UITutorial;
    InputAction UIMainMenu;
    InputAction UIVideoPlayer_playstopvideo;

    InputAction UITutorial_exit;
    InputAction UIMainMenu_exit;
    InputAction UIInfoDesk_exit;
    InputAction UIStand_exit;
    InputAction UINetworking_exit;
    InputAction UIVideoPlayer_exit;
    InputAction UIAuditorium_exit;

    private void OnEnable()
    {
        // Initializing ActionMaps
        Locomotion = asset.FindActionMap("Locomotion", throwIfNotFound: true);
        UI = asset.FindActionMap("UI", throwIfNotFound: true);
        // Initializing Actions
        Locomotion_look = Locomotion.FindAction("look", throwIfNotFound: true);
        Locomotion_walk = Locomotion.FindAction("walk", throwIfNotFound: true);
        Locomotion_sidewalk = Locomotion.FindAction("sidewalk", throwIfNotFound: true);
        UIMap_changemode = UI.FindAction("UIMap_changemode", throwIfNotFound: true);
        UINotification = UI.FindAction("UINotification", throwIfNotFound: true);
        UINetworking = UI.FindAction("UINetworking", throwIfNotFound: true);
        UITutorial = UI.FindAction("UITutorial", throwIfNotFound: true);
        UIMainMenu = UI.FindAction("UIMainMenu", throwIfNotFound: true);

        UITutorial_exit = UI.FindAction("UITutorial_exit", throwIfNotFound: true);
        UIMainMenu_exit = UI.FindAction("UIMainMenu_exit", throwIfNotFound: true);
        UIInfoDesk_exit = UI.FindAction("UIInfoDesk_exit", throwIfNotFound: true);
        UIStand_exit = UI.FindAction("UIStand_exit", throwIfNotFound: true);
        UINetworking_exit = UI.FindAction("UINetworking_exit", throwIfNotFound: true);
        UIVideoPlayer_exit = UI.FindAction("UIVideoPlayer_exit", throwIfNotFound: true);
        UIVideoPlayer_playstopvideo = UI.FindAction(
            "UIVideoPlayer_playstopvideo",
            throwIfNotFound: true
        );
        UIAuditorium_exit = UI.FindAction("UIAuditorium_exit", throwIfNotFound: true);

        // Subscribing to input events
        Locomotion_look.performed += on_locomotion_look;
        UIMap_changemode.performed += on_change_map_mode;
        UINotification.performed += on_ui_notification;
        UINetworking.performed += on_ui_networking;
        UITutorial.performed += on_ui_tutorial;
        UIMainMenu.performed += on_ui_main_menu;
        UIVideoPlayer_playstopvideo.performed += on_ui_play_stop_video;

        UITutorial_exit.performed += on_ui_tutorial_exit;
        UIMainMenu_exit.performed += on_ui_mainmenu_exit;
        UIInfoDesk_exit.performed += on_ui_infodesk_exit;
        UIStand_exit.performed += on_ui_stand_exit;
        UINetworking_exit.performed += on_ui_networking_exit;
        UIVideoPlayer_exit.performed += on_ui_videoplayer_exit;
        UIAuditorium_exit.performed += on_ui_auditorium_exit;
    }

    private void OnDisable()
    {
        Locomotion_look.performed -= on_locomotion_look;
        UIMap_changemode.performed -= on_change_map_mode;
        UINotification.performed -= on_ui_notification;
        UINetworking.performed -= on_ui_networking;
        UITutorial.performed -= on_ui_tutorial;
        UIMainMenu.performed -= on_ui_main_menu;
        UIVideoPlayer_playstopvideo.performed -= on_ui_play_stop_video;

        UITutorial_exit.performed -= on_ui_tutorial_exit;
        UIMainMenu_exit.performed -= on_ui_mainmenu_exit;
        UIInfoDesk_exit.performed -= on_ui_infodesk_exit;
        UIStand_exit.performed -= on_ui_stand_exit;
        UINetworking_exit.performed -= on_ui_networking_exit;
        UIVideoPlayer_exit.performed -= on_ui_videoplayer_exit;
        UIAuditorium_exit.performed -= on_ui_auditorium_exit;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////// ENABLING AND DISABLING Action Maps ///////////////////////////////////
    public void EnableLocomotionInput() => Locomotion.Enable();

    public void DisableLocomotionInput() => Locomotion.Disable();

    public void EnableUIMapInput() => UIMap_changemode.Enable();

    public void DisableUIMapInput() => UIMap_changemode.Disable();

    public void EnableUITutorialInput() => UITutorial.Disable();

    public void DisableUITutorialInput() => UITutorial.Disable();

    public void EnableUIMainMenuInput() => UIMainMenu.Enable();

    public void DisableUIMainMenuInput() => UIMainMenu.Disable();

    public void EnableUINotificationInput() => UINotification.Enable();

    public void EnableUINetworkingInput() => UINetworking.Enable();

    public void DisableUINetworkingInput() => UINetworking.Disable();

    public void EnableUIPlayStopVideoInput() => UIVideoPlayer_playstopvideo.Enable();

    public void DisableUINotificationInput() => UINotification.Disable();

    public void DisableUIPlayStopVideoInput() => UIVideoPlayer_playstopvideo.Disable();

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////// ENTERING AND EXITING SPECIFIC INPUT MODES //////////////////////////////
    public void EnterUIMainMenuInputMode()
    {
        UIMainMenu_exit.Enable();
    }

    public void ExitUIMainMenuInputMode()
    {
        UIMainMenu_exit.Disable();
    }

    public void EnterUIStandInputMode()
    {
        UIStand_exit.Enable();
    }

    public void ExitUIStandInputMode()
    {
        UIStand_exit.Disable();
    }

    public void EnterUIInfoDeskInputMode()
    {
        UIInfoDesk_exit.Enable();
    }

    public void ExitUIInfoDeskInputMode()
    {
        UIInfoDesk_exit.Disable();
    }

    public void EnterUINetworkingInputMode()
    {
        UINetworking_exit.Enable();
    }

    public void ExitUINetworkingInputMode()
    {
        UINetworking_exit.Disable();
    }

    public void EnterUIAuditoriumInputMode()
    {
        UIAuditorium_exit.Enable();
    }

    public void ExitUIAuditoriumInputMode()
    {
        UIAuditorium_exit.Disable();
    }

    public void EnterUITutorialInputMode()
    {
        UITutorial_exit.Enable();
    }

    public void EnterUIHudInputMode()
    {
        UITutorial.Enable();
        UINetworking.Enable();
        UIMainMenu.Enable();
        UIMap_changemode.Enable();
    }

    public void ExitUIHudInputMode()
    {
        UITutorial.Disable();
        UINetworking.Disable();
        UIMainMenu.Disable();
        UIMap_changemode.Disable();
    }

    public void ExitUITutorialInputMode()
    {
        UITutorial_exit.Disable();
    }

    public void EnterUINotificationInputMode()
    {
        UINotification.Enable();
    }

    public void ExitUINotificationInputMode()
    {
        UINotification.Disable();
    }

    public void EnterUIVideoPlayerInputMode()
    {
        UIVideoPlayer_exit.Enable();
        UIVideoPlayer_playstopvideo.Enable();
    }

    public void ExitUIVideoPlayerInputMode()
    {
        UIVideoPlayer_exit.Disable();
        UIVideoPlayer_playstopvideo.Disable();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // ////////////////////////// EVENT INVOKERS THROUGH INPUT SYSTEM //////////////////////////////////
    private void on_locomotion_look(InputAction.CallbackContext context) =>
        LookEvent?.Invoke(context.ReadValue<Vector2>());

    private void on_change_map_mode(InputAction.CallbackContext _) =>
        UIMapChangeModeEvent?.Invoke();

    private void on_ui_notification(InputAction.CallbackContext _) => UINotificationEvent?.Invoke();

    private void on_ui_networking(InputAction.CallbackContext _) => UINetworkingEvent?.Invoke();

    private void on_ui_tutorial(InputAction.CallbackContext _) => UITutorialEvent?.Invoke();

    private void on_ui_main_menu(InputAction.CallbackContext _) => UIMainMenuEvent?.Invoke();

    private void on_ui_play_stop_video(InputAction.CallbackContext _) =>
        UIPlayStopVideoEvent?.Invoke();

    private void on_ui_tutorial_exit(InputAction.CallbackContext _) =>
        UITutorialExitEvent?.Invoke();

    private void on_ui_mainmenu_exit(InputAction.CallbackContext _) =>
        UIMainMenuExitEvent?.Invoke();

    private void on_ui_stand_exit(InputAction.CallbackContext _) => UIStandExitEvent?.Invoke();

    private void on_ui_infodesk_exit(InputAction.CallbackContext _) =>
        UIInfoDeskExitEvent?.Invoke();

    private void on_ui_networking_exit(InputAction.CallbackContext _) =>
        UINetworkingExitEvent?.Invoke();

    private void on_ui_videoplayer_exit(InputAction.CallbackContext _) =>
        UIVideoPlayerExitEvent?.Invoke();

    private void on_ui_auditorium_exit(InputAction.CallbackContext _) =>
        UIAuditoriumExitEvent?.Invoke();

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////// EVENT INVOKERS /////////////////////////////////////////////
    public void InvokeUINetworkingEvent() => UINetworkingEvent?.Invoke();

    public void InvokeUITutorialEvent() => UITutorialEvent?.Invoke();

    public void InvokeUITutorialExitEvent() => UITutorialExitEvent?.Invoke();

    public void InvokeUIPlayStopVideoEvent() => UIPlayStopVideoEvent?.Invoke();

    public void InvokeUIMainMenuExitEvent() => UIMainMenuExitEvent?.Invoke();

    public void InvokeUINetworkingExitEvent() => UINetworkingExitEvent?.Invoke();

    public void InvokeUIInfoDeskExitEvent() => UIInfoDeskExitEvent?.Invoke();

    public void InvokeUIAuditoriumExitEvent() => UIAuditoriumExitEvent?.Invoke();

    public void InvokeUIVideoPlayerExitEvent() => UIVideoPlayerExitEvent?.Invoke();

    public void InvokeUIStandExitEvent() => UIStandExitEvent?.Invoke();

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////// INPUT ACTION SPECIFIC FUNCTIONS ///////////////////////////////////
    public bool IsWalking() => Locomotion_walk.IsPressed();

    public float GetWalkValue() => Locomotion_walk.ReadValue<float>();

    public bool IsSidewalking() => Locomotion_sidewalk.IsPressed();

    public float GetSidewalkValue() => Locomotion_sidewalk.ReadValue<float>();
    ////////////////////////////////////////////////////////////////////////////////////////////////////
}

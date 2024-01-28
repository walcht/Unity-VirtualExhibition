using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public InputEventLayer InputLayer;

    public Button resume;
    public Button networking;
    public Button tutorial;
    public Button settings;
    public Button exit;

    [DllImport("__Internal")]
    private static extern void ApplicationExitEvent();

    void Awake()
    {
        resume.onClick.AddListener(OnResumeClick);
        networking.onClick.AddListener(OnNetworkingClick);
        tutorial.onClick.AddListener(OnTutorialClick);
        exit.onClick.AddListener(OnExitClick);
    }

    void OnEnable()
    {
        resume.Select();
    }

    void OnResumeClick()
    {
        InputLayer.InvokeUIMainMenuExitEvent();
    }

    void OnTutorialClick()
    {
        UIManager.Instance.HideUIMainMenu();
        InputLayer.UITutorialExitEvent += OnExitUITutorial;
        UIManager.Instance.ShowUITutorial();
    }

    void OnNetworkingClick()
    {
        UIManager.Instance.HideUIMainMenu();
        InputLayer.UINetworkingExitEvent += OnExitUINetworking;
        UIManager.Instance.ShowUINetworking();
    }

    void OnExitClick()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ApplicationExitEvent();
#endif
        Application.Quit();
    }

    void OnExitUITutorial()
    {
        UIManager.Instance.HideUITutorial();
        InputLayer.UITutorialExitEvent -= OnExitUITutorial;
        UIManager.Instance.ShowUIMainMenu();
        tutorial.Select();
    }

    void OnExitUINetworking()
    {
        UIManager.Instance.HideUINetworking();
        InputLayer.UINetworkingExitEvent -= OnExitUINetworking;
        UIManager.Instance.ShowUIMainMenu();
        networking.Select();
    }
}

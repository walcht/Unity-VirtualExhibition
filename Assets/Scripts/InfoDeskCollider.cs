using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class InfoDeskCollider : MonoBehaviour
{
    public InputEventLayer InputLayer;
    public GameObject GlowingSphere;
    public string NotificationMsg = "pour ouvrir le menu du bureau d'information";
    int playerTagHash;

    private void Start()
    {
        playerTagHash = "MainCharacter".GetHashCode();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.GetHashCode() == playerTagHash)
        {
            GlowingSphere.SetActive(false);
            InputLayer.UINotificationEvent += OnStartInteraction;
            InputLayer.DisableUIMainMenuInput();
            InputLayer.DisableUINetworkingInput();
            InputLayer.DisableUITutorialInput();
            UIManager.Instance.ShowUIButtonNotification(
                Common.ButtonSprite.KEYBOARD_E,
                NotificationMsg
            );
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.GetHashCode() == playerTagHash)
        {
            GlowingSphere.SetActive(true);
            InputLayer.UINotificationEvent -= OnStartInteraction;
            InputLayer.EnableUIMainMenuInput();
            InputLayer.EnableUINetworkingInput();
            InputLayer.EnableUITutorialInput();
            UIManager.Instance.HideUIButtonNotification();
        }
    }

    private void OnExitInfoDeskUI()
    {
        InputLayer.UIInfoDeskExitEvent -= OnExitInfoDeskUI;
        InputLayer.EnableLocomotionInput();
        UIManager.Instance.HideUIInfo();
        UIManager.Instance.ShowUIButtonNotification(
            Common.ButtonSprite.KEYBOARD_E,
            NotificationMsg
        );
        UIManager.Instance.ShowUIMinimap();
    }

    private void OnStartInteraction()
    {
        InputLayer.UIInfoDeskExitEvent += OnExitInfoDeskUI;
        InputLayer.DisableLocomotionInput();
        UIManager.Instance.ShowUIInfoDesk();
        UIManager.Instance.HideUIButtonNotification();
        UIManager.Instance.HideUIMinimap();
    }
}

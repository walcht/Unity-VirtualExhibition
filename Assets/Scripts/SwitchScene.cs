using Common;
using UnityEngine;

public class SwitchScene : MonoBehaviour
{
    public EnterTrigger EnterWhat;
    public SceneManagementLayer SceneLayer;
    public InputEventLayer InputLayer;
    int playerTagHash;

    private void Awake()
    {
        playerTagHash = "MainCharacter".GetHashCode();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.GetHashCode() == playerTagHash)
        {
            InputLayer.UINotificationEvent += OnEnterAction;
            InputLayer.DisableUIMainMenuInput();
            InputLayer.DisableUINetworkingInput();
            InputLayer.DisableUITutorialInput();
            ShowNotificationUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.GetHashCode() == playerTagHash)
        {
            InputLayer.UINotificationEvent -= OnEnterAction;
            InputLayer.EnableUIMainMenuInput();
            InputLayer.EnableUINetworkingInput();
            InputLayer.EnableUITutorialInput();
            UIManager.Instance.HideUIButtonNotification();
        }
    }

    private void OnEnterAction()
    {
        InputLayer.UINotificationEvent -= OnEnterAction;
        UIManager.Instance.HideUIButtonNotification();

        switch (EnterWhat)
        {
            case EnterTrigger.ENTER_EXHIBITION:
                SceneLayer.LoadExhibitionScene();
                break;

            case EnterTrigger.ENTER_ENTRANCE:
                SceneLayer.LoadEntranceScene();
                break;

            case EnterTrigger.ENTER_AUDITORIUM:
                StatisticsManager.Instance.OnAuditoriumEnterPerformed();
                SceneLayer.LoadAuditoriumScene();
                break;

            case EnterTrigger.EXIT:
                break;

            default:
                break;
        }
    }

    private void ShowNotificationUI()
    {
        switch (EnterWhat)
        {
            case EnterTrigger.ENTER_EXHIBITION:
                UIManager.Instance.ShowUIButtonNotification(
                    ButtonSprite.KEYBOARD_E,
                    "pour entrer dans la salle d'exposition"
                );
                break;

            case EnterTrigger.ENTER_ENTRANCE:
                UIManager.Instance.ShowUIButtonNotification(
                    ButtonSprite.KEYBOARD_E,
                    "pour entrer dans la salle d'entree"
                );
                break;

            case EnterTrigger.ENTER_AUDITORIUM:
                UIManager.Instance.ShowUIButtonNotification(
                    ButtonSprite.KEYBOARD_E,
                    "pour entrer dans la salle d'auditorium"
                );
                break;

            case EnterTrigger.EXIT:
                // idk, mb exit game?
                break;

            default:
                break;
        }
    }
}

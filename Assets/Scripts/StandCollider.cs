using Common;
using UnityEngine;

/// <summary>
///     This class is used to show\hide stand UI and to start stand interaction.
/// </summary>
[RequireComponent(typeof(Collider))]
public class StandCollider : MonoBehaviour
{
    public InputEventLayer InputLayer;
    public GameObject GlowingSphere;

    [Tooltip("Message text that will be shown in notification popup UI")]
    public string NotficationMessage;

    // in order to properly show stand interaction UI, it is extremely
    // important for this collider to know which stand it is attached to.
    [HideInInspector]
    public StandHelper ParentStand; // the StandHelper component that parents this object.

    // this attribute is also set via an external function by StandHelper of the parent stand
    int playerTagHash;
    float interactionStartTime; // holds the start time of the stand interaction, useful for statistics.

    private void OnEnable()
    {
        playerTagHash = "MainCharacter".GetHashCode();
    }

    /// <summary>
    ///     When MainCharacter enters this collider, show Stand UI and start listening to StartInteraction
    ///     input
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.GetHashCode() == playerTagHash)
        {
            GlowingSphere.SetActive(false);
            InputLayer.UINotificationEvent += OnUIStandStart;
            InputLayer.DisableUIMainMenuInput();
            InputLayer.DisableUINetworkingInput();
            InputLayer.DisableUITutorialInput();
            UIManager.Instance.ShowUIButtonNotification(
                ButtonSprite.KEYBOARD_E,
                NotficationMessage
            );
        }
    }

    /// <summary>
    ///     When MainCharacter exits this collider, hide Stand UI and stop listening to StartInteraction
    ///     input
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.GetHashCode() == playerTagHash)
        {
            GlowingSphere.SetActive(true);
            InputLayer.UINotificationEvent -= OnUIStandStart;
            InputLayer.EnableUIMainMenuInput();
            InputLayer.EnableUINetworkingInput();
            InputLayer.EnableUITutorialInput();
            UIManager.Instance.HideUIButtonNotification();
        }
    }

    /// <summary>
    ///     Gets called when MainCharacter starts an intercation with a stand
    /// </summary>
    private void OnUIStandStart()
    {
        InputLayer.UIStandExitEvent += OnUIStandExit;
        InputLayer.DisableLocomotionInput();
        UIManager.Instance.ShowUIStand(ParentStand.standCustomizationData);
        UIManager.Instance.HideUIButtonNotification();
        UIManager.Instance.HideUIMinimap();
        // starting interaction timer (for statistics)
        interactionStartTime = Time.realtimeSinceStartup;
    }

    /// <summary>
    ///     Gets called when MainCharacter ends an interaction with a stand
    /// </summary>
    //
    private void OnUIStandExit()
    {
        InputLayer.UIStandExitEvent -= OnUIStandExit;
        InputLayer.EnableLocomotionInput();
        UIManager.Instance.HideUIStand();
        UIManager.Instance.ShowUIButtonNotification(ButtonSprite.KEYBOARD_E, NotficationMessage);
        UIManager.Instance.ShowUIMinimap();
        // ending interaction timer and sending statistics about this interaction
        StatisticsManager.Instance.OnStandActionPerformed(
            ParentStand.standCustomizationData.standID,
            StandAction.INTERACTION,
            Time.realtimeSinceStartup - interactionStartTime
        );
    }
}

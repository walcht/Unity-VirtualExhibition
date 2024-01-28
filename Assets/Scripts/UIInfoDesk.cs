using System.Collections.Generic;
using Common;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInfoDesk : MonoBehaviour
{
    public InputEventLayer InputLayer;
    public Image RightPanelLogo;
    public TMP_Text RightPanelDescription;
    public TMP_Text RightPanelPhone;
    public TMP_Text RightPanelAddress;
    public Button ExitButton;

    [Tooltip("Parent Transform of entries that contains stand entries")]
    public Transform entriesContainer;

    [Tooltip("Prefab of a button that will be used to instantiate entries")]
    public GameObject entry;

    public Sprite defaultLogo;

    public string defaultDescription;

    // for referencing instantiated entries
    readonly List<(GameObject gameObject, StandCustomizationData standCustomizationData)> entries =
        new List<(GameObject gameObject, StandCustomizationData standCustomizationData)>();

    private void OnEnable()
    {
        EventSystem eventSystemRef = EventSystem.current;
        if (entries.Count > 0)
            eventSystemRef.SetSelectedGameObject(entries[0].gameObject);
        else
            eventSystemRef.SetSelectedGameObject(ExitButton.gameObject);
    }

    /// <summary>
    ///    Instantiates an entry for the provided stand in the info desk menu
    /// </summary>
    /// <param name="standCustomizationData">StandCustomizationData that this entry references.</param>
    public void InstantiateEntryForStand(StandCustomizationData standCustomizationData)
    {
        GameObject instantiatedEntry = Instantiate(entry, entriesContainer);
        entries.Add((instantiatedEntry, standCustomizationData));
        int entryIdx = entries.Count - 1;
        // It is assumed that each button has a child on which a TMP_text componenet is attached instantiatedEntry.GetComponentInChildren<TMP_Text>().text = standCustomizationData.standName;
        instantiatedEntry.GetComponentInChildren<TMP_Text>().text =
            standCustomizationData.standName;
        instantiatedEntry.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(entryIdx));
        instantiatedEntry.GetComponent<UIInfoDeskEntry>().onSelectEvent += () =>
            OnButtonSelect(entryIdx);
    }

    /// <summary>
    ///     Called each time the user presses the button identified by its index.
    /// </summary>
    /// <param name="index">Identifier of the clicked button.</param>
    public void OnButtonClick(int index)
    {
        InputLayer.UIStandExitEvent += OnEndStandInteraction;
        UIManager.Instance.HideUIInfo();
        UIManager.Instance.ShowUIStand(entries[index].standCustomizationData);
    }

    /// <summary>
    ///     Called each time the user exits the stand interaction UI after having entered
    ///     it through info desk interaction UI.
    /// </summary>
    public void OnEndStandInteraction()
    {
        InputLayer.UIStandExitEvent -= OnEndStandInteraction;
        UIManager.Instance.HideUIStand();
        UIManager.Instance.ShowUIInfoDesk();
    }

    /// <summary>
    ///     Called each time the user selects a stand's entry button.
    /// </summary>
    /// <param name="index">Identifier of the selected button.</param>
    public void OnButtonSelect(int index)
    {
        TextureRequestManager.Instance.Abort();
        StandCustomizationData stand = entries[index].standCustomizationData;
        // if a logo texture is available use it, otherwise set the Logo to its default texture
        if (stand.logoDownloadURL != null)
        {
            TextureRequestManager.Instance.AddRequest(
                new RequestData(
                    RightPanelLogo.gameObject,
                    CustomizationManager.Instance.mediaStaticURL + stand.logoDownloadURL,
                    true,
                    true
                )
            );

            TextureRequestManager.Instance.StartServingTextureRequests();
        }
        else
            RightPanelLogo.sprite = defaultLogo;
        // if a short description string is available use it, otherwise set the description to its default value
        if (!string.IsNullOrWhiteSpace(stand.shortDescription))
            RightPanelDescription.text = stand.shortDescription;
        else
            RightPanelDescription.text = defaultDescription;
        // if a phone number is providede then show it in the phone field, otherwise set the phone number to its default value
        if (!string.IsNullOrWhiteSpace(stand.phoneNumber))
            RightPanelPhone.text = stand.phoneNumber;
        else
            RightPanelPhone.text = "NON DISPONIBLE";
        // if an address is provided then show it in the address field, otherwise set the address field to its default value
        if (!string.IsNullOrWhiteSpace(stand.address))
            RightPanelAddress.text = stand.address;
        else
            RightPanelAddress.text = "NON DISPONIBLE";
    }

    /// <summary>
    ///     Called when Exit butto (on the UI) is pressed.
    /// </summary>
    public void OnExitButtonClick() => InputLayer.InvokeUIInfoDeskExitEvent();
}

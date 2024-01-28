using System.Collections;
using JSONContainer;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UINetworking : MonoBehaviour
{
    public InputEventLayer InputLayer;
    public Button pageLeftButton;
    public Button pageRightButton;
    public Button returnButton;
    public UINetworkingEntry[] entries;

    // value is fetched from Web server and indicates the total number of pages
    int totalNumberPages;

    // to ensure that JSON nbr of pages is fetched only one time
    bool isTotalNbrPagesSet = false;

    // to keep reference of the current page
    int currentPageIndex = 0;
    const string notProvided = "-";

    private void Awake()
    {
        foreach (UINetworkingEntry entry in entries)
        {
            entry.first_name.text = notProvided;
            entry.last_name.text = notProvided;
            entry.email.text = notProvided;
            entry.phone.text = notProvided;
            entry.position.text = notProvided;
            entry.establishment.text = notProvided;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Init());
    }

    private void OnDisable()
    {
        // reset to page 0
        currentPageIndex = 0;
    }

    /// <summary>
    ///     Call this to enable\disable page left or page right buttons (or both).
    ///     Should be called at the start and whenever current page changes (for example after a switch-page button click).
    /// </summary>
    void RefreshPageButtons()
    {
        pageLeftButton.interactable = true;
        pageRightButton.interactable = true;
        if (currentPageIndex == 0)
            pageLeftButton.interactable = false;
        if (currentPageIndex == (totalNumberPages - 1))
            pageRightButton.interactable = false;
    }

    void OnPageLeftButtonClick()
    {
        if (!isTotalNbrPagesSet)
            return;
        currentPageIndex = Mathf.Min(currentPageIndex - 1, 0);
        if (currentPageIndex == 0)
        {
            pageLeftButton.interactable = false;
            pageRightButton.interactable = true;
        }
        for (int i = 0; i < entries.Length; i++)
            entries[i].gameObject.SetActive(false); // clear all rows
        StartCoroutine(SetNetworkingDataJSON());
    }

    void OnPageRightButtonClick()
    {
        if (!isTotalNbrPagesSet)
            return;
        currentPageIndex = Mathf.Max(currentPageIndex + 1, totalNumberPages - 1);
        if (currentPageIndex == totalNumberPages - 1)
        {
            pageLeftButton.interactable = true;
            pageRightButton.interactable = false;
        }
        for (int i = 0; i < entries.Length; i++)
            entries[i].gameObject.SetActive(false); // clear all rows
        StartCoroutine(SetNetworkingDataJSON());
    }

    void OnReturnButtonClick() => InputLayer.InvokeUINetworkingExitEvent();

    IEnumerator Init()
    {
        if (!isTotalNbrPagesSet)
            StartCoroutine(SetNetworkingNbrPagesJSON());
        // clear all rows
        for (int i = 0; i < entries.Length; i++)
            entries[i].gameObject.SetActive(false);
        // wait until total number of pages is set
        while (!isTotalNbrPagesSet)
            yield return null;
        StartCoroutine(SetNetworkingDataJSON());
        returnButton.Select();
        if (totalNumberPages <= 1)
        {
            pageLeftButton.interactable = false;
            pageRightButton.interactable = false;
        }
        else
        {
            pageLeftButton.onClick.AddListener(OnPageLeftButtonClick);
            pageRightButton.onClick.AddListener(OnPageRightButtonClick);
        }
        returnButton.onClick.AddListener(OnReturnButtonClick);
    }

    IEnumerator SetNetworkingDataJSON()
    {
        using (
            UnityWebRequest uwr = UnityWebRequest.Get(
                new System.Uri(
                    new System.Uri(CustomizationManager.Instance.jsonStaticURL),
                    $"exhibition/networking/{CustomizationManager.Instance.id}/{currentPageIndex}"
                )
            )
        )
        {
            yield return uwr.SendWebRequest();
            if (!(uwr.result == UnityWebRequest.Result.Success))
            {
                Debug.LogWarning("Failed to request networking JSON data");
                yield break;
            }
            else
            {
                NetworkingPageDataContainer networkingPageDataContainer =
                    NetworkingPageDataContainer.FromJson(uwr.downloadHandler.text);
                // allocate rows
                for (int i = 0; i < networkingPageDataContainer.Data.Count; ++i)
                {
                    // activate (show) the ith row
                    entries[i].gameObject.SetActive(true);
                    if (!string.IsNullOrWhiteSpace(networkingPageDataContainer.Data[i].FirstName))
                        entries[i].first_name.text = networkingPageDataContainer.Data[i].FirstName;
                    else
                        entries[i].first_name.text = notProvided;
                    if (!string.IsNullOrWhiteSpace(networkingPageDataContainer.Data[i].LastName))
                        entries[i].last_name.text = networkingPageDataContainer.Data[i].LastName;
                    else
                        entries[i].last_name.text = notProvided;
                    if (!string.IsNullOrWhiteSpace(networkingPageDataContainer.Data[i].PhoneNumber))
                        entries[i].phone.text = networkingPageDataContainer.Data[i].PhoneNumber;
                    else
                        entries[i].phone.text = notProvided;
                    if (!string.IsNullOrWhiteSpace(networkingPageDataContainer.Data[i].Profession))
                        entries[i].position.text = networkingPageDataContainer.Data[i].Profession;
                    else
                        entries[i].position.text = notProvided;
                    if (
                        !string.IsNullOrWhiteSpace(
                            networkingPageDataContainer.Data[i].Establishment
                        )
                    )
                        entries[i].establishment.text = networkingPageDataContainer
                            .Data[i]
                            .Establishment;
                    else
                        entries[i].establishment.text = notProvided;
                    if (!string.IsNullOrWhiteSpace(networkingPageDataContainer.Data[i].Email))
                        entries[i].email.text = networkingPageDataContainer.Data[i].Email;
                    else
                        entries[i].email.text = notProvided;
                }
                // refresh buttons
                RefreshPageButtons();
            }
        }
    }

    IEnumerator SetNetworkingNbrPagesJSON()
    {
        using (
            UnityWebRequest uwr = UnityWebRequest.Get(
                new System.Uri(
                    new System.Uri(CustomizationManager.Instance.jsonStaticURL),
                    $"exhibition/networking/count/{CustomizationManager.Instance.id}"
                )
            )
        )
        {
            yield return uwr.SendWebRequest();
            if (!(uwr.result == UnityWebRequest.Result.Success))
            {
                Debug.LogWarning("Failed to request networking (nbr of pages) JSON data");
                totalNumberPages = 0;
                isTotalNbrPagesSet = true;
            }
            else
            {
                totalNumberPages = NetworkingNbrPagesContainer
                    .FromJson(uwr.downloadHandler.text)
                    .Data;
                isTotalNbrPagesSet = true;
            }
        }
    }
}

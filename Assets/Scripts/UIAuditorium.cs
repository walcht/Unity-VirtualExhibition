//#define DEBUG_AUDITORIUM_VIDEO

using System.Collections;
using JSONContainer;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIAuditorium : MonoBehaviour
{
    [Tooltip("WebinarVideo UI Prefab for video previews.")]
    public GameObject WebinarVideoPrefab;

    [Tooltip("Rect Transform that will contain webinar video entries.")]
    public RectTransform Content;
    public TMP_Text description;
    public Button Exit;
    public InputEventLayer inputEventLayer;
    public SceneManagementLayer sceneManagementLayer;
    WebinarDataContainer webinarDataContainer = null;

    private void OnEnable()
    {
        if (webinarDataContainer == null)
            StartCoroutine(SetWebinarData());
        Exit.onClick.AddListener(() => inputEventLayer.InvokeUIAuditoriumExitEvent());
    }

    private void OnDisable()
    {
        Exit.onClick.RemoveAllListeners();
    }

    IEnumerator SetWebinarData()
    {
        using (
            UnityWebRequest uwr = UnityWebRequest.Get(
                new System.Uri(
                    new System.Uri(CustomizationManager.Instance.jsonStaticURL),
                    $"exhibition/webinarForVisitor/{CustomizationManager.Instance.id}"
                )
            )
        )
        {
            yield return uwr.SendWebRequest();
            if (!(uwr.result == UnityWebRequest.Result.Success))
            {
                Debug.LogWarning("Failed to request webinar JSON data");
                yield break;
            }
            else
            {
                webinarDataContainer = WebinarDataContainer.FromJson(uwr.downloadHandler.text);
                PopulateWebinar();
            }
        }
    }

    void PopulateWebinar()
    {
        foreach (WebinarVideo webinarVideoData in webinarDataContainer.Data.Videos)
        {
            UIWebinarVideoEntry instantiatedWebinarVideo = Instantiate(WebinarVideoPrefab, Content)
                .GetComponent<UIWebinarVideoEntry>();
            // by setting data we also set callbacks to that webinar video is clicked
# if DEBUG_AUDITORIUM_VIDEO
            webinarVideoData.VideoDownloadUrl =
                "https://dl6.webmfiles.org/big-buck-bunny_trailer.webm";
# endif
            instantiatedWebinarVideo.WebinarVideoData = webinarVideoData;
            instantiatedWebinarVideo.DescriptionText = description;
        }
    }
}

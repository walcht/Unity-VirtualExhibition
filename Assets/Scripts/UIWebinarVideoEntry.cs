using Common;
using JSONContainer;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIWebinarVideoEntry : MonoBehaviour, IPointerEnterHandler
{
    public InputEventLayer InputLayer;
    public Image Thumbnail;
    public Button EntryButton;
    public TMP_Text buttonText;
    WebinarVideo webinarVideoData;
    TMP_Text _descriptionText;

    void OnButtonClick()
    {
        InputLayer.UIVideoPlayerExitEvent += OnExitUIVideoPlayer;
        UIManager.Instance.HideUIAuditorium();
        UIManager.Instance.ShowUIVideoPlayer(
            new System.Uri(
                new System.Uri(CustomizationManager.Instance.mediaStaticURL),
                webinarVideoData.VideoDownloadUrl
            )
        );
    }

    void OnExitUIVideoPlayer()
    {
        InputLayer.UIVideoPlayerExitEvent -= OnExitUIVideoPlayer;
        UIManager.Instance.HideUIVideoPlayer();
        UIManager.Instance.ShowUIAuditorium();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _descriptionText.text = webinarVideoData.VideoDescription;
    }

    public WebinarVideo WebinarVideoData
    {
        set
        {
            webinarVideoData = value;
            EntryButton.onClick.RemoveAllListeners();
            EntryButton.onClick.AddListener(OnButtonClick);
            // update thumbnail and button text
            if (webinarVideoData.ThumbnailDownloadUrl != null)
            {
                TextureRequestManager.Instance.AddRequest(
                    new RequestData(
                        Thumbnail.gameObject,
                        CustomizationManager.Instance.mediaStaticURL
                            + webinarVideoData.ThumbnailDownloadUrl,
                        true,
                        true
                    )
                );
                TextureRequestManager.Instance.StartServingTextureRequests();
            }
            buttonText.text = webinarVideoData.VideoTitle;
        }
    }

    public TMP_Text DescriptionText
    {
        set { _descriptionText = value; }
    }
}

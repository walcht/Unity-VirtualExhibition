using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIVideoPlayer : MonoBehaviour
{
    public InputEventLayer inputLayer;
    public VideoPlayer webinarVideo;
    public Button exit;
    public Button play_stop;
    public Sprite playButtonSprite;
    public Sprite stopButtonSprite;
    public GameObject ErrorBox;
    public AspectRatioFitter videoAspectRatioFitter;
    public TMPro.TMP_Text totalTime;
    public TMPro.TMP_Text currentTime;
    System.Uri _videoUri;

    private void OnEnable()
    {
        ErrorBox.SetActive(false);
        play_stop.interactable = false;
        StartCoroutine(Init());
        play_stop.GetComponent<Image>().sprite = playButtonSprite;
        webinarVideo.loopPointReached += OnVideoEnd;
    }

    private string GetVideoTimeText(double timeInSecs)
    {
        if (timeInSecs >= 3600)
            return System.TimeSpan.FromSeconds(timeInSecs).ToString(@"hh\:mm\:ss");
        return System.TimeSpan.FromSeconds(timeInSecs).ToString(@"mm\:ss");
    }

    private IEnumerator Init()
    {
        webinarVideo.errorReceived += OnError;
        webinarVideo.url = _videoUri.AbsoluteUri;
        webinarVideo.Prepare();
        yield return new WaitUntil(() => _videoUri != null);
        yield return new WaitUntil(() => webinarVideo.isPrepared);
        totalTime.text = GetVideoTimeText(webinarVideo.length);
        play_stop.interactable = true;
        play_stop.onClick.AddListener(OnPlayStop);
        exit.onClick.AddListener(() => inputLayer.InvokeUIVideoPlayerExitEvent());
        inputLayer.UIPlayStopVideoEvent += OnPlayStop;
        OnPlayStop();
    }

    private IEnumerator TimerUpdate()
    {
        while (true)
        {
            double currentDurration =
                webinarVideo.length * (webinarVideo.frame / (float)webinarVideo.frameCount);
            currentTime.text = GetVideoTimeText(currentDurration);
            yield return new WaitForSeconds(1);
        }
    }

    private void OnError(VideoPlayer source, string message)
    {
        ErrorBox.SetActive(true);
        play_stop.onClick.RemoveAllListeners();
        play_stop.GetComponent<Image>().sprite = playButtonSprite;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        totalTime.text = "-";
        currentTime.text = "-";
        _videoUri = null;
        webinarVideo.Stop();
        webinarVideo.errorReceived -= OnError;
        play_stop.onClick.RemoveAllListeners();
        exit.onClick.RemoveAllListeners();
        inputLayer.UIPlayStopVideoEvent -= OnPlayStop;
    }

    void OnPlayStop()
    {
        if (webinarVideo.isPlaying)
        {
            webinarVideo.Pause();
            play_stop.GetComponent<Image>().sprite = playButtonSprite;
            StopCoroutine(TimerUpdate());
            return;
        }
        webinarVideo.Play();
        play_stop.GetComponent<Image>().sprite = stopButtonSprite;
        StartCoroutine(TimerUpdate());
    }

    private void OnVideoEnd(VideoPlayer source)
    {
        webinarVideo.Pause();
        play_stop.GetComponent<Image>().sprite = playButtonSprite;
    }

    public System.Uri VideoURI
    {
        set { _videoUri = value; }
    }
}

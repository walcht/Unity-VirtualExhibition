using UnityEngine;
using UnityEngine.UI;

public class UIRequest : MonoBehaviour
{
    public Slider slider;
    public TMPro.TMP_Text percentage;

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        slider.value = TextureRequestManager.Instance.GetDownloadProgress();
        percentage.text = string.Format(
            "{0}%",
            Mathf.Round(TextureRequestManager.Instance.GetDownloadProgress() * 100)
        );
    }
}

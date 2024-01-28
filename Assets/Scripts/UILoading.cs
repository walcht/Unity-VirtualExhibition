using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    [Tooltip("Header message in startup UI")]
    public TMPro.TMP_Text mainMsg;

    [Tooltip("Loading progress slider")]
    public Slider loadingProgress;

    [Tooltip("Logging message field")]
    public TMPro.TMP_Text logOutput;

    [Tooltip("Progress text under progress bar")]
    public TMPro.TMP_Text progText;
}

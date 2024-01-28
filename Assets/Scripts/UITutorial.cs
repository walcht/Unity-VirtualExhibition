using UnityEngine;
using UnityEngine.UI;

public class UITutorial : MonoBehaviour
{
    public InputEventLayer InputLayer;
    public Button exit;

    void Awake()
    {
        exit.onClick.AddListener(OnExitClick);
    }

    private void OnEnable()
    {
        exit.Select();
    }

    void OnExitClick() => InputLayer.InvokeUITutorialExitEvent();
}

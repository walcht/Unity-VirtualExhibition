using TMPro;
using UnityEngine;

public class UIDimensionPopup : MonoBehaviour
{
    [SerializeField]
    private TMP_Text height;

    [SerializeField]
    private TMP_Text width;

    public void SetDimension(int _height, int _width)
    {
        height.text = _height.ToString() + " px";
        width.text = _width.ToString() + " px";
    }
}

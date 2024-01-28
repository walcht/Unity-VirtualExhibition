using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewCanvasWorldCam : MonoBehaviour
{
    public Canvas[] ChildCanvas;
    private void OnEnable()
    {
        Camera tmp = Camera.main.GetComponentInChildren<Camera>();
        foreach (var canvas in ChildCanvas)
        {
            canvas.worldCamera = tmp;
        }
    }
}

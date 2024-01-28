using UnityEngine;

public class LookAtMainCam : MonoBehaviour
{
    private void Update() => transform.LookAt(Camera.main.transform);
}

using UnityEngine;

public class SponsorRotation : MonoBehaviour
{
    public float RotationSpeed = 80; // in degrees per second

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * RotationSpeed, Space.Self);
    }
}

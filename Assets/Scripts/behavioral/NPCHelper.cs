using UnityEngine;

/// <summary>
///     Monobehaviour that provides helper utilities for the attached NPC agent.
/// </summary>
public class NPCHelper : MonoBehaviour
{
    public Transform    hand;
    public GameObject   phone;
    public Vector3      phoneLocalPosition;
    public Quaternion   phoneLocalRotation;
    [HideInInspector]   public float        angularVelocityY = 0;

    GameObject          _instantiatedPhone;
    Vector3             _lastForward;

    private void Awake()
    {
        _lastForward = transform.forward;
    }

    private void Update()
    {
        angularVelocityY = Mathf.Deg2Rad * Vector3.SignedAngle(_lastForward, transform.forward, transform.up) / Time.deltaTime;
        _lastForward = transform.forward;
    }

    /// <summary>
    ///     Instantiates a phone in the right hand of the attached NPC
    /// </summary>
    public void InstantiatePhone() 
    {
        if (_instantiatedPhone != null)
            return;

        _instantiatedPhone = Instantiate<GameObject>(phone, hand);
        _instantiatedPhone.transform.localPosition = phoneLocalPosition;
        _instantiatedPhone.transform.localRotation = phoneLocalRotation;
    }

    /// <summary>
    ///     Attempts to destroy the previously instantiated phone GameObject
    /// </summary>
    public void DestroyPhone() {if (_instantiatedPhone != null) Destroy(_instantiatedPhone); }
}
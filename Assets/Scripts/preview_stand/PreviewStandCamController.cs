using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using DesignPatterns;

public class PreviewStandCamController : Singleton<PreviewStandCamController>
{
    public float NavigationSpeed = 10.00f;    // speed of camera rotation in: degrees-per-second 

    public InputActionAsset asset;

    public NavigationButton LeftNavButton;
    public NavigationButton RightNavButton;

    float _maxVerticalCamRotation = 60.00f;
    float _minVerticalCamRotation = 300.00f;

    Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void LateUpdate()
    {
        if (LeftNavButton.IsPressedDown()) UpdateCamTransform(-1.00f);
        if (RightNavButton.IsPressedDown()) UpdateCamTransform(+1.00f);
    }

    private void UpdateCamTransform(float whichDirectionToNavigate)
    {
        // Vertical axis rotation
        float verticalRotationValue = whichDirectionToNavigate * NavigationSpeed * Time.deltaTime;
        float willBeRotationAroundVertical = transform.eulerAngles.y + verticalRotationValue;
            
        if ((willBeRotationAroundVertical > _maxVerticalCamRotation) && (willBeRotationAroundVertical <= 180.00f))
        {
            willBeRotationAroundVertical = _maxVerticalCamRotation;
        } else if ((willBeRotationAroundVertical < _minVerticalCamRotation) && (willBeRotationAroundVertical > 180.00f))
        {
            willBeRotationAroundVertical = _minVerticalCamRotation;
        }

        transform.rotation = Quaternion.Euler(0f, willBeRotationAroundVertical, 0f);
    }

    /// <summary>
    ///     Sets camera rotation angle limits to that of XL stand.
    /// </summary>
    public void SetXLCamProperties()
    {
        _maxVerticalCamRotation = 60.00f;
        _minVerticalCamRotation = 300.00f;
        mainCamera.fieldOfView = 50.00f;
    }
    /// <summary>
    ///     Sets camera rotation angle limits to that of LL stand.
    /// </summary>
    public void SetLLCamProperties()
    {
        _maxVerticalCamRotation = 60.00f;
        _minVerticalCamRotation = 359.00f;
        mainCamera.fieldOfView = 35.00f;
    }
    /// <summary>
    ///     Sets camera rotation limits to that of LR stand.
    /// </summary>
    public void SetLRCamProperties()
    {
        _maxVerticalCamRotation = 1.00f;
        _minVerticalCamRotation = 300.00f;
        mainCamera.fieldOfView = 35.00f;
    }
    /// <summary>
    ///     Sets camera rotation limits to that of M stand.
    /// </summary>
    public void SetMCamProperties()
    {
        _maxVerticalCamRotation = 60.00f;
        _minVerticalCamRotation = 300.00f;
        mainCamera.fieldOfView = 35.00f;
    }
    /// <summary>
    ///     Sets camera rotation limits to that of S stand.
    /// </summary>
    public void SetSCamProperties()
    {
        _maxVerticalCamRotation = 60.00f;
        _minVerticalCamRotation = 300.00f;
        mainCamera.fieldOfView = 35.00f;
    }
}

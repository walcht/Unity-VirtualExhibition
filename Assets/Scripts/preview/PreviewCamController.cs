using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using Common;

public class PreviewCamController : Singleton<PreviewCamController>
{
    public InputEventLayer          inputEventLayerSO;
    CamControllerSettings           _setting;
    Quaternion                      _currentSettingsDefaultRotation;

    public CamControllerSettings Settings 
    {
        get
        {
            return _setting;
        }

        set
        {
            if (_setting != null) _setting.rotateAround.localRotation = _currentSettingsDefaultRotation;

            _setting = value;

            _currentSettingsDefaultRotation = _setting.rotateAround.localRotation;


            transform.SetParent(_setting.rotateAround);
            transform.localRotation = Quaternion.identity;
            transform.localPosition = new Vector3(0.0f, 0.0f, -_setting.distanceToRotateAround);

        }
    }

    private void LateUpdate()
    {
        if (PreviewUIManager.Instance.LeftNavButton.IsPressedDown())
        {
            UpdateHorizontalCamRotaion(-_setting.rotationSpeed * Time.fixedDeltaTime);
            return;
        }

        if (PreviewUIManager.Instance.RightNavButton.IsPressedDown())
        {
            UpdateHorizontalCamRotaion(_setting.rotationSpeed * Time.fixedDeltaTime);
            return;
        }
    }

    private void UpdateHorizontalCamRotaion(float horizontalRotationValue)
    {
        float currentHorizontalAngle = _setting.rotateAround.localRotation.eulerAngles.y;
        float willBeHorizontalRotation = (currentHorizontalAngle + horizontalRotationValue) % 360;

        // please find a better way to do this in the future!
        if (_setting.minHorizontalAngle >= _setting.maxHorizontalAngle)
        {
            if (    (willBeHorizontalRotation >= _setting.minHorizontalAngle)
                    || (willBeHorizontalRotation <= _setting.maxHorizontalAngle)     )
                _setting.rotateAround.Rotate(_setting.rotateAround.up, horizontalRotationValue);
        }
        else
        {
            if (    (willBeHorizontalRotation >= _setting.minHorizontalAngle)
                    && (willBeHorizontalRotation <= _setting.maxHorizontalAngle)     )
                _setting.rotateAround.Rotate(_setting.rotateAround.up, horizontalRotationValue);
        }     
    }

    private void UpdateVerticalCamRotation(float verticalRotationValue)
    {
        float currentHorizontalAngle = _setting.rotateAround.localRotation.eulerAngles.x;
        float willBeHorizontalRotation = (currentHorizontalAngle + verticalRotationValue) % 360;

        // please find a better way to do this in the future!
        if (_setting.minHorizontalAngle >= _setting.maxHorizontalAngle)
        {
            if ((willBeHorizontalRotation > _setting.minHorizontalAngle)
                    || (willBeHorizontalRotation < _setting.maxHorizontalAngle))
                _setting.rotateAround.Rotate(_setting.rotateAround.right, verticalRotationValue);
        }
        else
        {
            if ((willBeHorizontalRotation > _setting.minHorizontalAngle)
                && (willBeHorizontalRotation < _setting.maxHorizontalAngle))
                _setting.rotateAround.Rotate(_setting.rotateAround.right, verticalRotationValue);
        }
    }
}

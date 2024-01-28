using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class NavigationButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool _isPressedDown = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressedDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressedDown = false;
    }

    public bool IsPressedDown() => _isPressedDown;
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInfoDeskEntry : MonoBehaviour, ISelectHandler
{
    public event Action onSelectEvent;

    public void OnSelect(BaseEventData _)
    {
        onSelectEvent?.Invoke();
    }
}

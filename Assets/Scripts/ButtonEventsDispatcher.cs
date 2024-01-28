using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
///     Add to a button for a set of UnityEvent dispatchers.
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonEventsDispatcher : MonoBehaviour, ISelectHandler
{
    public UnityEvent OnSelectEvent;

    public void OnSelect(BaseEventData baseEventData)
    {
        OnSelectEvent?.Invoke();
    }
}

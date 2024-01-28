using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemScript : MonoBehaviour
{
    private void OnEnable()
    {
        if (EventSystem.current != null)
            Destroy(this);
    }
}

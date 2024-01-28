using UnityEngine;
using UnityEngine.EventSystems;

public class DimensionPopupBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int height;
    public int width;
    public GameObject UIDimensionPopup;

    GameObject instantiatedUIDimensionPopup;
    UIDimensionPopup UIDimensionPopupComponent;

    private void Start()
    {
        instantiatedUIDimensionPopup = Instantiate<GameObject>(
            UIDimensionPopup,
            Vector3.zero,
            Quaternion.Euler(0.00f, 180.00f, 0.00f)
        );
        UIDimensionPopupComponent = instantiatedUIDimensionPopup.GetComponent<UIDimensionPopup>();
        UIDimensionPopupComponent.SetDimension(height, width);

        instantiatedUIDimensionPopup.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData _eventData)
    {
        instantiatedUIDimensionPopup.SetActive(true);
        instantiatedUIDimensionPopup.transform.position = _eventData
            .pointerCurrentRaycast
            .worldPosition;

        Vector3 newDirection = Vector3.RotateTowards(
            instantiatedUIDimensionPopup.transform.forward,
            Camera.main.transform.forward,
            Mathf.Infinity,
            0.0f
        );
        instantiatedUIDimensionPopup.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public void OnPointerExit(PointerEventData _eventData)
    {
        instantiatedUIDimensionPopup.SetActive(false);
    }
}

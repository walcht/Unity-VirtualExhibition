using Common;
using UnityEngine;
using UnityEngine.EventSystems;

public class DimensionPopupStaticBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public StandRelatedData StandRelatedDataSO;
    public CustomizableArea CustomizableArealID;

    Dimension current_area_dimensions = new Dimension();

    private void Start()
    {
        current_area_dimensions = StandRelatedDataSO.GetAreaDimensions(CustomizableArealID);
    }

    public void OnPointerEnter(PointerEventData _eventData) =>
        PreviewUIManager.Instance.ShowDimensionPopupUI(
            current_area_dimensions.width,
            current_area_dimensions.height
        );

    public void OnPointerExit(PointerEventData _eventData) =>
        PreviewUIManager.Instance.HideDimensionPopupUI();
}

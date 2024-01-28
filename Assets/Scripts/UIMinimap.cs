using System.Collections;
using Common;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Deals with everything related to minimap UI.
/// </summary>
public class UIMinimap : MonoBehaviour
{
    public InputEventLayer InputLayer;
    public float updateFrequency; // updates per second = 1/T
    public GameObject mapCursor; // this is the cursor that points to the main character's position
    public RawImage image;
    public RectTransform imageTransform;

    Transform mainCharacter; // useful for determining main character's position and rotation

    // Check W and H fields on the RawImage component of the minimap,
    // these values specify the amount of zoom to perform on the map texture
    // The WorldOrigin (that is Coordinate (0, 0, 0)) SHOULD corresponds
    // to XY = 0.00 when WH = 1.00 (these fields are on the minimap's RawImage component)
    public float ZoomIN_WH = 0.20f;
    public float ZoomOUT_WH = 0.50f;

    // holds current map settings
    float currentGTMUnit;

    float updatePeriod;

    UIMapMode currentMapMode = UIMapMode.ZOOMED_IN;

    private void OnEnable()
    {
        InputLayer.UIMapChangeModeEvent += OnChangeMapMode;
        updatePeriod = 1.00f / updateFrequency;
        StartUpdatingMapUI(ZoomIN_WH);
    }

    private void OnDisable()
    {
        InputLayer.UIMapChangeModeEvent -= OnChangeMapMode;
        StopUpdatingMapUI();
    }

    /// <summary>
    ///     Call this to Zoom-IN or Zoom-OUT on the minimap
    /// </summary>
    void OnChangeMapMode()
    {
        if (currentMapMode == UIMapMode.ZOOMED_IN)
        {
            currentMapMode = UIMapMode.ZOOMED_OUT;
            RefreshUpdatingMapUI(ZoomOUT_WH);
            return;
        }
        currentMapMode = UIMapMode.ZOOMED_IN;
        RefreshUpdatingMapUI(ZoomIN_WH);
    }

    /// <summary>
    ///     Call this function to start updating map UI.
    /// </summary>
    /// <param name="wh_value">WH value of the W and H fileds on the minimap's RawImage component.</param>
    public void StartUpdatingMapUI(float wh_value)
    {
        StartCoroutine(UpdateMapUI(wh_value));
    }

    /// <summary>
    ///     Call this function whenever a change in map mode occurs
    /// </summary>
    /// <param name="mapModeSettings">map mode settings for setting Raw Image's rect component</param>
    public void RefreshUpdatingMapUI(float wh_value)
    {
        StopAllCoroutines();
        StartCoroutine(UpdateMapUI(wh_value));
    }

    /// <summary>
    ///     Call this function to stop updating map UI
    /// </summary>
    public void StopUpdatingMapUI()
    {
        StopAllCoroutines();
    }

    /// <summary>
    ///     Sets minimap texture.
    /// </summary>
    /// <param name="texture">Texture to use in the minimap</param>
    /// <param name="GTMUnit">GTM stands for how much the cursor moves on the minimap for 1 meter of movement in game.
    /// Each texture has its own GTM that mainly depends on the size of the texture.</param>
    public void SetMapImage(Texture texture, float GTMUnit)
    {
        image.texture = texture;
        currentGTMUnit = GTMUnit;
    }

    /// <summary>
    ///     Sets the reference to the main character's transform component.
    /// </summary>
    /// <param name="mainCharacter">Main character's transform component</param>
    public void SetMainCharacterTransform(Transform mainCharacter)
    {
        this.mainCharacter = mainCharacter;
    }

    IEnumerator UpdateMapUI(float wh_value)
    {
        // to avoid unnecessary re-calculations in the loop
        Vector2 current_xy_offsets = new Vector2(
            wh_to_xy_offsets(wh_value),
            wh_to_xy_offsets(wh_value)
        );

        Rect RectToApply = new Rect
        {
            x = current_xy_offsets.x,
            y = current_xy_offsets.y,
            width = wh_value,
            height = wh_value
        };

        while (true)
        {
            if (mainCharacter == null)
                yield return new WaitUntil(() => mainCharacter != null); // wait until mainCharacter Transform component is assigned

            // rotating the map so that it always faces forward
            imageTransform.rotation = Quaternion.Euler(
                0,
                0,
                mainCharacter.rotation.eulerAngles.y + 90
            );

            // offsets the map (without using any mask) by changing UV coordinates of map's Raw Image component
            RectToApply.x = (currentGTMUnit * mainCharacter.position.z) + current_xy_offsets.x;
            RectToApply.y = (-currentGTMUnit * mainCharacter.position.x) + current_xy_offsets.y;

            image.uvRect = RectToApply;
            yield return new WaitForSecondsRealtime(updatePeriod);
        }
    }

    /// <summary>
    ///     Used to correctly transform the current map texture's X and Y offsets.
    /// </summary>
    /// <param name="x">X or Y offset of the current map</param>
    /// <returns></returns>
    float wh_to_xy_offsets(float wh) => wh * (-0.5f) + 0.5f;
}

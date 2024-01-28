using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonNotification : MonoBehaviour
{
    // References child image object that holds button 2D sprite
    public Image ButtonImage;

    // References button text message, why-press-this-button text
    public TMP_Text WhyPressButtonText;

    // References ScriptableObject that holds button sprites
    public ButtonSprites ButtonSpritesSO;

    /// <summary>
    ///     Sets why-press-this-button text
    /// </summary>
    /// <param name="text"></param>
    public void SetDescriptiveText(string text)
    {
        WhyPressButtonText.text = text;
    }

    /// <summary>
    ///     Sets button 2D Sprite.
    /// </summary>
    /// <param name="buttonSprite">Which button sprite to use.</param>
    public void SetButtonSprite(ButtonSprite buttonSprite)
    {
        ButtonImage.sprite = ButtonSpritesSO.GetInputSprite(buttonSprite);
    }
}

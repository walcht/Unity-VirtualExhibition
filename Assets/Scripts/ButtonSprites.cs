using Common;
using UnityEngine;

[CreateAssetMenu(fileName = "ButtonSprites", menuName = "SO/ButtonSprites")]
/// <summary>
///     ScriptableObject for 2D button sprites.
/// </summary>
public class ButtonSprites : ScriptableObject
{
    public Sprite KEYBOARD_ENTER_SPRITE;
    public Sprite KEYBOARD_SPACE_SPRITE;
    public Sprite KEYBOARD_ESC_SPRITE;
    public Sprite KEYBOARD_V_SPRITE;
    public Sprite KEYBOARD_E_SPRITE;
    public Sprite KEYBOARD_M_SPRITE;
    public Sprite KEYBOARD_WASD_SPRITE;
    public Sprite KEYBOARD_ARROWS_SPRITE;

    public Sprite KEYBOARD_SPRITE;
    public Sprite MOUSE_SPRITE;

    public Sprite GetInputSprite(ButtonSprite buttonSprite)
    {
        switch (buttonSprite)
        {
            case ButtonSprite.KEYBOARD_ENTER:
                return KEYBOARD_ENTER_SPRITE;

            case ButtonSprite.KEYBOARD_SPACE:
                return KEYBOARD_SPACE_SPRITE;

            case ButtonSprite.KEYBOARD_ESC:
                return KEYBOARD_ESC_SPRITE;

            case ButtonSprite.KEYBOARD_V:
                return KEYBOARD_V_SPRITE;

            case ButtonSprite.KEYBOARD_E:
                return KEYBOARD_E_SPRITE;

            case ButtonSprite.KEYBOARD_M:
                return KEYBOARD_M_SPRITE;

            case ButtonSprite.KEYBOARD_WASD:
                return KEYBOARD_WASD_SPRITE;

            case ButtonSprite.KEYBOARD_ARROWS:
                return KEYBOARD_ARROWS_SPRITE;

            case ButtonSprite.KEYBOARD:
                return KEYBOARD_SPRITE;

            case ButtonSprite.MOUSE:
                return MOUSE_SPRITE;

            default:
                return null;
        }
    }
}

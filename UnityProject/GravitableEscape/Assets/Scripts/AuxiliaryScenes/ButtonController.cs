using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Handles sprite changes for a UI button based on user interactions.
/// Implements hover, click, and release states using defined sprites.
/// </summary>
/// <remarks>
/// Requires an <see cref="Image"/> component and three sprites:
/// - <see cref="defaultSprite"/>: Default button state.
/// - <see cref="activeSprite"/>: Hover/active button state.
/// - <see cref="pushedSprite"/>: Clicked button state.
/// </remarks>
public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Sprite defaultSprite;  // Default state sprite
    public Sprite activeSprite;   // Sprite for hover/active state
    public Sprite pushedSprite;   // Sprite for the pushed state

    private Image buttonImage;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonImage.sprite = defaultSprite; // Set the default sprite initially
    }

    // Pointer enters the button (active state)
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = activeSprite;
    }

    // Pointer exits the button (return to default state)
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = defaultSprite;
    }

    // Pointer clicks the button (pushed state)
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonImage.sprite = pushedSprite;
    }

    // Pointer releases the button (return to active state if still hovering)
    public void OnPointerUp(PointerEventData eventData)
    {
        buttonImage.sprite = activeSprite;
    }
}
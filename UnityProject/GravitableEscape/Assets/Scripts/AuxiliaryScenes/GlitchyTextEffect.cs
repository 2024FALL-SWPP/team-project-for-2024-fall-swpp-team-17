using TMPro;
using UnityEngine;

/// <summary>
/// Adds a glitchy visual effect to a TextMeshProUGUI component. 
/// Periodically manipulates text color, style, and position to simulate glitches.
/// This is used in Title, Menu scene's "GRAVITABLE ESCAPE" text to add glitchy effect
/// </summary>
public class GlitchyTextEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    private string originalText;
    private Color originalColor;
    private Color transparent = new Color(1f, 1f, 1f, 0f);
    private bool isGlitching = false;
    private RectTransform textRectTransform; // RectTransform for position manipulation
    private Vector3 originalPosition;
    private bool originalBold;


    private void Start()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        if (textMeshPro != null)
        {
            originalText = textMeshPro.text;
            originalColor = textMeshPro.color;
            textRectTransform = textMeshPro.GetComponent<RectTransform>();
            originalPosition = textRectTransform.localPosition;

            InvokeRepeating(nameof(Glitch), 8f, 8f);
        }
        else
        {
            Debug.LogError("No TextMeshPro component found.");
        }
    }

    private void Glitch()
    {
        if (!isGlitching)
            StartCoroutine(GlitchCoroutine());
    }

    private System.Collections.IEnumerator GlitchCoroutine()
    {
        isGlitching = true;
        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.03f);
        textMeshPro.fontStyle = FontStyles.Italic;
        textMeshPro.color = originalColor;
        yield return new WaitForSeconds(0.09f);
        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.07f);
        textMeshPro.color = originalColor;
        yield return new WaitForSeconds(0.05f);
        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.1f);
        textMeshPro.fontStyle = FontStyles.Normal;
        textRectTransform.localPosition = originalPosition;
        textMeshPro.color = originalColor;

        yield return new WaitForSeconds(4f);

        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.03f);
        textMeshPro.fontStyle = FontStyles.Italic;
        textMeshPro.color = originalColor;
        yield return new WaitForSeconds(0.09f);
        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.07f);
        textMeshPro.fontStyle = FontStyles.Normal;
        textMeshPro.color = originalColor;
        yield return new WaitForSeconds(0.05f);
        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.03f);
        textMeshPro.fontStyle = FontStyles.Italic;
        textMeshPro.color = originalColor;
        yield return new WaitForSeconds(0.05f);
        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.2f);
        textMeshPro.fontStyle = FontStyles.Normal;
        textMeshPro.color = originalColor;

        isGlitching = false;

    }
}
using TMPro;
using UnityEngine;

public class GlitchyTextEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    // public float glitchInterval = 0.5f; // Interval between glitches in seconds
    // public Color glitchColor;       // Colors to switch between during glitch
    public Vector2 scaleRange = new Vector2(0.9f, 1.1f); // Scale range for glitch effect
    public Vector2 letterSpacingRange = new Vector2(-50f, 50f); // Letter spacing range for glitch effect

    private string originalText;
    private Color originalColor;
    private Color transparent = new Color(1f, 1f, 1f, 0f);
    private Color glitchColor = new Color(0f, 1f, 1f, 1f);
    private float originalFontSize;
    private float originalLetterSpacing;
    private bool isGlitching = false;

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
            // originalFontSize = textMeshPro.fontSize;
            // originalLetterSpacing = textMeshPro.characterSpacing;

            InvokeRepeating(nameof(Glitch), 1f, 8f);
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
        textMeshPro.color = originalColor;
        yield return new WaitForSeconds(0.09f);
        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.07f);
        textMeshPro.color = originalColor;
        yield return new WaitForSeconds(0.05f);
        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.1f);
        textMeshPro.color = originalColor;

        yield return new WaitForSeconds(4f);

        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.03f);
        textMeshPro.color = originalColor;
        yield return new WaitForSeconds(0.09f);
        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.07f);
        textMeshPro.color = originalColor;
        yield return new WaitForSeconds(0.05f);
        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.03f);
        textMeshPro.color = originalColor;
        yield return new WaitForSeconds(0.05f);
        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.2f);
        textMeshPro.color = originalColor;


        // textMeshPro.color = glitchColor;
        // yield return new WaitForSeconds(0.02f);
        // textMeshPro.color = transparent;
        // yield return new WaitForSeconds(0.65f);
        // textMeshPro.color = originalColor;
        // yield return new WaitForSeconds(2f);
        // textMeshPro.color = glitchColor;
        // yield return new WaitForSeconds(0.03f);
        // textMeshPro.color = transparent;
        // yield return new WaitForSeconds(0.3f);
        // textMeshPro.color = originalColor;

        // yield return new WaitForSeconds(7f);

        // textMeshPro.color = glitchColor;
        // yield return new WaitForSeconds(0.01f);
        // textMeshPro.color = transparent;
        // yield return new WaitForSeconds(0.15f);
        // textMeshPro.color = originalColor;
        // yield return new WaitForSeconds(0.3f);
        // textMeshPro.color = glitchColor;
        // yield return new WaitForSeconds(0.03f);
        // textMeshPro.color = transparent;
        // yield return new WaitForSeconds(0.1f);
        // textMeshPro.color = originalColor;

        isGlitching = false;


        // Apply glitch effects
        // textMeshPro.fontSize = originalFontSize * Random.Range(scaleRange.x, scaleRange.y);
        // textMeshPro.characterSpacing = Random.Range(letterSpacingRange.x, letterSpacingRange.y);

        // Wait for half the interval before reverting to normal

        // Reset to original state
        // textMeshPro.fontSize = originalFontSize;
        // textMeshPro.characterSpacing = originalLetterSpacing;

    }
}
using TMPro;
using UnityEngine;

public class GlitchyTextEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    private string originalText;
    private Color originalColor;
    private Color transparent = new Color(1f, 1f, 1f, 0f);
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

        isGlitching = false;

    }
}
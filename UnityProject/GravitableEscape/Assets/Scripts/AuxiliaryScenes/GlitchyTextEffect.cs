using TMPro;
using UnityEngine;

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
        // isGlitching = true;
        // textMeshPro.color = originalColor;
        // yield return new WaitForSeconds(1f);
        // Vector3 randomOffset = new Vector3(-30f, 0f, 0f);
        // textRectTransform.localPosition = originalPosition + new Vector3(-40f, 0f, 0f);
        // textMeshPro.color = originalColor;
        // yield return new WaitForSeconds(1f);
        // textRectTransform.localPosition = originalPosition;
        // yield return new WaitForSeconds(1f);
        // textRectTransform.localPosition = originalPosition + new Vector3(30f, 0f, 0f);


        isGlitching = true;
        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.03f);
        //textRectTransform.localPosition = originalPosition + new Vector3(-10f, 0f, 0f);
        textMeshPro.fontStyle = FontStyles.Italic;
        textMeshPro.color = originalColor;
        yield return new WaitForSeconds(0.09f);
        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.07f);
        //textRectTransform.localPosition = originalPosition + new Vector3(15f, 0f, 0f);
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
        //textRectTransform.localPosition = originalPosition + new Vector3(10f, 0f, 0f);
        textMeshPro.fontStyle = FontStyles.Italic;
        textMeshPro.color = originalColor;
        yield return new WaitForSeconds(0.09f);
        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.07f);
        //textRectTransform.localPosition = originalPosition + new Vector3(-5f, 0f, 0f);
        textMeshPro.fontStyle = FontStyles.Normal;
        textMeshPro.color = originalColor;
        yield return new WaitForSeconds(0.05f);
        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.03f);
        textRectTransform.localPosition = originalPosition + new Vector3(3f, 0f, 0f);
        //textMeshPro.fontStyle = FontStyles.Italic;
        textMeshPro.color = originalColor;
        yield return new WaitForSeconds(0.05f);
        textMeshPro.color = transparent;
        yield return new WaitForSeconds(0.2f);
        textMeshPro.fontStyle = FontStyles.Normal;
        textMeshPro.color = originalColor;

        isGlitching = false;

    }
}
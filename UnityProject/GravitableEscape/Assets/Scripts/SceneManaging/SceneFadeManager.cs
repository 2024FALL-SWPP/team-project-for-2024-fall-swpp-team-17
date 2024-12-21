using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the fade-in effect for scenes using a UI Image overlay.
/// Gradually decreases the alpha of the fade image to create a smooth transition effect.
/// </summary>
/// <remarks>
/// Attach this script to a GameObject with an <see cref="Image"/> component assigned to <see cref="fadeImage"/>.
/// Ensure the fade image covers the entire screen for a seamless fade effect.
/// </remarks>
public class SceneFadeManager : MonoBehaviour
{
    public Image fadeImage; // The UI Image used for the fade effect
    private float fadeSpeed = 0.4f; // Speed of the fade-in effect

    void Start()
    {
        // Begin the fade-in effect when the scene starts
        StartCoroutine(FadeIn());
    }

    /// <summary>
    /// Gradually fades in the scene by decreasing the alpha of the fade image.
    /// </summary>
    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(1f); // Wait briefly before starting the fade-in
        float alpha = 1f;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed; // Decrease alpha over time
            SetAlpha(alpha);
            yield return null; // Wait for the next frame
        }
    }

    /// <summary>
    /// Sets the alpha transparency of the fade image.
    /// </summary>
    /// <param name="alpha">The alpha value to set, ranging from 0 (transparent) to 1 (opaque).</param>
    private void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha; // Update the alpha channel
        fadeImage.color = color;
    }
}
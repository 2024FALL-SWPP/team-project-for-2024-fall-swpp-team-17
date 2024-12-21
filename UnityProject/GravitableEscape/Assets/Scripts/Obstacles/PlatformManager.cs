using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages the activation and visual state of platform groups.
/// Toggles between active, caution, and inactive states for two platform groups over time.
/// </summary>
/// <remarks>
/// Each platform group alternates between three states:
/// - **Active**: Platforms are fully visible and collidable.
/// - **Caution**: Platforms are semi-transparent and collidable, indicating a state change is imminent.
/// - **Inactive**: Platforms are invisible and non-collidable.
/// </remarks>
public class PlatformManager : MonoBehaviour
{
    public GameObject[] platformGroupA; // First group of platforms
    public GameObject[] platformGroupB; // Second group of platforms

    private float activeInterval = 4.0f;   // Duration of the active state
    private float cautionInterval = 1.0f; // Duration of the caution state
    private float inactiveInterval = 3.0f; // Duration of the inactive state

    private float inactiveAlpha = 0.0f;  // Alpha value for the inactive state
    private float cautionAlpha = 0.3f;  // Alpha value for the caution state
    private float activeAlpha = 1.0f;   // Alpha value for the active state

    private bool isGroupAActive = true; // Tracks the state of platform group A
    private bool isGroupBActive = true; // Tracks the state of platform group B

    void Start()
    {
        StartCoroutine(GroupAToggle());
        StartCoroutine(GroupBToggle());
    }

    /// <summary>
    /// Toggles the states of platform group A cyclically.
    /// </summary>
    private IEnumerator GroupAToggle()
    {
        while (true)
        {
            if (isGroupAActive)
            {
                SetPlatformGroup(platformGroupA, activeAlpha, true);
                yield return new WaitForSeconds(activeInterval);
                SetPlatformGroup(platformGroupA, cautionAlpha, true);
                yield return new WaitForSeconds(cautionInterval);
            }
            else
            {
                SetPlatformGroup(platformGroupA, inactiveAlpha, false);
                yield return new WaitForSeconds(inactiveInterval);
            }
            isGroupAActive = !isGroupAActive;
        }
    }

    /// <summary>
    /// Toggles the states of platform group B cyclically.
    /// </summary>
    private IEnumerator GroupBToggle()
    {
        SetPlatformGroup(platformGroupB, inactiveAlpha, false);
        yield return new WaitForSeconds(4f);
        while (true)
        {
            if (isGroupBActive)
            {
                SetPlatformGroup(platformGroupB, activeAlpha, true);
                yield return new WaitForSeconds(activeInterval);
                SetPlatformGroup(platformGroupB, cautionAlpha, true);
                yield return new WaitForSeconds(cautionInterval);
            }
            else
            {
                SetPlatformGroup(platformGroupB, inactiveAlpha, false);
                yield return new WaitForSeconds(inactiveInterval);
            }
            isGroupBActive = !isGroupBActive;
        }
    }

    /// <summary>
    /// Sets the visual and collision state of a platform group.
    /// </summary>
    /// <param name="platformGroup">The array of platform GameObjects.</param>
    /// <param name="alpha">The alpha value for the visual state.</param>
    /// <param name="isCollider">Whether the platforms should be collidable.</param>
    private void SetPlatformGroup(GameObject[] platformGroup, float alpha, bool isCollider)
    {
        foreach (GameObject platform in platformGroup)
        {
            ChangePlatformVisual(platform, alpha);
            SetCollider(platform, isCollider);
        }
    }

    /// <summary>
    /// Updates the visual state of a platform based on the alpha value.
    /// </summary>
    /// <param name="platform">The platform GameObject.</param>
    /// <param name="alpha">The alpha value for the platform.</param>
    private void ChangePlatformVisual(GameObject platform, float alpha)
    {
        Renderer renderer = platform.GetComponent<Renderer>();
        if (alpha == 0.0f)
        {
            renderer.enabled = false;
        }
        else if (alpha == 1.0f)
        {
            renderer.enabled = true;
            SetAlpha(renderer, alpha);
        }
        else
        {
            SetAlpha(renderer, alpha);
        }
    }

    /// <summary>
    /// Sets the alpha transparency of a platform's material.
    /// </summary>
    /// <param name="renderer">The renderer of the platform.</param>
    /// <param name="alpha">The alpha value to apply.</param>
    private void SetAlpha(Renderer renderer, float alpha)
    {
        Color color = renderer.material.color;
        color.a = alpha;
        renderer.material.color = color;
    }

    /// <summary>
    /// Enables or disables the collider of a platform.
    /// </summary>
    /// <param name="platform">The platform GameObject.</param>
    /// <param name="isCollider">Whether the collider should be enabled.</param>
    private void SetCollider(GameObject platform, bool isCollider)
    {
        Collider collider = platform.GetComponent<Collider>();
        collider.enabled = isCollider;
    }
}
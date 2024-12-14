using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject[] platformGroupA;
    public GameObject[] platformGroupB;

    private float activeInterval = 4.0f;
    private float cautionInterval = 1.0f;
    private float inactiveInterval = 3.0f;

    private float inactiveAlpha = 0.0f;
    private float cautionAlpha = 0.3f;
    private float activeAlpha = 1.0f;

    private bool isGroupAActive = true;
    private bool isGroupBActive = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GroupAToggle());
        StartCoroutine(GroupBToggle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    private void SetPlatformGroup(GameObject[] platformGroup, float alpha, bool isCollider)
    {
        foreach(GameObject platform in platformGroup)
        {
            ChangePlatformVisual(platform, alpha);
            SetCollider(platform, isCollider);
        }
    }

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

    private void SetAlpha(Renderer renderer, float alpha)
    {
        Color color = renderer.material.color;
        color.a = alpha;
        renderer.material.color = color;
    }

    private void SetCollider(GameObject platform, bool isCollider)
    {
        Collider collider = platform.GetComponent<Collider>();
        collider.enabled = isCollider;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject[] platformGroupA;
    public GameObject[] platformGroupB;

    private float activeInterval = 5f;
    private float inactiveInterval = 3f;
    private float inactiveAlpha = 0.3f;
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
            SetAlpha(platform, alpha);
            SetCollider(platform, isCollider);
        }
    }

    private void SetAlpha(GameObject platform, float alpha)
    {
        Renderer renderer = platform.GetComponent<Renderer>();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraManager cameraManager;
    public GameObject player, wormhole;
    public Vector3 wormholeTargetPos;
    void Start()
    {
        cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// This function is called by WormholeManager when player is close enough to the wormhole.
    /// It triggers animation of warping into the wormhole.
    /// </summary>
    /// <param name="wormhole">transform of the wormhole object</param>
    /// <param name="targetPos">position to move after animation</param>
    public void startWormhole(GameObject wormhole, Vector3 targetPos)
    {
        wormholeTargetPos = targetPos;
        cameraManager.enterWormholeMode(wormhole.transform);
        this.wormhole = wormhole;
    }

    /// <summary>
    /// This function is called when the animation warping into the wormhole ends.
    /// </summary>
    public void exitWormhole()
    {
        wormhole.GetComponent<WormholeManager>().Reset();
        player.transform.position = wormholeTargetPos;
    }
}

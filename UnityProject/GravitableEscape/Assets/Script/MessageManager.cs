using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    private PlayerManager playerManager;
    private GameManager gameManager;
    private UIManager uiManager;

    [System.Serializable]
    public class Zone
    {
        [TextArea]
        public string message;
        public float startZ;
        public float endZ;
    }

    public Zone[] zones;

    private string currentMessage = null;
    private bool isDelayTriggered = false;
    private bool hasMessageShown = false;

    public float gravityMessageStartZ;
    public float gravityMessageEndZ;

    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();

        // Optional validation to ensure all zones have valid bounds
        foreach (var zone in zones)
        {
            if (zone.startZ > zone.endZ)
            {
                Debug.LogError("Invalid zone bounds: startZ is greater than endZ.");
            }
        }
    }

    void Update()
    {
        if (!gameManager.isMessageRequiredScene)
        {
            return;
        }

        Vector3 playerPosition = playerManager.GetPlayerPos();
        float playerZ = playerPosition.z;

        string newMessage = null;

        foreach (var zone in zones)
        {
            if (playerZ >= zone.startZ && playerZ <= zone.endZ)
            {
                newMessage = zone.message;
                if (playerZ <= -80 && !isDelayTriggered && !hasMessageShown)
                {
                    isDelayTriggered = true;
                    hasMessageShown = true;
                    StartCoroutine(CallUIManagerAfterDelay(newMessage));
                }
                break;
            }
        }

        if (newMessage != currentMessage && !isDelayTriggered)
        {
            currentMessage = newMessage;

            if (string.IsNullOrEmpty(currentMessage))
            {
                uiManager.HideMessage();
                hasMessageShown = false;
            }
            else
            {
                uiManager.ShowMessage(currentMessage);
            }
        }

        // Check if gravity-change instructions should be shown
        if (playerZ >= gravityMessageStartZ && playerZ <= gravityMessageEndZ)
        {
            uiManager.ShowGravityDirections();
        }
        else
        {
            uiManager.HideGravityDirections();
        }

    }

    IEnumerator CallUIManagerAfterDelay(string currentMessage)
    {
        yield return new WaitForSeconds(1.5f);
        uiManager.ShowMessage(currentMessage);
        isDelayTriggered = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    private PlayerManager playerManager;
    private GameManager gameManager;
    private UIManager uiManager;

    public string[] zoneMessages;
    public float[] zoneBounds; // z positions of [start1, end1, start2, end2, ...]
    private string currentMessage = null;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();

        if (zoneMessages.Length * 2 != zoneBounds.Length)
        {
            Debug.LogError("Incompatible array size btw MessageManager's ZoneMessages and ZoneBounds.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.isMessageRequiredScene)
        {
            return;
        }

        Vector3 playerPosition = playerManager.GetPlayerPos();
        float playerZ = playerPosition.z;

        string newMessage = null;

        for (int i = 0; i < zoneMessages.Length; i++)
        {
            if (playerZ >= zoneBounds[i * 2] && playerZ <= zoneBounds[i * 2 + 1])
            {
                newMessage = zoneMessages[i];
                break;
            }
        }

        if (newMessage != currentMessage)
        {
            currentMessage = newMessage;

            if (string.IsNullOrEmpty(currentMessage))
            {
                uiManager.HideMessage();
            }
            else
            {
                uiManager.ShowMessage(currentMessage);
            }
        }

    }
}

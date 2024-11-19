using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI lifeText;
    PlayerManager playerManager;
    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeText.text = $"Life: {playerManager.GetLife()}";
    }
}

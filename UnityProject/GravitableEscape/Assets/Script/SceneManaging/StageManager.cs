using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public DoorMovementManager leftDoorManager;
    public DoorMovementManager rightDoorManager;
    public GameObject player;

    private bool doorOpened = false;
    private bool stageCleared = false;

    private float doorOpenPos = 460f;
    private float stageClearPos = 502f;

    // Start is called before the first frame update
    void Start()
    {
        leftDoorManager = GameObject.Find("LeftDoor").gameObject.GetComponent<DoorMovementManager>();
        rightDoorManager = GameObject.Find("RightDoor").gameObject.GetComponent<DoorMovementManager>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.z > 460f && !doorOpened)
        {
            DoorOpen();
        }
        if (player.transform.position.z > stageClearPos && !stageCleared)
        {
            SceneManager.LoadScene(GetNextSceneIndex());
        }

    }

    void DoorOpen()
    {
        leftDoorManager.StartMoving();
        rightDoorManager.StartMoving();
    }

    private int GetNextSceneIndex()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        return currentSceneIndex + 1;
    }
}

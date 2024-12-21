using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearManager : MonoBehaviour
{
    public DoorMovementManager leftDoorManager;
    public DoorMovementManager rightDoorManager;

    private bool stageCleared = false;

    // Start is called before the first frame update
    void Start()
    {
        leftDoorManager = GameObject.Find("LeftDoor").gameObject.GetComponent<DoorMovementManager>();
        rightDoorManager = GameObject.Find("RightDoor").gameObject.GetComponent <DoorMovementManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DoorOpen()
    {
        leftDoorManager.StartMoving();
        rightDoorManager.StartMoving();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!stageCleared)
        {
            if (collision.collider.CompareTag("Player"))
            {
                stageCleared = true;
                DoorOpen();
            }
        }
    }
}

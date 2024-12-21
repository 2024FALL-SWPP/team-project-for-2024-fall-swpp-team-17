using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGame;

public class BoxPuzzleManager : MonoBehaviour, PuzzleInterface
{
    public GameObject[] keyboxes;

    public GameObject[] locks;

    public GameObject[] plates;

    private Vector3[] startPoses = new Vector3[7];
    private Vector3 targetRot = new Vector3(0f, 90f, 0f);
    private float rotationSpeed = 30f;
    private bool isCleared = false;

    private int unlockCount = 0;

    public ButtonManager buttonManager;

    // Start is called before the first frame update
    void Start()
    {
        buttonManager = GameObject.Find("Button").GetComponent<ButtonManager>();
        GetBoxPosition();
        SetBoxesInActive();
        SetLockIDs();
        SetPlatesInActive();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCleared)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRot), rotationSpeed * Time.deltaTime);
        }
    }

    public void GetUnlockSignal(int lockID)
    {
        plates[lockID].SetActive(true);
        IncreaseUnlockCount();
    }

    void SetLockIDs()
    {
        int lockID = 0;
        foreach (GameObject puzzleLock in locks)
        {
            LockManager lockManager = puzzleLock.GetComponent<LockManager>();
            lockManager.SetLockID(lockID);
            lockID++;
        }
    }

    void SetBoxesActive()
    {
        foreach (GameObject box in keyboxes)
        {
            box.SetActive(true);
        }
    }

    void GetBoxPosition()
    {
        for (int index = 0; index < 7; index++)
        {
            startPoses[index] = keyboxes[index].transform.position;
        }
    }

    void SetBoxAtStartPos()
    {
        for (int index = 0; index < 7; index++)
        {
            keyboxes[index].transform.position = startPoses[index];
        }
    }

    void SetBoxesInActive()
    {
        foreach (GameObject box in keyboxes)
        {
            box.SetActive(false);
        }
    }

    void SetPlatesInActive()
    {
        foreach (GameObject plate in plates)
        {
            plate.SetActive(false);
        }
    }

    void IncreaseUnlockCount()
    {
        unlockCount++;
        if (unlockCount == 7)
        {
            PuzzleClear();
        }
    }

    public void PuzzleStart()
    {
        SetBoxesActive();
    }

    public void PuzzleReset()
    {
        StartCoroutine(ResetBox());
    }

    public void PuzzleClear()
    {
        buttonManager.FixButton();
        isCleared = true;
    }

    private IEnumerator ResetBox()
    {
        SetBoxesInActive();
        SetPlatesInActive();
        yield return new WaitForSeconds(0.5f);
        SetBoxAtStartPos();
        SetBoxesActive();
    }
}

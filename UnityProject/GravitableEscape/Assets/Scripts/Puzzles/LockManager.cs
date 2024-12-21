using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGame;

public class LockManager : MonoBehaviour
{
    private PuzzleInterface puzzleInterface;
    public GameObject puzzle;
    private bool isUnlocked = false;
    private int lockID = -1;

    // Start is called before the first frame update
    void Start()
    {
        puzzleInterface = puzzle.GetComponent<PuzzleInterface>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLockID(int idNumber)
    {
        lockID = idNumber;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("key") && !isUnlocked)
        {
            Unlock();
        }
    }

    void Unlock()
    {
        isUnlocked = true;
        puzzleInterface.GetUnlockSignal(lockID);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBoxManager : MonoBehaviour
{
    public MazeManager mazeManager;
    public ButtonManager buttonManager;
    private bool isCleared = false;

    // Start is called before the first frame update
    void Start()
    {
        buttonManager = GameObject.Find("Button").GetComponent<ButtonManager>();
        mazeManager = GameObject.Find("SimpleMaze").GetComponent<MazeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Clear") && !isCleared)
        {
            PuzzleCleared();
        }
    }

    void PuzzleCleared()
    {
        isCleared = true;
        buttonManager.FixButton();
        mazeManager.PuzzleClear();
    }
}

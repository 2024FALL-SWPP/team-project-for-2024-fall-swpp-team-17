using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInterface : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    interface Puzzle
    {
        void PuzzleStart();
        void PuzzleReset();
        void PuzzleClear();
    }
}

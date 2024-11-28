using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGame;

public class MazeManager : MonoBehaviour, PuzzleInterface
{
    public GameObject keyBox;
    private Vector3 startPos;
    private Vector3 targetRot = new Vector3(0f, 90f, 0f);
    private float rotationSpeed = 30f;
    private bool isCleared = false;

    // Start is called before the first frame update
    void Start()
    {
        Transform keyBoxTransform = transform.Find("keybox");
        keyBox = keyBoxTransform.gameObject;
        startPos = keyBoxTransform.position;
        keyBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCleared)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRot), rotationSpeed * Time.deltaTime);
        }
    }

    public void PuzzleStart()
    {
        keyBox.SetActive(true);
    }

    public void PuzzleReset()
    {
        StartCoroutine(ResetBox());
    }

    public void PuzzleClear()
    {
        isCleared = true;
    }

    private IEnumerator ResetBox()
    {
        keyBox.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        keyBox.transform.position = startPos;
        keyBox.SetActive(true);
    }
}

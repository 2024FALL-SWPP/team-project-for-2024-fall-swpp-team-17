using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    private float pressTime = 1f;
    private float timer = 0f;

    private bool isPressed = false;
    private bool isCleared = false;
    private bool firstActivation = true;

    private Vector3 direction;

    private float heightDifference;

    private Transform redButton;
    private Transform greenButton;

    public MazeManager mazeManager;

    // Start is called before the first frame update
    void Start()
    {
        redButton = transform.Find("redbutton");
        greenButton = transform.Find("greenbutton");
        mazeManager = GameObject.Find("SimpleMaze").GetComponent<MazeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCleared)
        {
            if (isPressed)
            {
                timer += Time.deltaTime;
                ButtonPressed();

                if (timer > pressTime)
                {
                    EnoughPress();
                }
            }
            else
            {
                ButtonReleased();
                timer = 0f;
            }
        }
    }

    void EnoughPress()
    {
        if (firstActivation)
        {
            mazeManager.PuzzleStart();
            firstActivation = false;
        }
        else
        {
            mazeManager.PuzzleReset();
        }
        
        timer = 0f;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isCleared)
        {
            if (collision.collider.CompareTag("Player") && IsPlayerUpward(collision))
            {
                isPressed = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!isCleared)
        {
            if (collision.collider.CompareTag("Player"))
            {
                isPressed = false;
            }
        }       
    }

    private bool IsPlayerUpward(Collision collision)
    {
        direction = collision.collider.transform.position - transform.position;
        heightDifference = Vector3.Dot(direction.normalized, Vector3.up);
        return heightDifference > 0.5f;
    }

    void ButtonPressed()
    {
        redButton.gameObject.SetActive(false);
        greenButton.gameObject.SetActive(true);
    }

    void ButtonReleased()
    {
        redButton.gameObject.SetActive(true);
        greenButton.gameObject.SetActive(false);
    }

    public void FixButton()
    {
        isCleared = true;
        ButtonPressed();
    }
}

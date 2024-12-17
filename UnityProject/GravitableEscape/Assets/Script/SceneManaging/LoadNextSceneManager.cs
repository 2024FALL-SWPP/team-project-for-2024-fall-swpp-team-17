using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextSceneManager : MonoBehaviour
{
    private GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            SceneManager.LoadScene(GetNextSceneIndex());
        }
    }

    private int GetNextSceneIndex()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        return currentSceneIndex + 1;
    }

    public void CurrentRestart()
    {
        gameManager.ResetLife();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndRestart()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 3 || currentSceneIndex == 4 || currentSceneIndex == 5)
        {
            SceneManager.LoadScene("Stage1-1");
        }
        else if (currentSceneIndex == 6 || currentSceneIndex == 7)
        {
            SceneManager.LoadScene("Stage2-1");
        }
        else
        {
            SceneManager.LoadScene("Tutorial");
        }
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene("Title");
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Load1()
    {
        SceneManager.LoadScene("Stage1-1");
    }
    public void Load2()
    {
        SceneManager.LoadScene("Stage2-1");
    }


}
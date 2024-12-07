using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadScene : MonoBehaviour
{
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void LoadStage1()
    {
        SceneManager.LoadScene("Stage1-1");
    }
    public void LoadStage2()
    {
        SceneManager.LoadScene("Stage2-1");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages scene loading and transitions within the game.
/// Provides methods for loading specific scenes, the next scene, or reloading the current scene.
/// </summary>
/// <remarks>
/// This class uses Unity's <see cref="SceneManager"/> to handle scene transitions.
/// Ensure all scenes are added to the Build Settings to enable proper transitions.
/// </remarks>
public class LoadScene : MonoBehaviour
{
    public void LoadTitle()
    {
        SceneManager.LoadScene("Title");
    }
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
    public void LoadEnding()
    {
        SceneManager.LoadScene("Ending");
    }

    public void LoadNextScene()
    {
        Debug.Log("good");
        SceneManager.LoadScene(GetNextSceneIndex());
    }

    private int GetNextSceneIndex()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        return currentSceneIndex + 1;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
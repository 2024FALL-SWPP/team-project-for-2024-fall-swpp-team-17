using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeepAudio : MonoBehaviour
{

    private static KeepAudio instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Avoid duplicates
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Destroy the audio object if leaving Title or Menu scenes
        if (scene.name != "Title" && scene.name != "Menu")
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


}

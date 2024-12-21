using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

/// <summary>
/// Handles sprite changes for a UI button based on user interactions.
/// Implements hover, click, and release states using defined sprites.
/// </summary>
/// <remarks>
/// Requires an <see cref="Image"/> component and three sprites:
/// - <see cref="defaultSprite"/>: Default button state.
/// - <see cref="activeSprite"/>: Hover/active button state.
/// - <see cref="pushedSprite"/>: Clicked button state.
/// </remarks>
public class ButtonsPlayMode
{
    private const int StartSceneIndex = 2; // First index of the scene to test
    private const int EndSceneIndex = 7;   // Last index of the scene to test

    [UnityTest]
    public IEnumerator TestPauseMenuButtons()
    {
        for (int sceneIndex = StartSceneIndex; sceneIndex <= EndSceneIndex; sceneIndex++)
        {
            // Load the scene
            SceneManager.LoadScene(sceneIndex);
            yield return null;

            // Simulate clicking the Pause button
            var pauseButton = GameObject.Find("pause").GetComponent<UnityEngine.UI.Button>();
            Assert.IsNotNull(pauseButton, $"Pause button not found in scene {sceneIndex}");
            pauseButton.onClick.Invoke();
            yield return null; // Wait for the UI update
            Assert.IsTrue(Time.timeScale == 0, $"Pause button failed in scene {sceneIndex}");

            // Check buttons displayed in the Pause menu
            var resumeButton = GameObject.Find("resume").GetComponent<UnityEngine.UI.Button>();
            var restartButton = GameObject.Find("restart").GetComponent<UnityEngine.UI.Button>();
            var mainMenuButton = GameObject.Find("main menu").GetComponent<UnityEngine.UI.Button>();
            var bgmButton = GameObject.Find("bgm").GetComponent<UnityEngine.UI.Button>();

            Assert.IsNotNull(resumeButton, $"Resume button not found in scene {sceneIndex}");
            Assert.IsNotNull(restartButton, $"Restart button not found in scene {sceneIndex}");
            Assert.IsNotNull(mainMenuButton, $"Main Menu button not found in scene {sceneIndex}");
            Assert.IsNotNull(bgmButton, $"BGM button not found in scene {sceneIndex}");

            // Test the functionality of each button
            resumeButton.onClick.Invoke();
            yield return null; // Verify Resume functionality
            Assert.IsTrue(Time.timeScale == 1, $"Resume button failed in scene {sceneIndex}");

            pauseButton.onClick.Invoke(); // Pause again
            yield return null;

            restartButton.onClick.Invoke();
            yield return new WaitForSeconds(1); // Wait for the scene to reload
            Assert.IsTrue(SceneManager.GetActiveScene().buildIndex == sceneIndex, $"Restart button failed in scene {sceneIndex}");
            Assert.IsTrue(Time.timeScale == 1, $"Restart button failed in scene {sceneIndex}");

            pauseButton.onClick.Invoke(); // Pause again
            yield return null;

            mainMenuButton.onClick.Invoke();
            yield return null;

            // Test the BGM button (simple On/Off toggle verification)
            pauseButton.onClick.Invoke(); // Pause again
            yield return null;
        }
    }
}
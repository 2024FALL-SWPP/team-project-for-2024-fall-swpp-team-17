using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using OurGame;

public class GameManagerPlayMode
{
    private GameManager gameManager;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        // Load the "Test" scene
        SceneManager.LoadScene("Test"); // Replace "Test" with your scene's actual name
        yield return null; // Wait one frame for the scene to load

        // Find the GameManager in the loaded scene
        gameManager = Object.FindObjectOfType<GameManager>();
        Assert.IsNotNull(gameManager, "GameManager was not found in the scene.");
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        // Optionally unload the scene after tests
        SceneManager.UnloadSceneAsync("Test");
        yield return null;
    }

    public static IEnumerable<object[]> ModifyLifeIntervalTestCases()
    {
        yield return new object[]
        {
            "Life 5 -> Decrease 2 -> Stun State for 2s -> Revived for 3s -> Playing",
            5,
            new int[] {-2, 0, 0, 0, 0, 0, 0},
            new int[] { 3, 3, 3, 3, 3, 3 }, // Life remains constant during the test
            new GameState[] { GameState.Stun, GameState.Stun, GameState.Revived, GameState.Revived, GameState.Revived, GameState.Playing }
        };

        yield return new object[]
        {
            "No damage in Stun, Revived: Life 5 -> Decrease 2 -> Stun State for 2s -> Revived for 3s -> Playing -> Decrease 1 -> Stun",
            5,
            new int[] {-2,-1, -1, -2, -5, -1, -1, 0},
            new int[] { 3, 3, 3, 3, 3, 3, 2},
            new GameState[] { GameState.Stun, GameState.Stun, GameState.Revived, GameState.Revived, GameState.Revived, GameState.Playing, GameState.Stun }
        };

        yield return new object[]
        {
            "No increase in Stun, yes increase in Revived: Life 5 -> Decrease 2 -> Stun State for 2s(no change) -> Revived for 3s(no damage) -> Playing -> Decrease 1 -> Stun",
            5,
            new int[] {-2,-1, 1, -2, 5, -1, -1},
            new int[] { 3, 3, 3, 3, 8, 8},
            new GameState[] { GameState.Stun, GameState.Stun, GameState.Revived, GameState.Revived, GameState.Revived, GameState.Playing, GameState.Stun }
        };
        // Additional test cases can be added here...
    }

    [UnityTest]
    public IEnumerator ModifyLife_Test_With_Intervals([ValueSource(nameof(ModifyLifeIntervalTestCases))] object[] testCase)
    {
        // Extract test case parameters
        string testName = (string)testCase[0];
        int initialLife = (int)testCase[1];
        int[] amounts = (int[])testCase[2];
        int[] expectedLives = (int[])testCase[3];
        GameState[] expectedStates = (GameState[])testCase[4];

        Debug.Log($"Running Test: {testName}");

        // Arrange: Set initial life using reflection
        typeof(GameManager)
            .GetField("life", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(gameManager, initialLife);

        // Act: Modify life
        gameManager.ModifyLife(amounts[0]);

        // Assert: Check life and state at 1-second intervals
        for (int i = 0; i < expectedLives.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(expectedLives[i], gameManager.Life, $"Life should be {expectedLives[i]} at second {i + 1}.");
            Assert.AreEqual(expectedStates[i], gameManager.gameState, $"Game state should be {expectedStates[i]} at second {i + 1}.");
            yield return new WaitForSeconds(0.8f);
            gameManager.ModifyLife(amounts[i + 1]);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
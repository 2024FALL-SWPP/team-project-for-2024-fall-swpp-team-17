using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using OurGame;
using UnityEngine.TestTools;

public class GameManagerEditMode
{
    private GameObject gameManagerObject;
    private GameManager gameManager;
    private GameObject dummy;
    private CameraManager cameraManager;
    private PlayerManager playerManager;
    private UIManager uIManager;

    [SetUp]
    public void Setup()
    {
        // Create and initialize GameManager
        gameManagerObject = new GameObject("GameManager");
        gameManager = gameManagerObject.AddComponent<GameManager>();
        dummy = new GameObject("Dummy");
        cameraManager = dummy.AddComponent<MockCameraManager>();
        playerManager = dummy.AddComponent<MockPlayerManager>();
        uIManager = dummy.AddComponent<MockUIManager>();

        // Initialize Subject and observers
        typeof(GameManager)
            .GetField("gameStateChange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(gameManager, new Subject<GameStateObserver, GameState>());
        gameManager.cameraManager = cameraManager;
        gameManager.playerManager = playerManager;
        gameManager.uIManager = uIManager;

        // Manually add observers
        var gameStateChange = typeof(GameManager)
            .GetField("gameStateChange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.GetValue(gameManager) as Subject<GameStateObserver, GameState>;

        gameStateChange?.AddObserver(cameraManager);
        gameStateChange?.AddObserver(playerManager);
        gameStateChange?.AddObserver(uIManager);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(gameManagerObject);
        Object.DestroyImmediate(dummy);
    }

    [TestCase(5, -2, 3, GameState.Stun, TestName = "Life 5 -> Decrease 2 -> Stun State")]
    [TestCase(5, -5, 0, GameState.Gameover, TestName = "Life 5 -> Decrease 5 -> Gameover State")]
    [TestCase(5, -8, 0, GameState.Gameover, TestName = "Life 5 -> Decrease 8 -> Gameover State")]
    [TestCase(3, 3, 6, GameState.Playing, TestName = "Life 3 -> Increase 3 -> Playing State")]
    public void ModifyLife_Test_Single(int initialLife, int amount, int expectedLife, GameState expectedState)
    {
        typeof(GameManager)
            .GetField("life", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(gameManager, initialLife);
        gameManager.ModifyLife(0); // Ensure initial setup
        Assert.AreEqual(initialLife, gameManager.Life, "Initial life is not correctly set.");

        // Act: Modify life
        gameManager.ModifyLife(amount);

        // Assert: Verify life and game state
        Assert.AreEqual(expectedLife, gameManager.Life, $"Life should be {expectedLife} after modifying by {amount}.");
        Assert.AreEqual(expectedState, gameManager.gameState, $"Game state should be {expectedState} after modification.");
    }

    public static IEnumerable<object[]> ModifyLifeTestCases()
    {
        yield return new object[] { "Life 5 -> Decrease 2 -> Stun State -> Increase 1 -> Stun State", 5, -2, 3, GameState.Stun, 1, 3, GameState.Stun };
        yield return new object[] { "Life 5 -> Decrease 5 -> Gameover State -> Increase 1 -> Gameover State", 5, -5, 0, GameState.Gameover, 1, 0, GameState.Gameover };
        yield return new object[] { "Life 3 -> Increase 3 -> Playing State -> Decrease 3 -> Stun State", 3, 3, 6, GameState.Playing, -3, 3, GameState.Stun };
    }
    [UnityTest]
    public IEnumerator ModifyLife_Test_Double_Imediate_Cases([ValueSource(nameof(ModifyLifeTestCases))] object[] testCase)
    {
        // Extract test case parameters
        string testName = (string)testCase[0];
        int initialLife = (int)testCase[1];
        int amount1 = (int)testCase[2];
        int expectedLife1 = (int)testCase[3];
        GameState expectedState1 = (GameState)testCase[4];
        int amount2 = (int)testCase[5];
        int expectedLife2 = (int)testCase[6];
        GameState expectedState2 = (GameState)testCase[7];

        // Arrange: Set initial life
        typeof(GameManager)
            .GetField("life", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(gameManager, initialLife);

        // Act: Modify life first time
        gameManager.ModifyLife(amount1);

        // No delay needed for immediate changes
        yield return null;

        // Assert: Verify life and game state after first modification
        Assert.AreEqual(expectedLife1, gameManager.Life, $"Life should be {expectedLife1} after modifying by {amount1}.");
        Assert.AreEqual(expectedState1, gameManager.gameState, $"Game state should be {expectedState1} after modification.");

        // Act: Modify life second time
        gameManager.ModifyLife(amount2);

        // No delay needed for immediate changes
        yield return null;

        // Assert: Verify life and game state after second modification
        Assert.AreEqual(expectedLife2, gameManager.Life, $"Life should be {expectedLife2} after modifying by {amount2}.");
        Assert.AreEqual(expectedState2, gameManager.gameState, $"Game state should be {expectedState2} after modification.");
    }
}

using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using OurGame;
using UnityEngine.TestTools;

public class GravityManagerPlayModeTests
{
    private GravityManager gravityManager;
    private PlayerManager playerManager;
    private CameraManager cameraManager;

    [SetUp]
    public void Setup()
    {
        // Load the "Tutorial" scene
        SceneManager.LoadScene("Tutorial");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Get required objects from the scene
        gravityManager = Object.FindObjectOfType<GravityManager>();
        playerManager = Object.FindObjectOfType<PlayerManager>();
        cameraManager = Object.FindObjectOfType<CameraManager>();

        // Ensure objects are not null
        Assert.IsNotNull(gravityManager, "GravityManager not found in the Test scene.");
        Assert.IsNotNull(playerManager, "PlayerManager not found in the Test scene.");
        Assert.IsNotNull(cameraManager, "CameraManager not found in the Test scene.");
    }

    [UnityTest]
    public IEnumerator RotateAngle_PlayerAndCamera_RotateCorrectly_Single()
    {
        // Test cases for different gravity rotations
        var testCases = new[]
        {
        new { Angle = -90, ExpectedRotation = Quaternion.Euler(0, 0, -90) },
        new { Angle = -180, ExpectedRotation = Quaternion.Euler(0, 0, -180) },
        new { Angle = -270, ExpectedRotation = Quaternion.Euler(0, 0, -270) }
    };

        foreach (var testCase in testCases)
        {
            // Reload the scene for each test case
            SceneManager.LoadScene("Tutorial");
            yield return new WaitForSeconds(1f); // Allow time for the scene to load

            // Fetch objects again after reloading the scene
            gravityManager = Object.FindObjectOfType<GravityManager>();
            playerManager = Object.FindObjectOfType<PlayerManager>();
            cameraManager = Object.FindObjectOfType<CameraManager>();

            // Ensure objects are not null
            Assert.IsNotNull(gravityManager, "GravityManager not found in the Test scene.");
            Assert.IsNotNull(playerManager, "PlayerManager not found in the Test scene.");
            Assert.IsNotNull(cameraManager, "CameraManager not found in the Test scene.");

            // Act: Rotate gravity using reflection
            InvokeRotateAngle(gravityManager, testCase.Angle);
            yield return new WaitForSeconds(1f); // Allow time for the rotation to propagate

            // Assert: Check player and camera rotations
            AssertQuaternionApproximatelyEqual(
                testCase.ExpectedRotation,
                playerManager.transform.rotation,
                0.1f,
                $"Player rotation did not match for angle {testCase.Angle}."
            );

            AssertQuaternionApproximatelyEqual(
                testCase.ExpectedRotation,
                cameraManager.transform.rotation,
                0.1f,
                $"Camera rotation did not match for angle {testCase.Angle}."
            );
        }
    }

    [UnityTest]
    public IEnumerator RotateAngle_PlayerAndCamera_RotateCorrectly_Multiple()
    {
        // Test cases for different gravity rotations
        var testCases = new[]
        {
            new { Angle = -90, ExpectedRotation = Quaternion.Euler(0, 0, -90) },
            new { Angle = -180, ExpectedRotation = Quaternion.Euler(0, 0, -270) },
            new { Angle = -270, ExpectedRotation = Quaternion.Euler(0, 0, -180) }
        };

        foreach (var testCase in testCases)
        {
            // Act: Rotate gravity using reflection
            InvokeRotateAngle(gravityManager, testCase.Angle);
            yield return new WaitForSeconds(1f); // Allow time for the rotation to propagate

            // Assert: Check player and camera rotations
            AssertQuaternionApproximatelyEqual(
                testCase.ExpectedRotation,
                playerManager.transform.rotation,
                0.1f,
                $"Player rotation did not match for angle {testCase.Angle}."
            );

            AssertQuaternionApproximatelyEqual(
                testCase.ExpectedRotation,
                cameraManager.transform.rotation,
                0.1f,
                $"Camera rotation did not match for angle {testCase.Angle}."
            );
        }
    }

    [UnityTest]
    public IEnumerator RotateAngle_PhysicsGravity_IsUpdated()
    {
        // Arrange: Set initial gravity
        Vector3 initialGravity = Physics.gravity;

        // Act: Rotate gravity using reflection
        InvokeRotateAngle(gravityManager, -90);
        yield return null;

        // Assert: Check Physics.gravity
        Assert.AreNotEqual(initialGravity, Physics.gravity, "Physics.gravity should change after rotating the gravity.");
    }

    private void InvokeRotateAngle(GravityManager manager, int angle)
    {
        var method = typeof(GravityManager).GetMethod("RotateAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(manager, new object[] { angle });
    }

    private void AssertQuaternionApproximatelyEqual(Quaternion expected, Quaternion actual, float tolerance, string message = "")
    {
        float angleDifference = Quaternion.Angle(expected, actual);
        Assert.LessOrEqual(angleDifference, tolerance, $"{message} Expected angle difference <= {tolerance}, but was {angleDifference}.");
    }
}
// using System.Collections;
// using System.Collections.Generic;
// using NUnit.Framework;
// using UnityEngine;
// using OurGame;

// public class SpikeManagerEditMode
// {
//     private GameObject spikeObject;
//     private GameObject playerObject;
//     private SpikeManager spikeManager;

//     [SetUp]
//     public void Setup()
//     {
//         // Setup SpikeManager object
//         spikeObject = new GameObject("Spike");
//         spikeManager = spikeObject.AddComponent<SpikeManager>();

//         // Setup Player object
//         playerObject = new GameObject("Player");
//         playerObject.tag = "Player";
//         playerObject.transform.position = Vector3.zero;
//         playerObject.transform.up = Vector3.up;
//     }

//     [TearDown]
//     public void Teardown()
//     {
//         Object.DestroyImmediate(spikeObject);
//         Object.DestroyImmediate(playerObject);
//     }

//     [Test]
//     public void Collision_OnPointyPart_IsCollisionUpward_True()
//     {
//         spikeObject.transform.position = Vector3.zero;
//         spikeObject.transform.up = Vector3.up;

//         // Arrange: Collision on the pointy part of the spike
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = Vector3.zero,
//                 normal = -Vector3.up // Upward collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         // Act: Invoke IsCollisionUpward
//         bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

//         // Assert
//         Assert.IsTrue(result, "Collision on the pointy part of the spike should be considered as upward.");
//     }

//     [Test]
//     public void Collision_OnFlatPart_IsCollisionUpward_False()
//     {
//         spikeObject.transform.position = Vector3.zero;
//         spikeObject.transform.up = Vector3.up;
//         // Arrange: Collision on the flat part of the spike
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = Vector3.zero,
//                 normal = Vector3.right // Horizontal collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         // Act: Invoke IsCollisionUpward
//         bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

//         // Assert
//         Assert.IsFalse(result, "Collision on the flat part of the spike should not be considered as upward.");
//     }

//     [Test]
//     public void Collision_OnOppositeDirection_IsCollisionUpward_False()
//     {
//         spikeObject.transform.position = Vector3.zero;
//         spikeObject.transform.up = Vector3.up;
//         // Arrange: Collision with an angle that is not upward
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = Vector3.zero,
//                 normal = Vector3.up // Non-upward collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         // Act: Invoke IsCollisionUpward
//         bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

//         // Assert
//         Assert.IsFalse(result, "Collision with an incorrect angle should not be considered as upward.");
//     }

//     [Test]
//     public void MultipleContacts_OneUpward_IsCollisionUpward_True()
//     {
//         spikeObject.transform.position = Vector3.zero;
//         spikeObject.transform.up = Vector3.up;
//         // Arrange: Multiple contacts with at least one upward
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = Vector3.zero,
//                 normal = Vector3.right
//             },
//             new MyContactPoint
//             {
//                 point = Vector3.zero,
//                 normal = -Vector3.up // One upward contact
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         // Act: Invoke IsCollisionUpward
//         bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

//         // Assert
//         Assert.IsTrue(result, "Collision with multiple contacts including one upward should be considered as upward.");
//     }

//     [Test]
//     public void Wall1_Collision_OnPointyPart_IsCollisionUpward_True()
//     {
//         spikeObject.transform.position = Vector3.zero;
//         spikeObject.transform.up = Vector3.right;

//         // Arrange: Collision on the pointy part of the spike
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = Vector3.zero,
//                 normal = -Vector3.right // Upward collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         // Act: Invoke IsCollisionUpward
//         bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

//         // Assert
//         Assert.IsTrue(result, "Collision on the pointy part of the spike should be considered as upward.");
//     }

//     [Test]
//     public void Wall1_Collision_OnFlatPart_IsCollisionUpward_False()
//     {
//         spikeObject.transform.position = Vector3.zero;
//         spikeObject.transform.up = Vector3.right;
//         // Arrange: Collision on the flat part of the spike
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = Vector3.zero,
//                 normal = Vector3.up // Horizontal collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         // Act: Invoke IsCollisionUpward
//         bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

//         // Assert
//         Assert.IsFalse(result, "Collision on the flat part of the spike should not be considered as upward.");
//     }

//     [Test]
//     public void Wall1_Collision_OnOppositeDirection_IsCollisionUpward_False()
//     {
//         spikeObject.transform.position = Vector3.zero;
//         spikeObject.transform.up = Vector3.right;
//         // Arrange: Collision with an angle that is not upward
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = Vector3.zero,
//                 normal = Vector3.right // Non-upward collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         // Act: Invoke IsCollisionUpward
//         bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

//         // Assert
//         Assert.IsFalse(result, "Collision with an incorrect angle should not be considered as upward.");
//     }

//     [Test]
//     public void Wall2_Collision_OnPointyPart_IsCollisionUpward_True()
//     {
//         spikeObject.transform.position = Vector3.zero;
//         spikeObject.transform.up = -Vector3.up;

//         // Arrange: Collision on the pointy part of the spike
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = Vector3.zero,
//                 normal = Vector3.up // Upward collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         // Act: Invoke IsCollisionUpward
//         bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

//         // Assert
//         Assert.IsTrue(result, "Collision on the pointy part of the spike should be considered as upward.");
//     }

//     [Test]
//     public void Wall2_Collision_OnFlatPart_IsCollisionUpward_False()
//     {
//         spikeObject.transform.position = Vector3.zero;
//         spikeObject.transform.up = -Vector3.up;
//         // Arrange: Collision on the flat part of the spike
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = Vector3.zero,
//                 normal = Vector3.right // Horizontal collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         // Act: Invoke IsCollisionUpward
//         bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

//         // Assert
//         Assert.IsFalse(result, "Collision on the flat part of the spike should not be considered as upward.");
//     }

//     [Test]
//     public void Wall2_Collision_OnOppositeDirection_IsCollisionUpward_False()
//     {
//         spikeObject.transform.position = Vector3.zero;
//         spikeObject.transform.up = -Vector3.up;
//         // Arrange: Collision with an angle that is not upward
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = Vector3.zero,
//                 normal = -Vector3.up // Non-upward collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         // Act: Invoke IsCollisionUpward
//         bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

//         // Assert
//         Assert.IsFalse(result, "Collision with an incorrect angle should not be considered as upward.");
//     }

//     [Test]
//     public void Wall3_Collision_OnPointyPart_IsCollisionUpward_True()
//     {
//         spikeObject.transform.position = Vector3.zero;
//         spikeObject.transform.up = -Vector3.right;

//         // Arrange: Collision on the pointy part of the spike
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = Vector3.zero,
//                 normal = Vector3.right // Upward collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         // Act: Invoke IsCollisionUpward
//         bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

//         // Assert
//         Assert.IsTrue(result, "Collision on the pointy part of the spike should be considered as upward.");
//     }

//     [Test]
//     public void Wall3_Collision_OnFlatPart_IsCollisionUpward_False()
//     {
//         spikeObject.transform.position = Vector3.zero;
//         spikeObject.transform.up = -Vector3.right;
//         // Arrange: Collision on the flat part of the spike
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = Vector3.zero,
//                 normal = Vector3.up // Horizontal collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         // Act: Invoke IsCollisionUpward
//         bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

//         // Assert
//         Assert.IsFalse(result, "Collision on the flat part of the spike should not be considered as upward.");
//     }

//     [Test]
//     public void Wall3_Collision_OnOppositeDirection_IsCollisionUpward_False()
//     {
//         spikeObject.transform.position = Vector3.zero;
//         spikeObject.transform.up = -Vector3.right;
//         // Arrange: Collision with an angle that is not upward
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = Vector3.zero,
//                 normal = -Vector3.right // Non-upward collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         // Act: Invoke IsCollisionUpward
//         bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

//         // Assert
//         Assert.IsFalse(result, "Collision with an incorrect angle should not be considered as upward.");
//     }
//     private bool InvokeIsCollisionUpward(SpikeManager manager, MockCollision collision)
//     {
//         var method = typeof(SpikeManager).GetMethod("IsCollisionUpward", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
//         return (bool)method.Invoke(manager, new object[] { collision });
//     }
// }

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using OurGame;

public class SpikeManagerEditMode
{
    private GameObject spikeObject;
    private GameObject playerObject;
    private SpikeManager spikeManager;

    [SetUp]
    public void Setup()
    {
        spikeObject = new GameObject("Spike");
        spikeManager = spikeObject.AddComponent<SpikeManager>();

        playerObject = new GameObject("Player");
        playerObject.tag = "Player";
        playerObject.transform.position = Vector3.zero;
        playerObject.transform.up = Vector3.up;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(spikeObject);
        Object.DestroyImmediate(playerObject);
    }

    private static IEnumerable<TestCaseData> UpwardSpikeTestCases()
    {
        yield return new TestCaseData(
            new MyContactPoint { point = Vector3.zero, normal = -Vector3.up },
            true,
            "Upward Collision: Collision on pointy part of spike should be upward."
        ).SetName("UpwardSpike_PointyPart_True");

        yield return new TestCaseData(
            new MyContactPoint { point = Vector3.zero, normal = Vector3.right },
            false,
            "Upward Collision: Collision on flat part of spike should not be upward."
        ).SetName("UpwardSpike_FlatPart_False");

        yield return new TestCaseData(
            new MyContactPoint { point = Vector3.zero, normal = Vector3.up },
            false,
            "Upward Collision: Collision in opposite direction should not be upward."
        ).SetName("UpwardSpike_OppositeDirection_False");
    }

    private static IEnumerable<TestCaseData> RightwardSpikeTestCases()
    {
        yield return new TestCaseData(
            new MyContactPoint { point = Vector3.zero, normal = -Vector3.right },
            true,
            "Rightward Collision: Collision on pointy part of spike should be upward."
        ).SetName("RightwardSpike_PointyPart_True");

        yield return new TestCaseData(
            new MyContactPoint { point = Vector3.zero, normal = Vector3.up },
            false,
            "Rightward Collision: Collision on flat part of spike should not be upward."
        ).SetName("RightwardSpike_FlatPart_False");

        yield return new TestCaseData(
            new MyContactPoint { point = Vector3.zero, normal = Vector3.right },
            false,
            "Rightward Collision: Collision in opposite direction should not be upward."
        ).SetName("RightwardSpike_OppositeDirection_False");
    }

    private static IEnumerable<TestCaseData> DownwardSpikeTestCases()
    {
        yield return new TestCaseData(
            new MyContactPoint { point = Vector3.zero, normal = Vector3.up },
            true,
            "Downward Collision: Collision on pointy part of spike should be upward."
        ).SetName("DownwardSpike_PointyPart_True");

        yield return new TestCaseData(
            new MyContactPoint { point = Vector3.zero, normal = Vector3.right },
            false,
            "Downward Collision: Collision on flat part of spike should not be upward."
        ).SetName("DownwardSpike_FlatPart_False");

        yield return new TestCaseData(
            new MyContactPoint { point = Vector3.zero, normal = -Vector3.up },
            false,
            "Downward Collision: Collision in opposite direction should not be upward."
        ).SetName("DownwardSpike_OppositeDirection_False");
    }

    private static IEnumerable<TestCaseData> LeftwardSpikeTestCases()
    {
        yield return new TestCaseData(
            new MyContactPoint { point = Vector3.zero, normal = Vector3.right },
            true,
            "Leftward Collision: Collision on pointy part of spike should be upward."
        ).SetName("LeftwardSpike_PointyPart_True");

        yield return new TestCaseData(
            new MyContactPoint { point = Vector3.zero, normal = Vector3.up },
            false,
            "Leftward Collision: Collision on flat part of spike should not be upward."
        ).SetName("LeftwardSpike_FlatPart_False");

        yield return new TestCaseData(
            new MyContactPoint { point = Vector3.zero, normal = -Vector3.right },
            false,
            "Leftward Collision: Collision in opposite direction should not be upward."
        ).SetName("LeftwardSpike_OppositeDirection_False");
    }

    [TestCaseSource(nameof(UpwardSpikeTestCases))]
    public void UpwardSpikeTests(MyContactPoint contactPoint, bool expectedResult, string description)
    {
        RunCollisionTest(Vector3.up, contactPoint, expectedResult, description);
    }

    [TestCaseSource(nameof(RightwardSpikeTestCases))]
    public void RightwardSpikeTests(MyContactPoint contactPoint, bool expectedResult, string description)
    {
        RunCollisionTest(Vector3.right, contactPoint, expectedResult, description);
    }

    [TestCaseSource(nameof(DownwardSpikeTestCases))]
    public void DownwardSpikeTests(MyContactPoint contactPoint, bool expectedResult, string description)
    {
        RunCollisionTest(-Vector3.up, contactPoint, expectedResult, description);
    }

    [TestCaseSource(nameof(LeftwardSpikeTestCases))]
    public void LeftwardSpikeTests(MyContactPoint contactPoint, bool expectedResult, string description)
    {
        RunCollisionTest(-Vector3.right, contactPoint, expectedResult, description);
    }

    private void RunCollisionTest(Vector3 spikeDirection, MyContactPoint contactPoint, bool expectedResult, string description)
    {
        // Arrange
        spikeObject.transform.position = Vector3.zero;
        spikeObject.transform.up = spikeDirection;

        var mockContacts = new List<MyContactPoint> { contactPoint };
        var collisionMock = new MockCollision(playerObject, mockContacts);

        // Act
        bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

        // Assert
        Assert.AreEqual(expectedResult, result, description);
    }

    private bool InvokeIsCollisionUpward(SpikeManager manager, MockCollision collision)
    {
        var method = typeof(SpikeManager).GetMethod("IsCollisionUpward", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (bool)method.Invoke(manager, new object[] { collision });
    }
}
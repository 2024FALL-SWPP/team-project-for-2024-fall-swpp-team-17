// using System.Collections;
// using System.Collections.Generic;
// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.TestTools;
// using OurGame;

// public class HeavyObjectEditMode
// {
//     private GameObject heavyObject;
//     private GameObject playerObject;
//     private HeavyObjectManager heavyObjectManager;
//     private PlayerManager playerManager;

//     [SetUp]
//     public void Setup()
//     {
//         heavyObject = new GameObject("HeavyObject");
//         heavyObjectManager = heavyObject.AddComponent<HeavyObjectManager>();

//         playerObject = new GameObject("Player");
//         playerManager = playerObject.AddComponent<PlayerManager>();
//         playerObject.tag = "Player";
//         playerObject.transform.position = Vector3.zero;
//         playerObject.transform.up = Vector3.up;
//     }

//     [TearDown]
//     public void Teardown()
//     {
//         Object.DestroyImmediate(heavyObject);
//         Object.DestroyImmediate(playerObject);
//     }

//     [Test]
//     public void Collision_HeadVertical_IsCrushed_True()
//     {
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = playerObject.transform.position + Vector3.up, // Head collision
//                 normal = -Vector3.up // Vertical collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
//         Assert.IsTrue(result, "Collision at player's head with vertical direction should be considered as crushed.");
//     }

//     [Test]
//     public void Collision_HeadHorizontal_IsCrushed_False()
//     {
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = playerObject.transform.position + Vector3.up, // Head collision
//                 normal = Vector3.right // Vertical collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
//         Assert.IsFalse(result, "Collision at player's head with horizontal direction should not be considered as crushed.");
//     }

//     [Test]
//     public void Collision_FootVertical_IsCrushed_False()
//     {
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = playerObject.transform.position - Vector3.up, // Foot collision
//                 normal = -Vector3.up // Vertical collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
//         Assert.IsFalse(result, "Collision at player's foot with vertical direction should not be considered as crushed.");
//     }

//     [Test]
//     public void Collision_FootHorizontal_IsCrushed_False()
//     {
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = playerObject.transform.position - Vector3.up, // Foot collision
//                 normal = -Vector3.right // Horizontal collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
//         Assert.IsFalse(result, "Collision at player's foot with horizontal direction should not be considered as crushed.");
//     }


//     [Test]
//     public void Gravity1_Collision_HeadVertical_IsCrushed_True()
//     {
//         // Change gravity as if key 1 is pressed
//         playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -90));
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = playerObject.transform.position + Vector3.right, // Head collision
//                 normal = Vector3.right // Vertical collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
//         Assert.IsTrue(result, "Gravity Change1 - Collision at player's head with verical direction should be considered as crushed.");
//     }

//     [Test]
//     public void Gravity1_Collision_HeadHorizontal_IsCrushed_False()
//     {
//         // Change gravity as if key 1 is pressed
//         playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -90));
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = playerObject.transform.position + Vector3.right, // Head collision
//                 normal = Vector3.up // Horizontal collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
//         Assert.IsFalse(result, "Gravity Change1 - Collision at player's head with horizontal direction should not be considered as crushed.");
//     }

//     [Test]
//     public void Gravity1_Collision_FootVertical_IsCrushed_False()
//     {
//         // Change gravity as if key 1 is pressed
//         playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -90));
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = playerObject.transform.position - Vector3.right, // Foot collision
//                 normal = Vector3.right // Vertical collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
//         Assert.IsFalse(result, "Gravity Change1 - Collision at player's foot with vertical direction should not be considered as crushed.");
//     }

//     [Test]
//     public void Gravity1_Collision_FootHorizontal_IsCrushed_False()
//     {
//         // Change gravity as if key 1 is pressed
//         playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -90));
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = playerObject.transform.position - Vector3.right, // Foot collision
//                 normal = Vector3.up // Horizontal collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
//         Assert.IsFalse(result, "Gravity Change1 - Collision at player's foot with horizontal direction should not be considered as crushed.");
//     }

//     [Test]
//     public void Gravity2_Collision_HeadVertical_IsCrushed_True()
//     {
//         // Change gravity as if key 2 is pressed
//         playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -180));
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = playerObject.transform.position - Vector3.up, // Head collision
//                 normal = Vector3.up // Vertical collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
//         Assert.IsTrue(result, "Gravity Change2 - Collision at player's head with vertical direction should be considered as crushed.");
//     }

//     [Test]
//     public void Gravity2_Collision_HeadHorizontal_IsCrushed_False()
//     {
//         // Change gravity as if key 2 is pressed
//         playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -180));
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = playerObject.transform.position - Vector3.up, // Head collision
//                 normal = -Vector3.right // Horizontal collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
//         Assert.IsFalse(result, "Gravity Change2 - Collision at player's head with horizontal direction should not be considered as crushed.");
//     }

//     [Test]
//     public void Gravity2_Collision_FootVertical_IsCrushed_False()
//     {
//         // Change gravity as if key 2 is pressed
//         playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -180));
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = playerObject.transform.position + Vector3.up, // Foot collision
//                 normal = Vector3.up // Vertical collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
//         Assert.IsFalse(result, "Gravity Change2 - Collision at player's foot with vertical direction should not be considered as crushed.");
//     }

//     [Test]
//     public void Gravity2_Collision_FootHorizontal_IsCrushed_False()
//     {
//         // Change gravity as if key 2 is pressed
//         playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -180));
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = playerObject.transform.position + Vector3.up, // Foot collision
//                 normal = Vector3.right // Horizontal collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
//         Assert.IsFalse(result, "Gravity Change2 - Collision at player's foot with horizontal direction should not be considered as crushed.");
//     }

//     [Test]
//     public void Gravity3_Collision_HeadVertical_IsCrushed_True()
//     {
//         // Change gravity as if key 3 is pressed
//         playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -270));
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = playerObject.transform.position - Vector3.right, // Head collision
//                 normal = -Vector3.right // Vertical collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
//         Assert.IsTrue(result, "Gravity Change3 - Collision at player's head with verical direction should be considered as crushed.");
//     }

//     [Test]
//     public void Gravity3_Collision_HeadHorizontal_IsCrushed_False()
//     {
//         // Change gravity as if key 3 is pressed
//         playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -270));
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = playerObject.transform.position - Vector3.right, // Head collision
//                 normal = -Vector3.up // Horizontal collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
//         Assert.IsFalse(result, "Gravity Change3 - Collision at player's head with horizontal direction should not be considered as crushed.");
//     }

//     [Test]
//     public void Gravity3_Collision_FootVertical_IsCrushed_False()
//     {
//         // Change gravity as if key 3 is pressed
//         playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -270));
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = playerObject.transform.position + Vector3.right, // Foot collision
//                 normal = -Vector3.right // Vertical collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
//         Assert.IsFalse(result, "Gravity Change3 - Collision at player's foot with vertical direction should not be considered as crushed.");
//     }

//     [Test]
//     public void Gravity3_Collision_FootHorizontal_IsCrushed_False()
//     {
//         // Change gravity as if key 3 is pressed
//         playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -270));
//         var mockContacts = new List<MyContactPoint>
//         {
//             new MyContactPoint
//             {
//                 point = playerObject.transform.position + Vector3.right, // Foot collision
//                 normal = -Vector3.up // Horizontal collision
//             }
//         };

//         var collisionMock = new MockCollision(playerObject, mockContacts);

//         bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
//         Assert.IsFalse(result, "Gravity Change3 - Collision at player's foot with horizontal direction should not be considered as crushed.");
//     }


//     private bool InvokeIsCrushed(HeavyObjectManager manager, MockCollision collision)
//     {
//         var method = typeof(HeavyObjectManager).GetMethod("IsCrushed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
//         return (bool)method.Invoke(manager, new object[] { collision });
//     }
// }

using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using OurGame;

public class HeavyObjectEditMode
{
    private GameObject heavyObject;
    private GameObject playerObject;
    private HeavyObjectManager heavyObjectManager;
    private PlayerManager playerManager;

    [SetUp]
    public void Setup()
    {
        heavyObject = new GameObject("HeavyObject");
        heavyObjectManager = heavyObject.AddComponent<HeavyObjectManager>();

        playerObject = new GameObject("Player");
        playerManager = playerObject.AddComponent<PlayerManager>();
        playerObject.tag = "Player";
        playerObject.transform.position = Vector3.zero;
        playerObject.transform.up = Vector3.up;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(heavyObject);
        Object.DestroyImmediate(playerObject);
    }

    private static IEnumerable<TestCaseData> CollisionTestCases()
    {
        yield return new TestCaseData(
            new List<MyContactPoint>
            {
                new MyContactPoint
                {
                    point = Vector3.up,
                    normal = -Vector3.up
                }
            },
            true,
            "Head vertical collision should be crushed."
        ).SetName("Collision_HeadVertical_IsCrushed_True");

        yield return new TestCaseData(
            new List<MyContactPoint>
            {
                new MyContactPoint
                {
                    point = Vector3.up,
                    normal = Vector3.right
                }
            },
            false,
            "Head horizontal collision should not be crushed."
        ).SetName("Collision_HeadHorizontal_IsCrushed_False");

        yield return new TestCaseData(
            new List<MyContactPoint>
            {
                new MyContactPoint
                {
                    point = -Vector3.up,
                    normal = -Vector3.up
                }
            },
            false,
            "Foot vertical collision should not be crushed."
        ).SetName("Collision_FootVertical_IsCrushed_False");

        yield return new TestCaseData(
            new List<MyContactPoint>
            {
                new MyContactPoint
                {
                    point = -Vector3.up,
                    normal = Vector3.right
                }
            },
            false,
            "Foot horizontal collision should not be crushed."
        ).SetName("Collision_FootHorizontal_IsCrushed_False");
    }

    private static IEnumerable<TestCaseData> GravityCollisionTestCases()
    {
        // Gravity -90
        yield return new TestCaseData(
            Quaternion.Euler(0, 0, -90),
            new List<MyContactPoint>
            {
            new MyContactPoint
            {
                point = Vector3.right,
                normal = Vector3.right
            }
            },
            true,
            "Gravity -90: Head vertical collision should be crushed."
        ).SetName("Gravity1_Collision_HeadVertical_IsCrushed_True");

        yield return new TestCaseData(
            Quaternion.Euler(0, 0, -90),
            new List<MyContactPoint>
            {
            new MyContactPoint
            {
                point = Vector3.right,
                normal = Vector3.up
            }
            },
            false,
            "Gravity -90: Head horizontal collision should not be crushed."
        ).SetName("Gravity1_Collision_HeadHorizontal_IsCrushed_False");

        yield return new TestCaseData(
            Quaternion.Euler(0, 0, -90),
            new List<MyContactPoint>
            {
            new MyContactPoint
            {
                point = -Vector3.right,
                normal = Vector3.right
            }
            },
            false,
            "Gravity -90: Foot vertical collision should not be crushed."
        ).SetName("Gravity1_Collision_FootVertical_IsCrushed_False");

        yield return new TestCaseData(
            Quaternion.Euler(0, 0, -90),
            new List<MyContactPoint>
            {
            new MyContactPoint
            {
                point = -Vector3.right,
                normal = Vector3.up
            }
            },
            false,
            "Gravity -90: Foot horizontal collision should not be crushed."
        ).SetName("Gravity1_Collision_FootHorizontal_IsCrushed_False");

        // Gravity -180
        yield return new TestCaseData(
            Quaternion.Euler(0, 0, -180),
            new List<MyContactPoint>
            {
            new MyContactPoint
            {
                point = -Vector3.up,
                normal = Vector3.up
            }
            },
            true,
            "Gravity -180: Head vertical collision should be crushed."
        ).SetName("Gravity2_Collision_HeadVertical_IsCrushed_True");

        yield return new TestCaseData(
            Quaternion.Euler(0, 0, -180),
            new List<MyContactPoint>
            {
            new MyContactPoint
            {
                point = -Vector3.up,
                normal = -Vector3.right
            }
            },
            false,
            "Gravity -180: Head horizontal collision should not be crushed."
        ).SetName("Gravity2_Collision_HeadHorizontal_IsCrushed_False");

        yield return new TestCaseData(
            Quaternion.Euler(0, 0, -180),
            new List<MyContactPoint>
            {
            new MyContactPoint
            {
                point = Vector3.up,
                normal = Vector3.up
            }
            },
            false,
            "Gravity -180: Foot vertical collision should not be crushed."
        ).SetName("Gravity2_Collision_FootVertical_IsCrushed_False");

        yield return new TestCaseData(
            Quaternion.Euler(0, 0, -180),
            new List<MyContactPoint>
            {
            new MyContactPoint
            {
                point = Vector3.up,
                normal = Vector3.right
            }
            },
            false,
            "Gravity -180: Foot horizontal collision should not be crushed."
        ).SetName("Gravity2_Collision_FootHorizontal_IsCrushed_False");

        // Gravity -270
        yield return new TestCaseData(
            Quaternion.Euler(0, 0, -270),
            new List<MyContactPoint>
            {
            new MyContactPoint
            {
                point = -Vector3.right,
                normal = -Vector3.right
            }
            },
            true,
            "Gravity -270: Head vertical collision should be crushed."
        ).SetName("Gravity3_Collision_HeadVertical_IsCrushed_True");

        yield return new TestCaseData(
            Quaternion.Euler(0, 0, -270),
            new List<MyContactPoint>
            {
            new MyContactPoint
            {
                point = -Vector3.right,
                normal = -Vector3.up
            }
            },
            false,
            "Gravity -270: Head horizontal collision should not be crushed."
        ).SetName("Gravity3_Collision_HeadHorizontal_IsCrushed_False");

        yield return new TestCaseData(
            Quaternion.Euler(0, 0, -270),
            new List<MyContactPoint>
            {
            new MyContactPoint
            {
                point = Vector3.right,
                normal = -Vector3.right
            }
            },
            false,
            "Gravity -270: Foot vertical collision should not be crushed."
        ).SetName("Gravity3_Collision_FootVertical_IsCrushed_False");

        yield return new TestCaseData(
            Quaternion.Euler(0, 0, -270),
            new List<MyContactPoint>
            {
            new MyContactPoint
            {
                point = Vector3.right,
                normal = -Vector3.up
            }
            },
            false,
            "Gravity -270: Foot horizontal collision should not be crushed."
        ).SetName("Gravity3_Collision_FootHorizontal_IsCrushed_False");
    }

    [Test, TestCaseSource(nameof(CollisionTestCases))]
    public void CollisionTests(List<MyContactPoint> mockContacts, bool expectedResult, string description)
    {
        var collisionMock = CreateMockCollision(mockContacts);
        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.AreEqual(expectedResult, result, description);
    }

    [Test, TestCaseSource(nameof(GravityCollisionTestCases))]
    public void GravityCollisionTests(Quaternion gravityRotation, List<MyContactPoint> mockContacts, bool expectedResult, string description)
    {
        playerManager.OnNotify<GravityObserver>(gravityRotation);

        var collisionMock = CreateMockCollision(mockContacts);
        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.AreEqual(expectedResult, result, description);
    }

    private MockCollision CreateMockCollision(List<MyContactPoint> mockContacts)
    {
        return new MockCollision(playerObject, mockContacts);
    }

    private bool InvokeIsCrushed(HeavyObjectManager manager, MockCollision collision)
    {
        var method = typeof(HeavyObjectManager).GetMethod("IsCrushed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (bool)method.Invoke(manager, new object[] { collision });
    }
}
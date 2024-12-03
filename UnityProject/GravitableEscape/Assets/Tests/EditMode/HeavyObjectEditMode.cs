using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
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
        // 테스트를 위한 GameObject 생성
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

    [Test]
    public void Collision_HeadVertical_IsCrushed_True()
    {
        var mockContacts = new List<MockContactPoint>
        {
            new MockContactPoint
            {
                point = playerObject.transform.position + Vector3.up, // Head collision
                normal = -Vector3.up // Vertical collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.IsTrue(result, "Collision at player's head with vertical direction should be considered as crushed.");
    }

    [Test]
    public void Collision_HeadHorizontal_IsCrushed_False()
    {
        var mockContacts = new List<MockContactPoint>
        {
            new MockContactPoint
            {
                point = playerObject.transform.position + Vector3.up, // Head collision
                normal = Vector3.right // Vertical collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.IsFalse(result, "Collision at player's head with horizontal direction should not be considered as crushed.");
    }

    [Test]
    public void Collision_FootVertical_IsCrushed_False()
    {
        var mockContacts = new List<MockContactPoint>
        {
            new MockContactPoint
            {
                point = playerObject.transform.position - Vector3.up, // Foot collision
                normal = -Vector3.up // Vertical collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.IsFalse(result, "Collision at player's foot with vertical direction should not be considered as crushed.");
    }

    [Test]
    public void Collision_FootHorizontal_IsCrushed_False()
    {
        var mockContacts = new List<MockContactPoint>
        {
            new MockContactPoint
            {
                point = playerObject.transform.position - Vector3.up, // Foot collision
                normal = -Vector3.right // Horizontal collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.IsFalse(result, "Collision at player's foot with horizontal direction should not be considered as crushed.");
    }


    [Test]
    public void Gravity1_Collision_HeadVertical_IsCrushed_True()
    {
        // Change gravity as if key 1 is pressed
        playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -90));
        var mockContacts = new List<MockContactPoint>
        {
            new MockContactPoint
            {
                point = playerObject.transform.position + Vector3.right, // Head collision
                normal = Vector3.right // Vertical collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.IsTrue(result, "Gravity Change1 - Collision at player's head with verical direction should be considered as crushed.");
    }

    [Test]
    public void Gravity1_Collision_HeadHorizontal_IsCrushed_False()
    {
        // Change gravity as if key 1 is pressed
        playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -90));
        var mockContacts = new List<MockContactPoint>
        {
            new MockContactPoint
            {
                point = playerObject.transform.position + Vector3.right, // Head collision
                normal = Vector3.up // Horizontal collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.IsFalse(result, "Gravity Change1 - Collision at player's head with horizontal direction should not be considered as crushed.");
    }

    [Test]
    public void Gravity1_Collision_FootVertical_IsCrushed_False()
    {
        // Change gravity as if key 1 is pressed
        playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -90));
        var mockContacts = new List<MockContactPoint>
        {
            new MockContactPoint
            {
                point = playerObject.transform.position - Vector3.right, // Foot collision
                normal = Vector3.right // Vertical collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.IsFalse(result, "Gravity Change1 - Collision at player's foot with vertical direction should not be considered as crushed.");
    }

    [Test]
    public void Gravity1_Collision_FootHorizontal_IsCrushed_False()
    {
        // Change gravity as if key 1 is pressed
        playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -90));
        var mockContacts = new List<MockContactPoint>
        {
            new MockContactPoint
            {
                point = playerObject.transform.position - Vector3.right, // Foot collision
                normal = Vector3.up // Horizontal collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.IsFalse(result, "Gravity Change1 - Collision at player's foot with horizontal direction should not be considered as crushed.");
    }

    [Test]
    public void Gravity2_Collision_HeadVertical_IsCrushed_True()
    {
        // Change gravity as if key 2 is pressed
        playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -180));
        var mockContacts = new List<MockContactPoint>
        {
            new MockContactPoint
            {
                point = playerObject.transform.position - Vector3.up, // Head collision
                normal = Vector3.up // Vertical collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.IsTrue(result, "Gravity Change2 - Collision at player's head with vertical direction should be considered as crushed.");
    }

    [Test]
    public void Gravity2_Collision_HeadHorizontal_IsCrushed_False()
    {
        // Change gravity as if key 2 is pressed
        playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -180));
        var mockContacts = new List<MockContactPoint>
        {
            new MockContactPoint
            {
                point = playerObject.transform.position - Vector3.up, // Head collision
                normal = -Vector3.right // Horizontal collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.IsFalse(result, "Gravity Change2 - Collision at player's head with horizontal direction should not be considered as crushed.");
    }

    [Test]
    public void Gravity2_Collision_FootVertical_IsCrushed_False()
    {
        // Change gravity as if key 2 is pressed
        playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -180));
        var mockContacts = new List<MockContactPoint>
        {
            new MockContactPoint
            {
                point = playerObject.transform.position + Vector3.up, // Foot collision
                normal = Vector3.up // Vertical collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.IsFalse(result, "Gravity Change2 - Collision at player's foot with vertical direction should not be considered as crushed.");
    }

    [Test]
    public void Gravity2_Collision_FootHorizontal_IsCrushed_False()
    {
        // Change gravity as if key 2 is pressed
        playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -180));
        var mockContacts = new List<MockContactPoint>
        {
            new MockContactPoint
            {
                point = playerObject.transform.position + Vector3.up, // Foot collision
                normal = Vector3.right // Horizontal collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.IsFalse(result, "Gravity Change2 - Collision at player's foot with horizontal direction should not be considered as crushed.");
    }

    [Test]
    public void Gravity3_Collision_HeadVertical_IsCrushed_True()
    {
        // Change gravity as if key 3 is pressed
        playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -270));
        var mockContacts = new List<MockContactPoint>
        {
            new MockContactPoint
            {
                point = playerObject.transform.position - Vector3.right, // Head collision
                normal = -Vector3.right // Vertical collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.IsTrue(result, "Gravity Change3 - Collision at player's head with verical direction should be considered as crushed.");
    }

    [Test]
    public void Gravity3_Collision_HeadHorizontal_IsCrushed_False()
    {
        // Change gravity as if key 3 is pressed
        playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -270));
        var mockContacts = new List<MockContactPoint>
        {
            new MockContactPoint
            {
                point = playerObject.transform.position - Vector3.right, // Head collision
                normal = -Vector3.up // Horizontal collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.IsFalse(result, "Gravity Change3 - Collision at player's head with horizontal direction should not be considered as crushed.");
    }

    [Test]
    public void Gravity3_Collision_FootVertical_IsCrushed_False()
    {
        // Change gravity as if key 3 is pressed
        playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -270));
        var mockContacts = new List<MockContactPoint>
        {
            new MockContactPoint
            {
                point = playerObject.transform.position + Vector3.right, // Foot collision
                normal = -Vector3.right // Vertical collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.IsFalse(result, "Gravity Change3 - Collision at player's foot with vertical direction should not be considered as crushed.");
    }

    [Test]
    public void Gravity3_Collision_FootHorizontal_IsCrushed_False()
    {
        // Change gravity as if key 3 is pressed
        playerManager.OnNotify<GravityObserver>(Quaternion.Euler(0, 0, -270));
        var mockContacts = new List<MockContactPoint>
        {
            new MockContactPoint
            {
                point = playerObject.transform.position + Vector3.right, // Foot collision
                normal = -Vector3.up // Horizontal collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
        Assert.IsFalse(result, "Gravity Change3 - Collision at player's foot with horizontal direction should not be considered as crushed.");
    }


    private bool InvokeIsCrushed(HeavyObjectManager manager, MockCollision collision)
    {
        var method = typeof(HeavyObjectManager).GetMethod("IsCrushed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (bool)method.Invoke(manager, new object[] { collision });
    }
}
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
        // Setup SpikeManager object
        spikeObject = new GameObject("Spike");
        spikeManager = spikeObject.AddComponent<SpikeManager>();

        // Setup Player object
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

    [Test]
    public void Collision_OnPointyPart_IsCollisionUpward_True()
    {
        spikeObject.transform.position = Vector3.zero;
        spikeObject.transform.up = Vector3.up;

        // Arrange: Collision on the pointy part of the spike
        var mockContacts = new List<MyContactPoint>
        {
            new MyContactPoint
            {
                point = Vector3.zero,
                normal = -Vector3.up // Upward collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        // Act: Invoke IsCollisionUpward
        bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

        // Assert
        Assert.IsTrue(result, "Collision on the pointy part of the spike should be considered as upward.");
    }

    [Test]
    public void Collision_OnFlatPart_IsCollisionUpward_False()
    {
        spikeObject.transform.position = Vector3.zero;
        spikeObject.transform.up = Vector3.up;
        // Arrange: Collision on the flat part of the spike
        var mockContacts = new List<MyContactPoint>
        {
            new MyContactPoint
            {
                point = Vector3.zero,
                normal = Vector3.right // Horizontal collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        // Act: Invoke IsCollisionUpward
        bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

        // Assert
        Assert.IsFalse(result, "Collision on the flat part of the spike should not be considered as upward.");
    }

    [Test]
    public void Collision_OnOppositeDirection_IsCollisionUpward_False()
    {
        spikeObject.transform.position = Vector3.zero;
        spikeObject.transform.up = Vector3.up;
        // Arrange: Collision with an angle that is not upward
        var mockContacts = new List<MyContactPoint>
        {
            new MyContactPoint
            {
                point = Vector3.zero,
                normal = Vector3.up // Non-upward collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        // Act: Invoke IsCollisionUpward
        bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

        // Assert
        Assert.IsFalse(result, "Collision with an incorrect angle should not be considered as upward.");
    }

    [Test]
    public void MultipleContacts_OneUpward_IsCollisionUpward_True()
    {
        spikeObject.transform.position = Vector3.zero;
        spikeObject.transform.up = Vector3.up;
        // Arrange: Multiple contacts with at least one upward
        var mockContacts = new List<MyContactPoint>
        {
            new MyContactPoint
            {
                point = Vector3.zero,
                normal = Vector3.right
            },
            new MyContactPoint
            {
                point = Vector3.zero,
                normal = -Vector3.up // One upward contact
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        // Act: Invoke IsCollisionUpward
        bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

        // Assert
        Assert.IsTrue(result, "Collision with multiple contacts including one upward should be considered as upward.");
    }

    [Test]
    public void Wall1_Collision_OnPointyPart_IsCollisionUpward_True()
    {
        spikeObject.transform.position = Vector3.zero;
        spikeObject.transform.up = Vector3.right;

        // Arrange: Collision on the pointy part of the spike
        var mockContacts = new List<MyContactPoint>
        {
            new MyContactPoint
            {
                point = Vector3.zero,
                normal = -Vector3.right // Upward collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        // Act: Invoke IsCollisionUpward
        bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

        // Assert
        Assert.IsTrue(result, "Collision on the pointy part of the spike should be considered as upward.");
    }

    [Test]
    public void Wall1_Collision_OnFlatPart_IsCollisionUpward_False()
    {
        spikeObject.transform.position = Vector3.zero;
        spikeObject.transform.up = Vector3.right;
        // Arrange: Collision on the flat part of the spike
        var mockContacts = new List<MyContactPoint>
        {
            new MyContactPoint
            {
                point = Vector3.zero,
                normal = Vector3.up // Horizontal collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        // Act: Invoke IsCollisionUpward
        bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

        // Assert
        Assert.IsFalse(result, "Collision on the flat part of the spike should not be considered as upward.");
    }

    [Test]
    public void Wall1_Collision_OnOppositeDirection_IsCollisionUpward_False()
    {
        spikeObject.transform.position = Vector3.zero;
        spikeObject.transform.up = Vector3.right;
        // Arrange: Collision with an angle that is not upward
        var mockContacts = new List<MyContactPoint>
        {
            new MyContactPoint
            {
                point = Vector3.zero,
                normal = Vector3.right // Non-upward collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        // Act: Invoke IsCollisionUpward
        bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

        // Assert
        Assert.IsFalse(result, "Collision with an incorrect angle should not be considered as upward.");
    }

    [Test]
    public void Wall2_Collision_OnPointyPart_IsCollisionUpward_True()
    {
        spikeObject.transform.position = Vector3.zero;
        spikeObject.transform.up = -Vector3.up;

        // Arrange: Collision on the pointy part of the spike
        var mockContacts = new List<MyContactPoint>
        {
            new MyContactPoint
            {
                point = Vector3.zero,
                normal = Vector3.up // Upward collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        // Act: Invoke IsCollisionUpward
        bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

        // Assert
        Assert.IsTrue(result, "Collision on the pointy part of the spike should be considered as upward.");
    }

    [Test]
    public void Wall2_Collision_OnFlatPart_IsCollisionUpward_False()
    {
        spikeObject.transform.position = Vector3.zero;
        spikeObject.transform.up = -Vector3.up;
        // Arrange: Collision on the flat part of the spike
        var mockContacts = new List<MyContactPoint>
        {
            new MyContactPoint
            {
                point = Vector3.zero,
                normal = Vector3.right // Horizontal collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        // Act: Invoke IsCollisionUpward
        bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

        // Assert
        Assert.IsFalse(result, "Collision on the flat part of the spike should not be considered as upward.");
    }

    [Test]
    public void Wall2_Collision_OnOppositeDirection_IsCollisionUpward_False()
    {
        spikeObject.transform.position = Vector3.zero;
        spikeObject.transform.up = -Vector3.up;
        // Arrange: Collision with an angle that is not upward
        var mockContacts = new List<MyContactPoint>
        {
            new MyContactPoint
            {
                point = Vector3.zero,
                normal = -Vector3.up // Non-upward collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        // Act: Invoke IsCollisionUpward
        bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

        // Assert
        Assert.IsFalse(result, "Collision with an incorrect angle should not be considered as upward.");
    }

    [Test]
    public void Wall3_Collision_OnPointyPart_IsCollisionUpward_True()
    {
        spikeObject.transform.position = Vector3.zero;
        spikeObject.transform.up = -Vector3.right;

        // Arrange: Collision on the pointy part of the spike
        var mockContacts = new List<MyContactPoint>
        {
            new MyContactPoint
            {
                point = Vector3.zero,
                normal = Vector3.right // Upward collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        // Act: Invoke IsCollisionUpward
        bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

        // Assert
        Assert.IsTrue(result, "Collision on the pointy part of the spike should be considered as upward.");
    }

    [Test]
    public void Wall3_Collision_OnFlatPart_IsCollisionUpward_False()
    {
        spikeObject.transform.position = Vector3.zero;
        spikeObject.transform.up = -Vector3.right;
        // Arrange: Collision on the flat part of the spike
        var mockContacts = new List<MyContactPoint>
        {
            new MyContactPoint
            {
                point = Vector3.zero,
                normal = Vector3.up // Horizontal collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        // Act: Invoke IsCollisionUpward
        bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

        // Assert
        Assert.IsFalse(result, "Collision on the flat part of the spike should not be considered as upward.");
    }

    [Test]
    public void Wall3_Collision_OnOppositeDirection_IsCollisionUpward_False()
    {
        spikeObject.transform.position = Vector3.zero;
        spikeObject.transform.up = -Vector3.right;
        // Arrange: Collision with an angle that is not upward
        var mockContacts = new List<MyContactPoint>
        {
            new MyContactPoint
            {
                point = Vector3.zero,
                normal = -Vector3.right // Non-upward collision
            }
        };

        var collisionMock = new MockCollision(playerObject, mockContacts);

        // Act: Invoke IsCollisionUpward
        bool result = InvokeIsCollisionUpward(spikeManager, collisionMock);

        // Assert
        Assert.IsFalse(result, "Collision with an incorrect angle should not be considered as upward.");
    }
    private bool InvokeIsCollisionUpward(SpikeManager manager, MockCollision collision)
    {
        var method = typeof(SpikeManager).GetMethod("IsCollisionUpward", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (bool)method.Invoke(manager, new object[] { collision });
    }
}
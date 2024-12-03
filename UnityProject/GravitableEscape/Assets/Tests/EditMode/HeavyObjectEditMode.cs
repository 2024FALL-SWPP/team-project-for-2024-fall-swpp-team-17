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

    [SetUp]
    public void Setup()
    {
        // 테스트를 위한 GameObject 생성
        heavyObject = new GameObject("HeavyObject");
        heavyObjectManager = heavyObject.AddComponent<HeavyObjectManager>();

        playerObject = new GameObject("Player");
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


    // [Test]
    // public void Collision_FootVertical_IsCrushed_False()
    // {
    //     // 충돌 지점: 플레이어 발, 방향: 수직
    //     MockContactPoint contact = new MockContactPoint
    //     {
    //         point = playerObject.transform.position - Vector3.up, // 발 위치
    //         normal = -Vector3.up // 수직 방향
    //     };

    //     MockContactPoint[] contacts = { contact };
    //     var collisionMock = new CollisionMock(playerObject, contacts);

    //     bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
    //     Assert.IsFalse(result, "Collision at player's foot with vertical direction should not be considered as crushed.");
    // }

    // [Test]
    // public void Collision_HeadHorizontal_IsCrushed_False()
    // {
    //     // 충돌 지점: 플레이어 머리, 방향: 가로
    //     ContactPoint contact = new ContactPoint
    //     {
    //         point = playerObject.transform.position + Vector3.up, // 머리 위치
    //         normal = Vector3.right // 가로 방향
    //     };

    //     ContactPoint[] contacts = { contact };
    //     var collisionMock = new CollisionMock(playerObject, contacts);

    //     bool result = InvokeIsCrushed(heavyObjectManager, collisionMock);
    //     Assert.IsFalse(result, "Collision at player's head with horizontal direction should not be considered as crushed.");
    // }

    private bool InvokeIsCrushed(HeavyObjectManager manager, MockCollision collision)
    {
        var method = typeof(HeavyObjectManager).GetMethod("IsCrushed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (bool)method.Invoke(manager, new object[] { collision });
    }
}
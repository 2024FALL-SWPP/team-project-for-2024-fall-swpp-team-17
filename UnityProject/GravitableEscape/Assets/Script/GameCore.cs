using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

// enum, Class, Interfaces used throughout the entire game is defined in this file
namespace OurGame
{
    /// <summary>
    /// The global state of the whole game
    /// GameManager holds the state and observers get notified when it is changed
    /// </summary>
    public enum GameState
    {
        Playing,
        WormholeEffect,
        Gameover,
        Stun, // Player fainted and can't move
        Revived // After fainting, player revived and can move, but can't get damage
    }

    /// <summary>
    /// The subject to notify observers (e.g. gravity change, gamestate change, etc)
    /// </summary>
    /// <typeparam name="Observer">type of Observers to notify the event</typeparam>
    /// <typeparam name="NotifyType">type of OnNotify's parameter</typeparam>
    public class Subject<Observer, NotifyType>
    {
        private List<Observer<NotifyType>> observers = new List<Observer<NotifyType>>();
        public void AddObserver(Observer<NotifyType> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }
        public void RemoveObserver(Observer<NotifyType> observer)
        {
            if (observers.Contains(observer))
            {
                observers.Remove(observer);
            }
        }
        public void NotifyObservers(NotifyType data)
        {
            foreach (Observer<NotifyType> observer in observers)
            {
                observer.OnNotify<Observer>(data);
            }
        }
    }

    /// <summary>
    /// Interface to be used when making the actual Observer interface
    /// </summary>
    /// <typeparam name="NotifyType">Type of data to hand to the observer classes</typeparam>
    public interface Observer<NotifyType>
    {
        void OnNotify<Observer>(NotifyType data);
    }

    public interface GravityObserver : Observer<Quaternion> { }

    public interface GameOverObserver : Observer<bool> { }
    public interface GameStateObserver : Observer<GameState> { }

    public interface PuzzleInterface
    {
        void PuzzleStart();
        void PuzzleReset();
        void PuzzleClear();
    }

    /// <summary>
    /// Interface of PlayerManager
    /// </summary>
    public interface ILifeManager
    {
        int Life { get; }
        public void ModifyLife(int amount);
    }

    /// <summary>
    /// Abstract class of hazards, which can harm the player(i.e. decrease its life)
    /// </summary>
    public abstract class HazardManager : MonoBehaviour
    {
        protected int damage;
        protected abstract void HarmPlayer(ILifeManager gameManager);
    }

    // Wrapper Classes for Testing
    public struct MockContactPoint
    {
        public Vector3 point;
        public Vector3 normal;

        public MockContactPoint(Vector3 point, Vector3 normal)
        {
            this.point = point;
            this.normal = normal;
        }
    }

    public interface IMyCollision
    {
        GameObject gameObject { get; }
        int GetContacts(MockContactPoint[] contactArray);
    }

    public class CollisionWrapper : IMyCollision
    {
        private readonly Collision collision;

        public CollisionWrapper(Collision collision)
        {
            this.collision = collision;
        }

        public GameObject gameObject => collision.gameObject;

        public int GetContacts(MockContactPoint[] contactArray)
        {
            int count = Mathf.Min(contactArray.Length, collision.contactCount);
            for (int i = 0; i < count; i++)
            {
                var contact = collision.GetContact(i);
                contactArray[i] = new MockContactPoint(contact.point, contact.normal);
            }
            return count;
        }
    }

    public class MockCollision : IMyCollision
    {
        private readonly GameObject mockGameObject;
        private readonly List<MockContactPoint> mockContacts;
        public MockCollision(GameObject mockGameObject, List<MockContactPoint> mockContacts)
        {
            this.mockGameObject = mockGameObject;
            this.mockContacts = mockContacts;
        }
        public GameObject gameObject => mockGameObject;
        public int GetContacts(MockContactPoint[] contactArray)
        {
            int count = Mathf.Min(contactArray.Length, mockContacts.Count);
            for (int i = 0; i < count; i++)
            {
                contactArray[i] = mockContacts[i];
            }
            return count;
        }
    }

    public class MockCameraManager : CameraManager, GameStateObserver, GravityObserver
    {
        new public void OnNotify<GravityObserver>(Quaternion rot) { }
        new public void OnNotify<GameStateObserver>(GameState gs) { }
    }
    public class MockPlayerManager : PlayerManager, GameStateObserver, GravityObserver
    {
        new public void OnNotify<GravityObserver>(Quaternion rot) { }
        new public void OnNotify<GameStateObserver>(GameState gs) { }
    }

    public class MockUIManager : UIManager, GameStateObserver
    {
        new public void OnNotify<GameStateObserver>(GameState gs) { }
    }


}
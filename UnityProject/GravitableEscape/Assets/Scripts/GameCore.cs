using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

// Namespace containing core definitions for the game
namespace OurGame
{
    /// <summary>
    /// Represents the global state of the game.
    /// The GameManager tracks the state, and observers are notified when it changes.
    /// </summary>
    public enum GameState
    {
        Playing,
        Paused,
        WormholeEffect,
        Gameover,
        Stun, // Player is incapacitated and cannot move
        Revived // Player is revived but temporarily invulnerable
    }

    /// <summary>
    /// A generic subject class to notify observers of specific events or data changes.
    /// </summary>
    /// <typeparam name="Observer">The type of observer to notify.</typeparam>
    /// <typeparam name="NotifyType">The type of data passed to observers.</typeparam>
    public class Subject<Observer, NotifyType>
    {
        private List<Observer<NotifyType>> observers = new List<Observer<NotifyType>>();

        /// <summary>
        /// Adds an observer to the notification list.
        /// </summary>
        public void AddObserver(Observer<NotifyType> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }

        /// <summary>
        /// Removes an observer from the notification list.
        /// </summary>
        public void RemoveObserver(Observer<NotifyType> observer)
        {
            if (observers.Contains(observer))
            {
                observers.Remove(observer);
            }
        }

        /// <summary>
        /// Notifies all observers of an event or data change.
        /// </summary>
        public void NotifyObservers(NotifyType data)
        {
            foreach (Observer<NotifyType> observer in observers)
            {
                observer.OnNotify<Observer>(data);
            }
        }
    }

    /// <summary>
    /// Base interface for creating observer interfaces.
    /// </summary>
    /// <typeparam name="NotifyType">Type of data passed to observer classes.</typeparam>
    public interface Observer<NotifyType>
    {
        void OnNotify<Observer>(NotifyType data);
    }

    // Specific observer interfaces for gravity, game state, and game over events
    public interface GravityObserver : Observer<Quaternion> { }
    public interface GameOverObserver : Observer<bool> { }
    public interface GameStateObserver : Observer<GameState> { }

    /// <summary>
    /// Interface for puzzles, defining the basic lifecycle methods.
    /// </summary>
    public interface PuzzleInterface
    {
        void PuzzleStart();
        void PuzzleReset();
        void PuzzleClear();
        void GetUnlockSignal(int lockID);
    }

    /// <summary>
    /// Interface for managing the player's life, including health modifications.
    /// </summary>
    public interface ILifeManager
    {
        int Life { get; }
        void ModifyLife(int amount);
    }

    /// <summary>
    /// Abstract base class for hazards that can harm the player.
    /// </summary>
    public abstract class HazardManager : MonoBehaviour
    {
        protected int damage; // Amount of damage dealt by the hazard
        protected abstract void HarmPlayer(ILifeManager gameManager);
    }

    // Wrapper classes for testing purposes

    /// <summary>
    /// Represents a contact point during a collision.
    /// </summary>
    public struct MyContactPoint
    {
        public Vector3 point; // Point of contact
        public Vector3 normal; // Normal vector at the contact point

        public MyContactPoint(Vector3 point, Vector3 normal)
        {
            this.point = point;
            this.normal = normal;
        }
    }

    /// <summary>
    /// Interface for accessing collision data.
    /// </summary>
    public interface IMyCollision
    {
        GameObject gameObject { get; } // The GameObject involved in the collision
        int GetContacts(MyContactPoint[] contactArray); // Retrieves contact points from the collision
    }

    /// <summary>
    /// Wraps Unity's Collision object for testing purposes.
    /// </summary>
    public class CollisionWrapper : IMyCollision
    {
        private readonly Collision collision;

        public CollisionWrapper(Collision collision)
        {
            this.collision = collision;
        }

        public GameObject gameObject => collision.gameObject;

        public int GetContacts(MyContactPoint[] contactArray)
        {
            int count = Mathf.Min(contactArray.Length, collision.contactCount);
            for (int i = 0; i < count; i++)
            {
                var contact = collision.GetContact(i);
                contactArray[i] = new MyContactPoint(contact.point, contact.normal);
            }
            return count;
        }
    }

    /// <summary>
    /// Mock implementation of IMyCollision for testing.
    /// </summary>
    public class MockCollision : IMyCollision
    {
        private readonly GameObject mockGameObject;
        private readonly List<MyContactPoint> mockContacts;

        public MockCollision(GameObject mockGameObject, List<MyContactPoint> mockContacts)
        {
            this.mockGameObject = mockGameObject;
            this.mockContacts = mockContacts;
        }

        public GameObject gameObject => mockGameObject;

        public int GetContacts(MyContactPoint[] contactArray)
        {
            int count = Mathf.Min(contactArray.Length, mockContacts.Count);
            for (int i = 0; i < count; i++)
            {
                contactArray[i] = mockContacts[i];
            }
            return count;
        }
    }

    // Mock classes for testing specific components

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
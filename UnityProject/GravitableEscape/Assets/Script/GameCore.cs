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
        stun,
        Fainted
    }

    /// <summary>
    /// The subject to notify observers (e.g. gravity change, gamestate change, etc)
    /// </summary>
    /// <typeparam name="Observer">type of Observers to notify the event</typeparam>
    /// <typeparam name="NotifyType">type of OnNotify's parameter</typeparam>
    class Subject<Observer, NotifyType>
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
    public interface IPlayerManager
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
        protected abstract void HarmPlayer(IPlayerManager player);
    }
}
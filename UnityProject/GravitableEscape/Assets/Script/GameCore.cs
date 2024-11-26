using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

// enum, Class, Interfaces used throughout the entire game is defined in this file

namespace OurGame
{
    public enum GameState
    {
        Playing,
        WormholeEffect,
        Fainted
    }
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
    public interface Observer<NotifyType>
    {
        void OnNotify<Observer>(NotifyType data);
    }

    public interface GravityObserver : Observer<Quaternion> { }

    public interface GameOverObserver : Observer<bool> { }
    public interface GameStateObserver : Observer<GameState> { }

    public interface IPlayerManager
    {
        int Life { get; }
        public void ModifyLife(int amount);
    }

    public abstract class HazardManager : MonoBehaviour
    {
        protected int damage;
        protected abstract void HarmPlayer(IPlayerManager player);
    }
}

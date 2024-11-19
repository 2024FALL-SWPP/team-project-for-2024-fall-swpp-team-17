using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

namespace OurGame
{
    public enum GameState
    {
        Playing,
        WormholeEffect
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
                observer.OnNotify(data);
            }
        }
    }
    public interface Observer<NotifyType>
    {
        void OnNotify(NotifyType data);
    }

    public interface GravityObserver : Observer<Quaternion> { }

    public interface GameOverObserver : Observer<bool> { }

    public class Player
    {
        private int life;
        public Player(int initialLife)
        {
            life = initialLife;
        }
        public void ModifyLife(int amount)
        {
            life += amount;
        }
    }

    public abstract class HazardManager : MonoBehaviour
    {
        protected int damage;
        protected abstract void HarmPlayer(Player player);
    }
}

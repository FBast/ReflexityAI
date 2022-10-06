using System;
using System.Collections.Generic;
using UnityEngine;

namespace Examples.TankArena.Scripts.SOEvents {
    public abstract class BaseGameEvent<T> : ScriptableObject {

        [TextArea] public string Description;
        public int NumberOfListeners => _listeners.Count;
        public float NumberOfRaise => _numberOfRaise;

        [NonSerialized] private float _numberOfRaise;
        
        private readonly List<IGameEventListener<T>> _listeners = new List<IGameEventListener<T>>();

        public void Raise(T item) {
            for (int i = _listeners.Count - 1; i >= 0; i--) {
                _listeners[i].OnEventRaised(item);
            }
            _numberOfRaise++;
        }

        public void RegisterListener(IGameEventListener<T> listener) {
            if (!_listeners.Contains(listener))
                _listeners.Add(listener);
        }

        public void UnregisterListener(IGameEventListener<T> listener) {
            if (_listeners.Contains(listener))
                _listeners.Remove(listener);
        }
    
    }
}
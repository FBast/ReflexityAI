using System;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.ReflexityAI.Framework {
    [Serializable] 
    public abstract class SerializableDictionary<T, U> : Dictionary<T, U>, ISerializationCallbackReceiver {
        [SerializeField] private List<T> keys = new List<T>();
        [SerializeField] private List<U> values = new List<U>();

        public void OnBeforeSerialize() {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<T, U> pair in this) {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize() {
            Clear();
            if (keys.Count != values.Count)
                throw new Exception("there are " + keys.Count + " keys and " + values.Count + " values after " +
                                    "deserialization. Make sure that both key and value types are serializable.");
            for (int i = 0; i < keys.Count; i++)
                Add(keys[i], values[i]);
        }
        
    }
    [Serializable] public class PortDictionary : SerializableDictionary<string,string>{}
    [Serializable] public class InfoDictionary : SerializableDictionary<string,SerializableInfo>{}
}
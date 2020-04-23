using System;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.xNodeUtilityAi.Framework {
    [Serializable] 
    public class SerializableInfoDictionary : Dictionary<string, SerializableInfo>, ISerializationCallbackReceiver {
        [SerializeField] private List<string> keys = new List<string>();
        [SerializeField] private List<SerializableInfo> values = new List<SerializableInfo>();

        public void OnBeforeSerialize() {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<string, SerializableInfo> pair in this) {
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
}
using System;
using System.Collections.Generic;

namespace Examples.TankArena.Scripts.SOReferences.DictionaryStringObjectReference {
    [Serializable]
    public class DictionaryStringObjectReference : Reference<Dictionary<string, object>, DictionaryStringObjectVariable> {
        public object GetParam(string paramKey) {
            if (Value == null) return null;
            return Value.ContainsKey(paramKey) ? Value[paramKey] : null;
        }

        public void SetParam(Tuple<string, object> tuple) {
            SetParam(tuple.Item1, tuple.Item2);
        }
		
        public void SetParam(string paramKey, object paramValue) {
            if (Value == null)
                Value = new Dictionary<string, object>();
            RemoveParam(paramKey);
            Value.Add(paramKey, paramValue);
        }

        public void RemoveParam(string paramKey) {
            if (Value.ContainsKey(paramKey))
                Value.Remove(paramKey);
        }
    }
}
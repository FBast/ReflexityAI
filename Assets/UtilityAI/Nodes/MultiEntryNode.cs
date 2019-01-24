using System;
using System.Collections.Generic;
using System.Linq;
using XNode;

namespace UtilityAI.Nodes {
    public abstract class MultiEntryNode<T, U> : EntryNode<T> {
        
        [Output(ShowBackingValue.Always, ConnectionType.Multiple, true)] public List<U> MultiValue;

        protected readonly List<float> _multiEntryValue = new List<float>();
        
        public override object GetValue(NodePort port) {
            int index = Convert.ToInt32(port.fieldName.Split().Last());
            if (_multiEntryValue.Count < index + 1)
                return null;
            return _multiEntryValue[index];
        }

        public override void SetContext(T context) {
            foreach (U value in MultiValue) {
                if (value == null) continue;
                SetMultiContext(context, value);
            }
        }

        public abstract void SetMultiContext(T context, U value);
        
    }
}

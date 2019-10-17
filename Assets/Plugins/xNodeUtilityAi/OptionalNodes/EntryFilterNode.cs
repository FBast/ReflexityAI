using System;
using System.Linq;
using Plugins.xNodeUtilityAi.AbstractNodes;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.OptionalNodes {
    public class EntryFilterNode : MiddleNode {

        public enum FilterType {
            Sum,
            Max,
            Min
        }
        
        public FilterType Filter;
        public bool Negate;
        
        [Input(ShowBackingValue.Never)] public int ValuesIn;
        [Output] public int ValueOut;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == "ValueOut") {
                int value = 0;
                NodePort valuesPort = GetInputPort("ValuesIn");
                if (valuesPort.IsConnected) {
                    int[] floats = valuesPort.GetInputValues<int>();
                    switch (Filter) {
                        case FilterType.Sum:
                            value = floats.Sum();
                            break;
                        case FilterType.Max:
                            value = Mathf.Max(floats);
                            break;
                        case FilterType.Min:
                            value = Mathf.Min(floats);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                if (Negate) value = -value;
                return value;
            }
            return null;
        }
        
    }

}
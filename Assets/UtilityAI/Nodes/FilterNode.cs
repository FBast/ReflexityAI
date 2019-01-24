using System;
using System.Linq;
using UnityEngine;
using XNode;

namespace UtilityAI.Nodes {
    public class FilterNode : MiddleNode {

        public enum FunctionType {
            Average,
            Max,
            Min
        }

        public FunctionType Function;
        [Input(ShowBackingValue.Never)] public float UtilitiesIn;
        [Output] public float UtilityOut;

        public override object GetValue(NodePort port) {
            if (port.fieldName == "UtilityOut") {
                float[] floats = GetInputValues<float>("UtilitiesIn");
                switch (Function) {
                    case FunctionType.Average:
                        return floats.Average();
                    case FunctionType.Max:
                        return Mathf.Max(floats);
                    case FunctionType.Min:
                        return Mathf.Min(floats);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return null;
        }
        
    }

}
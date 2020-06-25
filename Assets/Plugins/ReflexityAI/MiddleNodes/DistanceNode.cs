using Plugins.ReflexityAI.Framework;
using UnityEngine;
using XNode;

namespace Plugins.ReflexityAI.MiddleNodes {
    public class DistanceNode : MiddleNode {
        
        [Input(ShowBackingValue.Never)] public Vector3 FirstValueIn;
        [Input(ShowBackingValue.Never)] public Vector3 SecondValueIn;
        [Output] public float ValueOut;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(ValueOut)) {
                return Vector3.Distance(GetInputValue<Vector3>(nameof(FirstValueIn)), GetInputValue<Vector3>(nameof(SecondValueIn)));
            }
            return null;
        }
        
    }
}
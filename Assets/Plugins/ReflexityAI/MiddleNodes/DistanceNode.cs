using Plugins.ReflexityAI.Framework;
using UnityEngine;
using XNode;

namespace Plugins.ReflexityAI.MiddleNodes {
    [CreateNodeMenu("Reflexity/Middle/Distance")]
    public class DistanceNode : MiddleNode {
        
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)] public Vector3 FirstValueIn;
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)] public Vector3 SecondValueIn;
        [Output] public float ValueOut;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(ValueOut)) {
                ReflectionData firstValue = GetInputValue<ReflectionData>(nameof(FirstValueIn));
                ReflectionData secondValue = GetInputValue<ReflectionData>(nameof(SecondValueIn));
                if (firstValue.Value != null && secondValue.Value != null)
                    return Vector3.Distance((Vector3) firstValue.Value, (Vector3) secondValue.Value);
                return null;
            }
            return null;
        }
        
    }
}
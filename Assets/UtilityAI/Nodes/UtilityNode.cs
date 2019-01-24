using UnityEngine;
using XNode;

namespace UtilityAI.Nodes {
    public class UtilityNode : MiddleNode {
        
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public float MinX;
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public float MaxX;
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public float X;

        public AnimationCurve Function = AnimationCurve.Linear(0, 0, 1, 1);

        [Output] public float Utility;
        
        public override object GetValue(NodePort port) {
            
            float minX = GetInputValue("MinX", MinX);
            float maxX = GetInputValue("MaxX", MaxX);
            float x = GetInputValue("X", X);

            Utility = 0f;
            if (port.fieldName == "Utility") {
                float scaledX = ScaleX(minX, maxX, x);
                Utility = Function.Evaluate(scaledX);
            }
            return Utility;
        }
        
        protected float ScaleX(float MinValue, float MaxValue, float x) {
            if (MinValue - MaxValue == 0) return 0;
            if (x < MinValue) x = MinValue;
            if (x > MaxValue) x = MaxValue;
            return (x - MinValue) / (MaxValue - MinValue);
        }
        
    }

}

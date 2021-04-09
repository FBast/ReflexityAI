using System;
using Plugins.Reflexity.Framework;
using UnityEngine;
using XNode;

namespace Plugins.Reflexity.MainNodes {
    [CreateNodeMenu("Reflexity/Main/Utility")]
    public class UtilityNode : MiddleNode {
        
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override), Tooltip("Scale the 0 on the X axe")] 
        public float MinX;
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override), Tooltip("Evaluated between Min X and Max X")] 
        public float X;
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override), Tooltip("Scale the 1 on the X axe")] 
        public float MaxX = 1;
        [Tooltip("Evaluate the Utility Y using the X values")]
        public AnimationCurve Function = AnimationCurve.Linear(0, 0, 1, 1);
        [Output(connectionType: ConnectionType.Override), Tooltip("Connect to the Option Node")] public int Rank;
        public float MaxY = 5;
        public float MinY = -5;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(Rank)) {
                float minX = GetInputValue(nameof(MinX), MinX);
                float maxX = GetInputValue(nameof(MaxX), MaxX);
                float x = GetInputValue(nameof(X), X);
                float scaledX = ScaleX(minX, maxX, x);
                return ScaleY(MinY, MaxY, Function.Evaluate(scaledX));
            }
            return null;
        }
        
        private float ScaleX(float minX, float maxX, float x) {
            if (Math.Abs(minX - maxX) <= 0) return 0;
            if (x < minX) x = minX;
            if (x > maxX) x = maxX;
            return (x - minX) / (maxX - minX);
        }
        
        private int ScaleY(float minY, float maxY, float y) {
            if (Math.Abs(minY - maxY) <= 0) return 0;
            return (int) ((maxY - minY) * y + minY);
        }

    }
}

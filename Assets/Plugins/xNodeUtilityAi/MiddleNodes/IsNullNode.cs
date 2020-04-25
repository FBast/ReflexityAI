using Plugins.xNodeUtilityAi.Framework;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.MiddleNodes {
    public class IsNullNode : MiddleNode {
        
        [Input(ShowBackingValue.Never, ConnectionType.Override)] public Object ValueIn;
        [Output] public bool ValueOut;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(ValueOut)) {
                ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(ValueIn));
                return reflectionData.Value == null;
            }
            return null;
        }
        
    }
}
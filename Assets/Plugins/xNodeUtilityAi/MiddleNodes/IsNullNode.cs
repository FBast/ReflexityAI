using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.MiddleNodes {
    public class IsNullNode : MiddleNode {
        
        [Input(ShowBackingValue.Never, ConnectionType.Override)] public Object ValueIn;
        [Output(connectionType: ConnectionType.Override)] public bool ValueOut;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == "ValueOut") {
                return GetInputValue<Object>("ValueIn") == null;
            }
            return null;
        }
        
    }
}
using Plugins.ReflexityAI.Framework;
using XNode;

namespace Plugins.ReflexityAI.MiddleNodes {
    public class NotNode : MiddleNode {
        
        [Input(ShowBackingValue.Never, ConnectionType.Override)] public bool ValueIn;
        [Output(connectionType: ConnectionType.Override)] public bool ValueOut;

        public override object GetValue(NodePort port) {
            if (port.fieldName == "ValueOut") {
                return !GetInputValue<bool>("ValueIn");
            }
            return null;
        }
        
    }

}
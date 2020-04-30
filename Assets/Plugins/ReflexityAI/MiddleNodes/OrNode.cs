using System.Linq;
using XNode;

namespace Plugins.ReflexityAI.MiddleNodes {
    public class OrNode : MiddleNode {
        
        [Input(ShowBackingValue.Never)] public bool ValuesIn;
        [Output(connectionType: ConnectionType.Override)] public bool ValueOut;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == "ValueOut") {
                bool[] values = GetInputValues<bool>("ValuesIn");
                return values.Any(value => value);
            }
            return null;
        }
        
    }
}
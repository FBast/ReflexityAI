using System.Linq;
using Plugins.ReflexityAI.Framework;
using XNode;

namespace Plugins.ReflexityAI.MiddleNodes {
    public class OrNode : MiddleNode {
        
        [Input(ShowBackingValue.Never)] public bool ValuesIn;
        [Output] public bool ValueOut;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(ValueOut)) {
                bool[] values = GetInputValues<bool>(nameof(ValuesIn));
                return values.Any(value => value);
            }
            return null;
        }
        
    }
}
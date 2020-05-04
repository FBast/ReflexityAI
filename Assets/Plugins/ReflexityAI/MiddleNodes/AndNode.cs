using System.Linq;
using Plugins.ReflexityAI.Framework;
using XNode;

namespace Plugins.ReflexityAI.MiddleNodes {
    public class AndNode : MiddleNode {
        
        [Input(ShowBackingValue.Never)] public bool ValuesIn;
        [Output] public bool ValueOut;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == "ValueOut") {
                bool[] values = GetInputValues<bool>("ValuesIn");
                return values.All(value => value);
            }
            return null;
        }
        
    }
}
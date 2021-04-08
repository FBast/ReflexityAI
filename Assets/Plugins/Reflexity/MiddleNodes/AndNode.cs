using System.Linq;
using Plugins.Reflexity.Framework;
using XNode;

namespace Plugins.Reflexity.MiddleNodes {
    [CreateNodeMenu("Reflexity/Middle/And")]
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
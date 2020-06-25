using System.Linq;
using Plugins.ReflexityAI.Framework;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.ReflexityAI.MiddleNodes {
    public class IsEqualNode : MiddleNode {
        
        [Input(ShowBackingValue.Never)] public Object ValuesIn;
        [Output] public bool ValueOut;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(ValueOut)) {
                ReflectionData[] list = GetInputValues<ReflectionData>(nameof(ValuesIn));
                if (list.Length > 0) return list.All(tuple => tuple.Value == list[0].Value);
            }
            return null;
        }
        
    }
}
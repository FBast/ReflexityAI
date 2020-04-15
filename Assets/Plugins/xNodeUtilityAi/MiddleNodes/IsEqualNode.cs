using System.Linq;
using Plugins.xNodeUtilityAi.Framework;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.MiddleNodes {
    public class IsEqualNode : MiddleNode {
        
        [Input(ShowBackingValue.Never)] public Object ValuesIn;
        [Output] public bool ValueOut;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(ValueOut)) {
                ReflectionData[] list = GetInputValues<ReflectionData>(nameof(ValuesIn));
                if (list.Length > 0) return list.All(tuple => tuple.Content == list[0].Content);
            }
            return null;
        }
        
    }
}
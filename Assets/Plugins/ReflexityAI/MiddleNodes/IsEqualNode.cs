using System.Linq;
using Plugins.Reflexity.Framework;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.Reflexity.MiddleNodes {
    [CreateNodeMenu("Reflexity/Middle/IsEqual")]
    public class IsEqualNode : MiddleNode {
        
        [Input(ShowBackingValue.Never)] public Object ValuesIn;
        [Output] public bool ValueOut;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(ValueOut)) {
                ReflectionData[] list = GetInputValues<ReflectionData>(nameof(ValuesIn));
                if (list.Length > 0) return list.All(reflectionData => reflectionData.Value == list[0].Value);
            }
            return null;
        }
        
    }
}
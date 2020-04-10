using System.Linq;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.MiddleNodes {
    public class IsEqualNode : MiddleNode {
        
        [Input(ShowBackingValue.Never)] public Object ValuesIn;
        [Output] public bool ValueOut;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(ValueOut)) {
                Object[] list = GetInputValues<Object>(nameof(ValuesIn));
                return list.Any(o => o == list[0]);
            }
            return null;
        }
        
    }
}
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.AbstractNodes {
    [NodeTint(255, 120, 120), NodeWidth(400)]
    public abstract class ActionNode : Node {
        
        [Input(ShowBackingValue.Never)] public Object Data;
        [Output(connectionType: ConnectionType.Override)] public ActionNode LinkedOption;
        public int Order;
        
        public abstract void Execute(AbstractAIComponent context, Object data);

        public Object GetData() {
            return GetInputPort(nameof(Data)) != null ? GetInputValue<Object>(nameof(Data)) : null;
        }
        
        public override object GetValue(NodePort port) {
            return port.fieldName == nameof(LinkedOption) ? this : null;
        }
        
    }
}
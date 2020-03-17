using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.MiddleNodes;
using XNode;

namespace Plugins.xNodeUtilityAi.MainNodes {
    public class ConverterNode : MiddleNode {
        
        [Input(ShowBackingValue.Never, ConnectionType.Override)] 
        public bool ValueIn;
        public int IsTrue;
        public int IsFalse;
        [Output(connectionType: ConnectionType.Override)]
        public int ValueOut;

        public override object GetValue(NodePort port) {
            if (port.fieldName == "ValueOut") {
                bool valueIn = GetInputValue("ValueIn", ValueIn);
                return valueIn ? IsTrue : IsFalse;
            }
            return null;
        }
        
    }
}
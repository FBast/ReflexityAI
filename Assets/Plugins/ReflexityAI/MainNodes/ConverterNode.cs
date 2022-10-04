using Plugins.ReflexityAI.Framework;
using XNode;

namespace Plugins.ReflexityAI.MainNodes {
    [CreateNodeMenu("Reflexity/Main/Converter")]
    public class ConverterNode : MiddleNode {
        
        [Input(ShowBackingValue.Never, ConnectionType.Override)] 
        public bool ValueIn;
        public int IsTrue;
        public int IsFalse;
        [Output(connectionType: ConnectionType.Override)]
        public int ValueOut;

        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(ValueOut)) {
                bool valueIn = GetInputValue<bool>(nameof(ValueIn));
                return valueIn ? IsTrue : IsFalse;
            }
            return null;
        }
        
    }
}
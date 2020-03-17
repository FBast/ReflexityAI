using Plugins.xNodeUtilityAi.Framework;
using XNode;

namespace Plugins.xNodeUtilityAi.AbstractNodes {
    public abstract class EntryBoolNode : EntryNode {
        
        [Output(connectionType: ConnectionType.Override)] public bool Value;
        
        protected abstract bool ValueProvider(AbstractAIComponent context);
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == "Value") {
                if (_context == null) return null;
                return ValueProvider(_context);
            }
            return null;
        }
        
    }
}
using Plugins.xNodeUtilityAi.Framework;
using XNode;

namespace Plugins.xNodeUtilityAi.AbstractNodes {
    public abstract class SimpleEntryNode : EntryNode {
        
        [Output(connectionType: ConnectionType.Override)] public int Value;
        
        protected abstract int ValueProvider(AbstractAIComponent context);
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == "Value") {
                if (_context == null) return null;
                return ValueProvider(_context);
            }
            return null;
        }

    }
}

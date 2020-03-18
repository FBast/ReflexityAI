using Plugins.xNodeUtilityAi.Framework;
using XNode;

namespace Plugins.xNodeUtilityAi.AbstractNodes.DataNodes {
    public abstract class SimpleDataNode : DataNode {

        [Output] public object DataOut;
        
        protected abstract object ValueProvider(AbstractAIComponent context);
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(DataOut)) {
                if (_context == null) return null;
                object data = ValueProvider(_context);
                if (data != null) return data;
            }
            return null;
        }

    }
}
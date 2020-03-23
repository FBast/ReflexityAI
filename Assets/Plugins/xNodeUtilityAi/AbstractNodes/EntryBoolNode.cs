using Plugins.xNodeUtilityAi.Framework;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.AbstractNodes {
    public abstract class EntryBoolNode : EntryNode {
        
        [Input(ShowBackingValue.Never, ConnectionType.Override)] public Object Data;
        [Output(connectionType: ConnectionType.Override)] public bool Value;
        
        protected abstract bool ValueProvider(AbstractAIComponent context);
        
        protected T GetData<T>() where T : class {
            return GetInputValue<T>(nameof(Data));
        }
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == "Value") {
                if (graph is AIBrainGraph brainGraph && brainGraph.CurrentContext != null) 
                    return ValueProvider(brainGraph.CurrentContext);
                return null;
            }
            return null;
        }
        
    }
}
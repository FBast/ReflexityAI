using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.Framework;
using XNode;

namespace Plugins.xNodeUtilityAi.AbstractNodes {
    [NodeTint(255, 120, 120), NodeWidth(400)]
    public abstract class ActionNode : Node {
        
        [Input(ShowBackingValue.Never)] public TaggedData Data;
        [Output(connectionType: ConnectionType.Override)] public ActionNode LinkedOption;
        public int Order;
        
        protected AbstractAIComponent _context;
        
        public abstract void Execute(AbstractAIComponent context, AIData aiData);
        
        public void SetContext(AbstractAIComponent context) {
            _context = context;
        }

        public List<TaggedData> GetData() {
            if (GetInputPort("Data") != null) {
                List<TaggedData> taggedDatas = GetInputValues<TaggedData>("Data").ToList();
                return taggedDatas;
            }
            return null;
        }
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == "LinkedOption")
                return this;
            return null;
        }
        
    }
}
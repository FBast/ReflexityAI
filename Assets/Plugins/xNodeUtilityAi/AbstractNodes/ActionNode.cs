using Plugins.xNodeUtilityAi.Framework;
using XNode;

namespace Plugins.xNodeUtilityAi.AbstractNodes {
    [NodeTint(255, 120, 120), NodeWidth(400)]
    public abstract class ActionNode : Node {
        
        protected AbstractAIComponent _context;
        [Output(connectionType: ConnectionType.Override)] public ActionNode LinkedOption;
        public int Order;
        
        public void SetContext(AbstractAIComponent context) {
            _context = context;
        }

        public abstract void Execute(AbstractAIComponent context, AIData aiData);

    }
}
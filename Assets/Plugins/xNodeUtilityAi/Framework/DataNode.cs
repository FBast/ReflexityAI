using XNode;

namespace Plugins.xNodeUtilityAi.Framework {
    [NodeTint(120, 120, 120), NodeWidth(400)]
    public abstract class DataNode : Node {
        
        protected AbstractAIComponent _context;
        
        public void SetContext(AbstractAIComponent context) {
            _context = context;
        }
        
    }
}
using NodeUtilityAi.Framework;
using XNode;

namespace NodeUtilityAi.AbstractNodes {
    [NodeTint(120, 255, 120), NodeWidth(400)]
    public abstract class EntryNode : Node {
        
        protected AbstractAIComponent _context;
        
        public void SetContext(AbstractAIComponent context) {
            _context = context;
        }
        
    }
}
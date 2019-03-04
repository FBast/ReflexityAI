using XNode;

namespace NodeUtilityAi.Nodes {
    [NodeTint(120, 255, 120), NodeWidth(400)]
    public abstract class EntryNode : Node {
        
        protected AbstractAIComponent _context;
        
        public void SetContext(AbstractAIComponent context) {
            _context = context;
        }
        
    }
}
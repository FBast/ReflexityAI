using XNode;

namespace UtilityAI.Nodes {
    [NodeTint(100, 255, 100)]
    public abstract class EntryNode<T> : Node {
        
        public abstract void SetContext(T context);
        
    }
}
using XNode;

namespace UtilityAI.Nodes {
    [NodeTint(255, 100, 100)]
    public abstract class ExitNode<T> : Node {
        
        public abstract void Execute(T context);
        
    }
}
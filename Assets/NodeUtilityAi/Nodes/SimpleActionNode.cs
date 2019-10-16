using XNode;

namespace NodeUtilityAi.Nodes {
    public abstract class SimpleActionNode : ActionNode {

        public override object GetValue(NodePort port) {
            if (port.fieldName == "LinkedOption")
                return this;
            return null;
        }
        
    }

}
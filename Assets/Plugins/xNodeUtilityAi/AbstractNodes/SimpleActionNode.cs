using XNode;

namespace Plugins.xNodeUtilityAi.AbstractNodes {
    public abstract class SimpleActionNode : ActionNode {

        public override object GetValue(NodePort port) {
            if (port.fieldName == "LinkedOption")
                return this;
            return null;
        }
        
    }

}
using XNode;

namespace NodeUtilityAi.Nodes {
    public class UtilityInverterNode : MiddleNode {
        
        [Input(ShowBackingValue.Never, ConnectionType.Override)] public float UtilityIn;
        [Output] public float UtilityOut;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == "UtilityOut") {
                return 1 - GetInputValue<float>("UtilityIn");
            }
            return null;
        }
        
    }

}
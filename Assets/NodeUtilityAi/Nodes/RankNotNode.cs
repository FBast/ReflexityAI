using XNode;

namespace NodeUtilityAi.Nodes {
    public class RankNotNode : MiddleNode {
        
        [Input(ShowBackingValue.Never, ConnectionType.Override)] public int RankIn;
        [Output(connectionType: ConnectionType.Override)] public int RankOut;

        public override object GetValue(NodePort port) {
            if (port.fieldName == "RankOut") {
                return GetInputValue<int>("RankIn") == 0 ? 1 : 0;
            }
            return null;
        }
        
    }

}
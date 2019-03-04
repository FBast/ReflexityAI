using System.Linq;
using XNode;

namespace NodeUtilityAi.Nodes {
    public class RankAndNode : MiddleNode {
        
        [Input(ShowBackingValue.Never)] public int RankIn;
        [Output(connectionType: ConnectionType.Override)] public int RankOut;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == "RankOut") {
                int[] values = GetInputValues<int>("RankIn");
                return values.Contains(0) ? 0 : values.Max();
            }
            return null;
        }
        
    }
}
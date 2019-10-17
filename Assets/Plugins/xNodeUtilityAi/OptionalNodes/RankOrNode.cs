using System.Linq;
using Plugins.xNodeUtilityAi.AbstractNodes;
using XNode;

namespace Plugins.xNodeUtilityAi.OptionalNodes {
    public class RankOrNode : MiddleNode {
        
        [Input(ShowBackingValue.Never)] public int RankIn;
        [Output(connectionType: ConnectionType.Override)] public int RankOut;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == "RankOut") {
                int[] values = GetInputValues<int>("RankIn");
                return values.Max();
            }
            return null;
        }
        
    }
}
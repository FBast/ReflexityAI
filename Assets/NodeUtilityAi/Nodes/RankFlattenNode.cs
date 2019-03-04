using System.Linq;
using XNode;

namespace NodeUtilityAi.Nodes {
    public class RankFlattenNode : MiddleNode {

        public int FlattenValue = 1;
    
        [Input(ShowBackingValue.Never, ConnectionType.Override)] public int ValueIn;
        [Output] public int ValueOut;

        public override object GetValue(NodePort port) {
            if (port.fieldName == "ValueOut") {
                NodePort valuePort = GetInputPort("ValueIn");
                if (valuePort.IsConnected) {
                    int[] value = valuePort.GetInputValues<int>();
                    return value.Sum() > 0 ? FlattenValue : 0;
                }
            }
            return null;
        }
    }
}
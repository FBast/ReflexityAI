using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.Framework;

namespace Plugins.xNodeUtilityAi.AbstractNodes {
    public abstract class DataActionNode : SimpleActionNode {
        
        [Input(ShowBackingValue.Never)] public TaggedData Data;
        
        public List<TaggedData> GetData() {
            if (GetInputPort("Data") != null) {
                List<TaggedData> taggedDatas = GetInputValues<TaggedData>("Data").ToList();
                return taggedDatas;
            }
            return null;
        }
        
    }
}
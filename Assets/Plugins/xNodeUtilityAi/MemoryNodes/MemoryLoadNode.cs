using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils;
using Plugins.xNodeUtilityAi.Utils.TagList;

namespace Plugins.xNodeUtilityAi.MemoryNodes {
    public class MemoryLoadNode : SimpleDataNode {

        [DropdownList(typeof(TagListHelper), nameof(TagListHelper.GetMemoryTags))] 
        public string MemoryTag;
        
        protected override object ValueProvider(AbstractAIComponent context) {
            return context.LoadFromMemory(MemoryTag);
        }

        public override object GetReflectedValue(string portName) {
            throw new System.NotImplementedException();
        }

        public override object GetFullValue(string portName) {
            throw new System.NotImplementedException();
        }
    }
}
using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils.TagList;

namespace Plugins.xNodeUtilityAi.MemoryNodes {
    public class MemoryLoadNode : SimpleDataNode {

        [TagListProperty(typeof(TagListHelper), nameof(TagListHelper.GetMemoryTags))] 
        public string MemoryTag;
        
        protected override object ValueProvider(AbstractAIComponent context) {
            return context.LoadFromMemory(MemoryTag);
        }

    }
}
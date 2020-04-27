using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils.TagList;

namespace Plugins.xNodeUtilityAi.MemoryNodes {
    public class MemoryLoadNode : SimpleDataNode {

        [DropdownList(typeof(TagListHelper), nameof(TagListHelper.GetMemoryTags))] 
        public string MemoryTag;
        
        protected override object ValueProvider(ReflexityAI context) {
            return context.LoadFromMemory(MemoryTag);
        }

    }
}
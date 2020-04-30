using Plugins.ReflexityAI.Framework;
using Plugins.ReflexityAI.Utils.TagList;

namespace Plugins.ReflexityAI.MemoryNodes {
    public class MemoryLoadNode : SimpleDataNode {

        [DropdownList(typeof(TagListHelper), nameof(TagListHelper.GetMemoryTags))] 
        public string MemoryTag;
        
        protected override object ValueProvider(Framework.ReflexityAI context) {
            return context.LoadFromMemory(MemoryTag);
        }

    }
}
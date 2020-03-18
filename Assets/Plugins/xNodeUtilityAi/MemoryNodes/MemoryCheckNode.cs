using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils.TagList;

namespace Plugins.xNodeUtilityAi.MemoryNodes {
    public class MemoryCheckNode : EntryBoolNode {
        
        [TagListProperty(typeof(TagListHelper), nameof(TagListHelper.GetMemoryTags))] 
        public string MemoryTag;
        
        protected override bool ValueProvider(AbstractAIComponent context) {
            return context.LoadFromMemory(MemoryTag) != null;
        }
        
    }
}
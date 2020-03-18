using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils.TagList;

namespace Plugins.xNodeUtilityAi.MemoryNodes {
    public class MemoryClearNode : ActionNode {

        [TagListProperty(typeof(TagListHelper), nameof(TagListHelper.GetMemoryTags))] 
        public string MemoryTag;
        
        public override void Execute(AbstractAIComponent context, AIData aiData) {
            context.ClearFromMemory(MemoryTag);
        }
        
    }
}
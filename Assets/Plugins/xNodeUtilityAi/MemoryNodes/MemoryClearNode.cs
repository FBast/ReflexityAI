using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils.TagList;
using UnityEngine;

namespace Plugins.xNodeUtilityAi.MemoryNodes {
    public class MemoryClearNode : ActionNode {

        [TagListProperty(typeof(TagListHelper), nameof(TagListHelper.GetMemoryTags))] 
        public string MemoryTag;
        
        public override void Execute(AbstractAIComponent context, Object data) {
            context.ClearFromMemory(MemoryTag);
        }
        
    }
}
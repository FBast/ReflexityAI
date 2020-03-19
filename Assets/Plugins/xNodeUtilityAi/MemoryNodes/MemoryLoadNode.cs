using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils.TagList;
using UnityEngine;

namespace Plugins.xNodeUtilityAi.AbstractNodes.DataNodes {
    public class MemoryLoadNode : SimpleDataNode {

        [TagListProperty(typeof(TagListHelper), nameof(TagListHelper.GetMemoryTags))] 
        public string MemoryTag;
        
        protected override object ValueProvider(AbstractAIComponent context) {
            return context.LoadFromMemory(MemoryTag);
        }

    }
}
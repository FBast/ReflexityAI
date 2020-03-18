using System;
using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils.TagList;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.MemoryNodes {
    public class MemorySaveNode : ActionNode {

        [TagListProperty(typeof(TagListHelper), nameof(TagListHelper.GetMemoryTags))] 
        public string MemoryTag;
        
        public override void Execute(AbstractAIComponent context, Object data) {
            if (string.IsNullOrEmpty(MemoryTag)) 
                throw new Exception("MemorySaveNode contain no dataTag, please select one");
            context.SaveInMemory(MemoryTag, data);
        }

    }

}
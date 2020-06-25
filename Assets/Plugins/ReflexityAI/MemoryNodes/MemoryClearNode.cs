using Plugins.ReflexityAI.Framework;
using Plugins.ReflexityAI.Utils.TagList;

namespace Plugins.ReflexityAI.MemoryNodes {
    public class MemoryClearNode : ActionNode {

        [DropdownList(typeof(TagListHelper), nameof(TagListHelper.GetMemoryTags))] 
        public string MemoryTag;

        public override void Execute(object context, object[] parameters) {
            // context.ClearFromMemory(MemoryTag);
        }

        public override object GetContext() {
            throw new System.NotImplementedException();
        }

        public override object[] GetParameters() {
            throw new System.NotImplementedException();
        }
        
    }
}
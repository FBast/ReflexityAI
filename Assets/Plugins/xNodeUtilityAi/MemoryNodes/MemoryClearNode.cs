using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils.TagList;

namespace Plugins.xNodeUtilityAi.MemoryNodes {
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
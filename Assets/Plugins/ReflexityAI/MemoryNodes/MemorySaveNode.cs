using System;
using Plugins.ReflexityAI.Framework;
using Plugins.ReflexityAI.Utils.TagList;

namespace Plugins.ReflexityAI.MemoryNodes {
    public class MemorySaveNode : ActionNode {

        [DropdownList(typeof(TagListHelper), nameof(TagListHelper.GetMemoryTags))] 
        public string MemoryTag;

        public override void Execute(object context, object[] parameters) {
            // if (string.IsNullOrEmpty(MemoryTag)) 
            //     throw new Exception("MemorySaveNode contain no dataTag, please select one");
            // context.SaveInMemory(MemoryTag, data);
        }

        public override object GetContext() {
            throw new NotImplementedException();
        }

        public override object[] GetParameters() {
            throw new NotImplementedException();
        }
    }

}
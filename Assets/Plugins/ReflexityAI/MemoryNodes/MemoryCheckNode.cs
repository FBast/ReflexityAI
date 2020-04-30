using Plugins.ReflexityAI.Framework;
using Plugins.ReflexityAI.Utils.TagList;
using XNode;

namespace Plugins.ReflexityAI.MemoryNodes {
    public class MemoryCheckNode : EntryNode, IContextual {
        
        [Output(connectionType: ConnectionType.Override)] public bool Value;
        [DropdownList(typeof(TagListHelper), nameof(TagListHelper.GetMemoryTags))] public string MemoryTag;
        
        public Framework.ReflexityAI Context { get; set; }
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(Value) && Context != null) {
                return Context.LoadFromMemory(MemoryTag) != null;
            }
            return null;
        }

    }
}
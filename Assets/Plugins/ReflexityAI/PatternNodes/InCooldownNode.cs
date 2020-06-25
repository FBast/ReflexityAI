using Plugins.ReflexityAI.Framework;
using Plugins.ReflexityAI.Utils.TagList;
using UnityEngine;
using XNode;

namespace Plugins.ReflexityAI.PatternNodes {
    public class InCooldownNode : EntryNode, IContextual {

        [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public float Cooldown;
        [Output(connectionType: ConnectionType.Override)] public bool Value;
        [DropdownList(typeof(TagListHelper), nameof(TagListHelper.GetHistoricTags))] public string HistoricTag;
        
        public Framework.ReflexityAI Context { get; set; }
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(Value) && Context != null) {
                float cooldown = GetInputValue(nameof(Cooldown), Cooldown);
                return Time.realtimeSinceStartup - Context.HistoricTime(HistoricTag) <= cooldown;
            }
            return null;
        }

    }
}
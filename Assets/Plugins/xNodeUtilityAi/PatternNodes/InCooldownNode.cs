using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils.TagList;
using UnityEngine;

namespace Plugins.xNodeUtilityAi.PatternNodes {
    public class InCooldownNode : EntryBoolNode {

        [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] 
        public float Cooldown;
        [TagListProperty(typeof(TagListHelper), nameof(TagListHelper.GetHistoricTags))] 
        public string HistoricTag;

        protected override bool ValueProvider(AbstractAIComponent context) {
            float cooldown = GetInputValue(nameof(Cooldown), Cooldown);
            return Time.realtimeSinceStartup - context.HistoricTime(HistoricTag) <= cooldown;
        }
        
    }
}
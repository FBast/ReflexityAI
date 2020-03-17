using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Plugins.xNodeUtilityAi.PatternNodes {
    public class InCooldownNode : EntryBoolNode {

        public string HistoricTag;
        public float Cooldown;

        protected override bool ValueProvider(AbstractAIComponent context) {
            return Time.realtimeSinceStartup - context.HistoricTime(HistoricTag) <= Cooldown;
        }
        
    }
}
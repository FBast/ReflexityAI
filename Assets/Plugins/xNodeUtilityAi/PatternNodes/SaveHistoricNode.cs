using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils.TagList;
using UnityEngine;

namespace Plugins.xNodeUtilityAi.PatternNodes {
    public class SaveHistoricNode : ActionNode {

        [TagListProperty(typeof(TagListHelper), nameof(TagListHelper.GetHistoricTags))]
        public string HistoricTag;

        public override void Execute(AbstractAIComponent context, Object data) {
            context.SaveInHistoric(HistoricTag);
        }
        
    }
}
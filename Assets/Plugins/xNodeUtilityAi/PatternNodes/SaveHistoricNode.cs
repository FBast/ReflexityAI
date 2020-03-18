using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils.TagList;

namespace Plugins.xNodeUtilityAi.PatternNodes {
    public class SaveHistoricNode : ActionNode {

        [TagListProperty(typeof(TagListHelper), nameof(TagListHelper.GetHistoricTags))] 
        public string HistoricTag;
        
        public override void Execute(AbstractAIComponent context, AIData aiData) {
            context.SaveInHistoric(HistoricTag);
        }
        
    }
}
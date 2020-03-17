using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;

namespace Plugins.xNodeUtilityAi.PatternNodes {
    public class SaveHistoricNode : ActionNode {

        public string HistoricTag;
        
        public override void Execute(AbstractAIComponent context, AIData aiData) {
            context.SaveInHistoric(HistoricTag);
        }
        
    }
}
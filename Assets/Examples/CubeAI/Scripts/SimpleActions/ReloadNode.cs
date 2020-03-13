using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;

namespace Examples.CubeAI.Scripts.SimpleActions {
    public class ReloadNode : SimpleActionNode {

        public override void Execute(AbstractAIComponent context, AIData aiData) {
            CubeAIComponent cubeAiComponent = (CubeAIComponent) context;
            cubeAiComponent.CubeEntity.Reload();
        }
        
    }
}
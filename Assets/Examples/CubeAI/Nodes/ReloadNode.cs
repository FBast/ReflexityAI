using NodeUtilityAi;
using NodeUtilityAi.AbstractNodes;
using NodeUtilityAi.Framework;

namespace Examples.CubeAI.Nodes {
    public class ReloadNode : SimpleActionNode {

        public override void Execute(AbstractAIComponent context, AIData aiData) {
            CubeAIComponent cubeAiComponent = (CubeAIComponent) context;
            cubeAiComponent.CubeEntity.Reload();
        }
        
    }
}
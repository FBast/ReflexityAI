using NodeUtilityAi;
using NodeUtilityAi.Framework;
using NodeUtilityAi.Nodes;

namespace Examples.CubeAI.Nodes {
    public class ReloadNode : SimpleActionNode {

        public override void Execute(AbstractAIComponent context, AIData aiData) {
            CubeAIComponent cubeAiComponent = (CubeAIComponent) context;
            cubeAiComponent.CubeEntity.Reload();
        }
        
    }
}
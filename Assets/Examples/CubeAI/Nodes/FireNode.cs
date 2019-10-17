using NodeUtilityAi;
using NodeUtilityAi.AbstractNodes;
using NodeUtilityAi.Framework;

namespace Examples.CubeAI.Nodes {
    public class FireNode : SimpleActionNode {

        public override void Execute(AbstractAIComponent context, AIData aiData) {
            CubeAIComponent cubeAiComponent = (CubeAIComponent) context;
            cubeAiComponent.CubeEntity.FireForward();
        }
        
    }
}
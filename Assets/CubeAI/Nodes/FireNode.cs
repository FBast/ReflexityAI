using UtilityAI.Nodes;

namespace CubeAI.Nodes {
    public class FireNode : ActionNode<CubeAiComponent> {

        public override void Execute(CubeAiComponent context) {
            context.CubeEntity.FireForward();
        }
        
    }
}
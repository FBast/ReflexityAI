using UtilityAI.Nodes;

namespace CubeAI.Nodes {
    public class FireNode : ActionNode<CubeEntity> {

        public override void Execute(CubeEntity context) {
            context.FireForward();
        }
        
    }
}
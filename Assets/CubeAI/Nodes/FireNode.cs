using UtilityAI.Nodes;

namespace CubeAI.Nodes {
    public class FireNode : OptionNode<CubeEntity> {

        public override void Execute(CubeEntity context) {
            context.FireForward();
        }
        
    }
}
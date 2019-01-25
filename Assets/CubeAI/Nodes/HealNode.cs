using UtilityAI.Nodes;

namespace CubeAI.Nodes {
    public class HealNode : ActionNode<CubeEntity> {

        public override void Execute(CubeEntity context) {
            context.Heal();
        }
        
    }
}
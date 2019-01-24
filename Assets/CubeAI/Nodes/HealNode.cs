using UtilityAI.Nodes;

namespace CubeAI.Nodes {
    public class HealNode : ActionNode<CubeAiComponent> {

        public override void Execute(CubeAiComponent context) {
            context.CubeEntity.Heal();
        }
        
    }
}
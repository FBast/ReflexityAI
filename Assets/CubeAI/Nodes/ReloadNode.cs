using UtilityAI.Nodes;

namespace CubeAI.Nodes {
    public class ReloadNode : ActionNode<CubeAiComponent> {

        public override void Execute(CubeAiComponent context) {
            context.CubeEntity.Reload();
        }
        
    }
}
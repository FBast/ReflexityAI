using UtilityAI.Nodes;

namespace CubeAI.Nodes {
    public class ReloadNode : ActionNode<CubeEntity> {

        public override void Execute(CubeEntity context) {
            context.Reload();
        }
        
    }
}
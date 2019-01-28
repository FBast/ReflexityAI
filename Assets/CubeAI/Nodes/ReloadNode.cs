using UtilityAI.Nodes;

namespace CubeAI.Nodes {
    public class ReloadNode : OptionNode<CubeEntity> {

        public override void Execute(CubeEntity context) {
            context.Reload();
        }
        
    }
}
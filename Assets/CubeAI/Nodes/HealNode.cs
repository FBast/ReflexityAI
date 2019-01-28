using UtilityAI.Nodes;

namespace CubeAI.Nodes {
    public class HealNode : OptionNode<CubeEntity> {

        public override void Execute(CubeEntity context) {
            context.Heal();
        }
        
    }
}
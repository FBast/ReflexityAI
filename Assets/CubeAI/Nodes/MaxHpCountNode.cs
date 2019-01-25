using UtilityAI.Nodes;

namespace CubeAI.Nodes {
    public class MaxHpCountNode : SimpleEntryNode<CubeEntity> {

        public override void SetContext(CubeEntity context) {
            Value = context.MaxHp;
        }
        
    }
}
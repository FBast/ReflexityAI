using UtilityAI.Nodes;

namespace CubeAI.Nodes {
    public class MaxHpCountNode : SimpleEntryNode<CubeAiComponent> {

        public override void SetContext(CubeAiComponent context) {
            Value = context.CubeEntity.MaxHp;
        }
        
    }
}
using UtilityAI.Nodes;

namespace CubeAI.Nodes {
    public class CurrentHpCountNode : SimpleEntryNode<CubeAiComponent> {

        public override void SetContext(CubeAiComponent context) {
            Value = context.CubeEntity.CurrentHp;
        }
    }
}